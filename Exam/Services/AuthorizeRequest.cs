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
        private static string JWT = string.Empty;
        private static readonly HttpClient client = new HttpClient();
        private readonly string urlPrefix;
        private readonly string tokensFile;

        private record Tokens(string jwt, string refreshToken);

        public AuthorizeRequest()
        {
            urlPrefix = Config.Configuration["AuthorizeAddress"];
            tokensFile = Config.Configuration["TokensStorageFile"];
        }

        public async Task LoginAsync(string login, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, urlPrefix)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { login, password }))
            };

            request.Headers.Add("RequestType", RequestHeader.LOGIN.Format());

            var responce = await client.SendAsync(request);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var responceBody = await responce.Content.ReadAsStringAsync();

                UserFileManager.WriteToUserFiles(tokensFile, responceBody);
            }
        }

        public async Task LogoutAsync()
        {
            var tokens = JsonConvert.DeserializeObject<Tokens>(UserFileManager.ReadFromUserFiles(tokensFile));

            if (tokens == null)
                return;

            var request = new HttpRequestMessage(HttpMethod.Post, urlPrefix);

            request.Headers.Add("RequestType", RequestHeader.LOGIN.Format());
            request.Headers.Add("AccessToken", tokens.jwt);

            var responce = await client.SendAsync(request);
        }

        public async Task RefreshAsync()
        {
            var tokens = JsonConvert.DeserializeObject<Tokens>(UserFileManager.ReadFromUserFiles(tokensFile));

            if (tokens == null)
                return;

            var request = new HttpRequestMessage(HttpMethod.Post, urlPrefix);

            request.Headers.Add("RequestType", RequestHeader.LOGIN.Format());
            request.Headers.Add("AccessToken", tokens.jwt);
            request.Headers.Add("RefreshToken", tokens.refreshToken);

            var responce = await client.SendAsync(request);
            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var responceBody = await responce.Content.ReadAsStringAsync();

                tokens = new Tokens(responceBody, tokens.refreshToken);

                UserFileManager.WriteToUserFiles(tokensFile, JsonConvert.SerializeObject(tokens));
            }
        }
    }
}
