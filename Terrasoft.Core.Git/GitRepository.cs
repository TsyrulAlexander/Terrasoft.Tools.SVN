using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Terrasoft.Core.Git {
	public class GitRepository {
		public static async Task<GitReleaseInfo> GetLatestReleaseInfo(string repoOwner, string repoName) {
			using (var client = GetGithubHttpClient()) {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "fda5aa7eddd21e51cbe4499ab8e967dc9cb868ba");
				var resp = await client.GetAsync($"repos/{repoOwner}/{repoName}/releases/latest");
				var bodyString = await resp.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<GitReleaseInfo>(bodyString);
			}
			//var client = new GitHubClient(new ProductHeaderValue("Github-API-Test"));
			//client.Credentials = new Credentials("TsyrulAlexander", "");

			//return await client.Repository.Release.GetLatest(repoOwner, repoName);
		}

		private static HttpClient GetGithubHttpClient() {
			return new HttpClient {
				BaseAddress = new Uri("https://api.github.com"),
				DefaultRequestHeaders = {{
					"User-Agent", "Github-API-Test"
				}}
			};
		}
	}
}
