using System;
using System.Net;

namespace Terrasoft.Core
{
    public static class NetworkUtilities
    {
        public static byte[] DownloadFileFromUrl(string url)
        {
            using (var client = new WebClient()) {
                return client.DownloadData(url);
            }
        }

        public static void SaveFtpFile(string path, string saveTo, string login = null, string password = null)
        {
            var client = new WebClient();
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)) {
                client.Credentials = new NetworkCredential(login, password);
            }

            client.DownloadFile(new Uri(path), saveTo);
        }
    }
}