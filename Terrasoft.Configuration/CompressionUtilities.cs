using System;
using System.IO;
using System.IO.Compression;

namespace Terrasoft.Configuration {
	public static class CompressionUtilities {
		private static void WriteFileName(string relativeFilePath, GZipStream zipStream) {
			var charArray = relativeFilePath.ToCharArray();
			zipStream.Write(BitConverter.GetBytes(charArray.Length), 0, 4);
			foreach (var ch in charArray) {
				zipStream.Write(BitConverter.GetBytes(ch), 0, 2);
			}
		}

		private static void WriteFileContent(string filePath, GZipStream zipStream) {
			var buffer = File.ReadAllBytes(filePath);
			zipStream.Write(BitConverter.GetBytes(buffer.Length), 0, 4);
			zipStream.Write(buffer, 0, buffer.Length);
		}

		public static void ZipFile(string filePath, int rootDirectoryPathLength, GZipStream zipStream) {
			WriteFileName(filePath.Substring(rootDirectoryPathLength).TrimStart(Path.DirectorySeparatorChar),
				zipStream);
			WriteFileContent(filePath, zipStream);
		}
	}
}
