using Exam.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class JwtApiRequest
    {
        public static JwtApiRequest Instance { get; set; } = new JwtApiRequest();

        private string _url;
        private string _accessTocken = string.Empty;
        private static readonly HttpClient _client = new HttpClient();

        public JwtApiRequest()
        {
            _url = Config.Configuration.GetSection("ApiAddress").Value;
        }

        public async Task<ICollection<object>> HttpGetAsync(string url)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _url + url))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessTocken);

                var responce = await _client.SendAsync(requestMessage);

                return JsonConvert.DeserializeObject(await responce.Content.ReadAsStringAsync()) as ICollection<object>;
            }
        }

        public async Task<ICollection<object>> HttpPostAsync(string url, Dictionary<string, string> values)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, _url + url))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessTocken);
                requestMessage.Content = new FormUrlEncodedContent(values);

                var responce = await _client.SendAsync(requestMessage);

                return JsonConvert.DeserializeObject(await responce.Content.ReadAsStringAsync()) as ICollection<object>;
            }
        }
    }
}
