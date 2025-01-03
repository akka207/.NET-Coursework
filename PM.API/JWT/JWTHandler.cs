using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Common;
using PM.API.Data;
using PM.API.Services;
using StaffManagerModels;
using System.Configuration;

namespace PM.API.JWT
{
    public class JWTHandler : IJWTHandler
    {
        private readonly PMAPIContext _context;
        private readonly IConfiguration _configuration;
        public JWTHandler(PMAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<string> RegisterJWTAsync(Person person)
        {
            string? alg = _configuration["JWT:Header:alg"];
            string? typ = _configuration["JWT:Header:typ"];
            if (alg == null || typ == null || alg != "HS256" || typ != "JWT")
            {
                return "ERROR: appsettings parameters does not supported";
            }

            string header = JsonConvert.SerializeObject(new { alg, typ });
            string payload = JsonConvert.SerializeObject(new JWTPayload(person.Login,
                DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JWT:ExpirationMins"])),
                Guid.NewGuid().ToString()));

            JWTConfig config = new JWTConfig(header, payload);


            string? secret = _configuration["JWT:Secret"];

            if (secret != null)
            {
                string jwt = config.Encrypt(secret);

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string refreshToken = Convert.ToBase64String(time.Concat(key).ToArray());


                JWTStoredRecord record = new JWTStoredRecord();
                record.JWT = jwt;
                record.RefreshToken = refreshToken;


                await _context.JWTs.AddAsync(record);
                await _context.SaveChangesAsync();


                return JsonConvert.SerializeObject(new { jwt, refreshToken });
            }
            return "ERROR: appsettings does not setuped propertly";
        }

        public string ValidateJWT(string jwt, bool ignoreTime = false)
        {
            JWTConfig config = new JWTConfig();
            string? secret = _configuration["JWT:Secret"];
            if (secret != null)
            {
                if (config.TryDecode(jwt, secret))
                {
                    JWTPayload? payload = JsonConvert.DeserializeObject<JWTPayload>(config.Payload);

                    if (payload != null)
                    {
                        if (payload.exp > DateTime.UtcNow || ignoreTime)
                            return payload.sub;
                        else
                            return "ERROR: Token expired";
                    }
                }
            }
            else
            {
                return "ERROR: appsettings does not setuped propertly";
            }

            return "ERROR: Invalide access token";
        }

        public async Task<string> RefreshAsync(string jwt, string refreshToken)
        {
            byte[] data;
            try
            {
                data = Convert.FromBase64String(refreshToken);
            }
            catch
            {
                return "ERROR: Invalid refresh token";
            }

            DateTime tokenLifetime = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (tokenLifetime < DateTime.UtcNow.AddDays(-_configuration.GetSection("JWT").GetValue<int>("RefreshTokenExpirationDays")))
                return "ERROR: Refresh token expired";

            string? secret = _configuration["JWT:Secret"];
            if (secret == null)
                return "ERROR: appsettings does not setuped propertly";

            JWTConfig config = new JWTConfig();
            if (!config.TryDecode(jwt, secret))
                return "ERROR: Invalid access token";


            JWTStoredRecord? record = await _context.JWTs.FirstOrDefaultAsync(j => j.JWT == jwt);

            if (record == null)
                return "ERROR: Unauthorized";

            _context.JWTs.Remove(record);
            await _context.SaveChangesAsync();


            JWTPayload? payload = JsonConvert.DeserializeObject<JWTPayload>(config.Payload);

            if (payload != null)
            {

                string newPayload = JsonConvert.SerializeObject(new JWTPayload(payload.sub,
                    DateTime.UtcNow.AddMinutes(_configuration.GetSection("JWT").GetValue<int>("ExpirationMins")),
                    Guid.NewGuid().ToString()));

                config.Payload = newPayload;

                record.JWT = config.Encrypt(secret);

                await _context.JWTs.AddAsync(record);
                await _context.SaveChangesAsync();

                return record.JWT;
            }

            return "ERROR: Invalid access token";
        }

        //public string GetSub(string jwt)
        //{
        //    JWTConfig config = new JWTConfig();
        //    string? secret = _configuration["JWT:Secret"];

        //    if (secret == null)
        //    {
        //        return "ERROR: appsettings does not setuped propertly";
        //    }

        //    config.TryDecode(jwt, secret);

        //    JWTPayload? payload = JsonConvert.DeserializeObject<JWTPayload>(config.Payload);

        //    if (payload != null)
        //    {
        //        return payload.sub.ToString();
        //    }

        //    return "ERROR: Invalid access tocken";
        //}
    }
}
