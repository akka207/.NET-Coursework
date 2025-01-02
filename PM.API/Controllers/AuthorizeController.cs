using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PM.API.Data;
using PM.API.JWT;
using PM.API.Services;
using StaffManagerModels;
using System.Linq;

namespace PM.API.Controllers
{
    [ApiController]
    [Route("authorize")]
    public class AuthorizeController : Controller
    {
        private readonly IJWTHandler _JWTHandler;
        private readonly PMAPIContext _context;

        private record Tokens(string jwt, string refreshToken);
        private record AuthData(string login, string hashedPassword);

        public AuthorizeController(IJWTHandler JWTHandler, PMAPIContext context)
        {
            _JWTHandler = JWTHandler;
            _context = context;
        }


        [HttpPost]
        public async Task<string> PostAsync()
        {
            if (Request.Headers["RequestType"] == RequestHeader.LOGIN.Format())
            {
                string body;
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                AuthData? authData = JsonConvert.DeserializeObject<AuthData>(body);

                if (authData != null)
                {
                    Person? person = await _context.Persons.FirstOrDefaultAsync(p => p.Login == authData.login);

                    if (person != null && person.HashedPasword == authData.hashedPassword)
                    {
                        Staff? staff = await _context.Staffs.FirstOrDefaultAsync(s => s.PersonId == person.Id);

                        if (staff != null)
                        {
                            Tokens? tokens = JsonConvert.DeserializeObject<Tokens>(await _JWTHandler.RegisterJWTAsync(staff));

                            if (tokens != null)
                            {
                                Response.Headers.Append("JWT", tokens.jwt);
                                Response.Headers.Append("RefreshToken", tokens.refreshToken);

                                return string.Empty;
                            }
                        }
                    }
                }
            }
            else if (Request.Headers["RequestType"] == RequestHeader.LOGOUT.Format())
            {
                string? jwt = Request.Headers["JWT"];
                if (jwt != null)
                {
                    string validationResult = _JWTHandler.ValidateJWT(jwt);
                    if (validationResult.StartsWith("ERROR"))
                        return validationResult;

                    JWTStoredRecord? record = _context.JWTs.FirstOrDefault(j => string.Compare(j.JWT, jwt) == 0);
                    if (record != null)
                    {
                        _context.JWTs.Remove(record);
                        await _context.SaveChangesAsync();
                        return string.Empty;
                    }
                }
            }
            else if (Request.Headers["RequestType"] == RequestHeader.REFRESH.Format())
            {
                string? jwt = Request.Headers["JWT"];
                string? refreshToken = Request.Headers["RefreshToken"];
                if (jwt != null && refreshToken != null)
                {
                    string validationResult = await _JWTHandler.RefreshAsync(jwt, refreshToken);
                    if (validationResult.StartsWith("ERROR"))
                        return validationResult;

                    Response.Headers.Append("JWT", validationResult);
                    return string.Empty;
                }
            }
            return "ERROR: Invalid request";
        }

    }
}