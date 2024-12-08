using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.Data;
using StaffManagerModels;

namespace Exam.Services
{

    public class ApiRequest
    {
        private const string UNKNOWN = "unknown";

        private static readonly HttpClient client = new HttpClient();
        private readonly string urlPrefix;


        public ApiRequest()
        {
            urlPrefix = Config.Configuration.GetSection("ApiAddress").Value;
        }

        public void Post()
        {

        }

        public async Task<string> GetAsync(string url, RequestHeader rtype, string body)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, urlPrefix + url)
            {
                Content = new StringContent(body)
            };

            request.Headers.Add("Request", rtype.Format());


            var responce = await client.SendAsync(request);

            if (responce.StatusCode == HttpStatusCode.OK)
            {
                var responceBody = await responce.Content.ReadAsStringAsync();

                if (!responceBody.Equals(UNKNOWN))
                {
                    return responceBody;
                }
            }

            return string.Empty;
        }
    }
}
