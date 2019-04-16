using System.Net;

namespace Terrasoft.Core
{
	public static class NetworkUtilities {
		public static byte[] DownloadFileFromUrl(string url) {
			using (var client = new WebClient()) {
				return client.DownloadData(url);
			}
		}
	}
}
