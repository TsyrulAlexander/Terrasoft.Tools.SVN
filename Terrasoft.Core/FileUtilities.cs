using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Terrasoft.Core {
	public static class FileUtilities {

		public static byte[] DownloadFtpFile(string path, string login = null, string password = null) {
			throw new NotImplementedException();
			var request = (FtpWebRequest)WebRequest.Create(path);
			request.UseBinary = true;
			request.Method = WebRequestMethods.Ftp.DownloadFile;
			if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)) {
				request.Credentials = new NetworkCredential(login, password);
			}
			var response = (FtpWebResponse)request.GetResponse();
			var responseStream = response.GetResponseStream();
			using (MemoryStream memoryStream = new MemoryStream()) {
				memoryStream.CopyTo(responseStream);
				return memoryStream.ToArray();
			}
		}

		public static void SaveFtpFile(string path, string saveTo, string login = null, string password = null) {
			var client = new WebClient();
			if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)) {
				client.Credentials = new NetworkCredential(login, password);
			}
			client.DownloadFile(new Uri(path), saveTo);
		}

		public static void UnZip(byte[] file, string directory) {
			using (var zippedStream = new MemoryStream(file)) {
				using (var archive = new ZipArchive(zippedStream)) {
					foreach (var zipArchiveEntry in archive.Entries) {
						var entryFile = GetArchiveEntryToFile(zipArchiveEntry);
						Directory.CreateDirectory(directory);
						Save(entryFile, directory + @"\" + zipArchiveEntry.Name);
					}
				}
			}
		}

		private static byte[] GetArchiveEntryToFile(ZipArchiveEntry archiveEntry) {
			using (var unzippedEntryStream = archiveEntry.Open()) {
				using (var ms = new MemoryStream()) {
					unzippedEntryStream.CopyTo(ms);
					var unzippedArray = ms.ToArray();
					return unzippedArray;
				}
			}
		}

		public static void Save(byte[] file, string path) {
			File.WriteAllBytes(path, file);
		}
	}
}