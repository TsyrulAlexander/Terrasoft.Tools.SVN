using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terrasoft.Core.Git
{
    public class GitRepository
    {
        public static async Task<GitReleaseInfo> GetLatestReleaseInfoAsync(string repoOwner, string repoName,
            string token = null)
        {
            using (HttpClient client = GetGithubHttpClient(token)) {
                HttpResponseMessage response = await client.GetAsync($"repos/{repoOwner}/{repoName}/releases/latest");
                string bodyString = await response.Content.ReadAsStringAsync();
                var errorMessage = JObject.Parse(bodyString).SelectToken("$.message")?.Value<string>();
                if (!string.IsNullOrWhiteSpace(errorMessage)) {
                    throw new Exception(errorMessage);
                }

                return JsonConvert.DeserializeObject<GitReleaseInfo>(bodyString);
            }
        }

        private static HttpClient GetGithubHttpClient(string token = null)
        {
            var httpClient = new HttpClient {
                BaseAddress = new Uri("https://api.github.com"),
                DefaultRequestHeaders = {{"User-Agent", "Github-API-Test"}}
            };
            if (!string.IsNullOrWhiteSpace(token)) {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            }

            return httpClient;
        }
    }
}