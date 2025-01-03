using Exam.Data;
using Newtonsoft.Json;
using StaffManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class AuthorizeRequest
    {
        public static string JWT = string.Empty;
        private static readonly HttpClient client = new HttpClient();
        private readonly string urlPrefix;

        private record Tokens(string jwt, string refreshToken);

        public AuthorizeRequest()
        {
            urlPrefix = Config.Configuration["AuthorizeAddress"] ?? string.Empty;
        }

        public async Task<string> LoginAsync(string login, string hashedPassword)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, urlPrefix)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { login, hashedPassword }))
            };

            request.Headers.Add("RequestType", RequestHeader.LOGIN.Format());

            var responce = await client.SendAsync(request);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var responceBody = await responce.Content.ReadAsStringAsync();

                if (!responceBody.StartsWith("ERROR"))
                {
                    string jwt = responce.Headers.GetValues("JWT").FirstOrDefault() ?? string.Empty;
                    string refreshToken = responce.Headers.GetValues("RefreshToken").FirstOrDefault() ?? string.Empty;
                    JWT = jwt;
                    Tokens tokens = new Tokens(jwt, refreshToken);
                    return JsonConvert.SerializeObject(tokens);
                }

                return responceBody;
            }

            return $"ERROR: {responce.StatusCode}";
        }

        public async Task LogoutAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, urlPrefix);

            request.Headers.Add("RequestType", RequestHeader.LOGOUT.Format());
            request.Headers.Add("JWT", JWT);

            var responce = await client.SendAsync(request);

            JWT = string.Empty;
        }

        public async Task<string> RefreshAsync(string jsonTokens)
        {
            var tokens = JsonConvert.DeserializeObject<Tokens>(jsonTokens);

            if (tokens == null)
                return "ERROR: Parameters deserialization failed";

            var request = new HttpRequestMessage(HttpMethod.Post, urlPrefix);

            request.Headers.Add("RequestType", RequestHeader.REFRESH.Format());
            request.Headers.Add("JWT", tokens.jwt);
            request.Headers.Add("RefreshToken", tokens.refreshToken);

            var responce = await client.SendAsync(request);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var responceBody = await responce.Content.ReadAsStringAsync();

                if (responceBody.StartsWith("ERROR"))
                {
                    return responceBody;
                }

                string jwt = responce.Headers.GetValues("JWT").FirstOrDefault() ?? string.Empty;

                tokens = new Tokens(jwt, tokens.refreshToken);

                JWT = jwt;

                return JsonConvert.SerializeObject(tokens);
            }

            return $"ERROR: {responce.StatusCode}";
        }
    }
}
