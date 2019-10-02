using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Terrasoft.Core;

namespace Terrasoft.Configuration {
	public class PackageCompressionHelper {
		private readonly ILogger _logger;
		private const string PackageDescriptorFileName = "descriptor.json";
		private const string BranchesDirectoryName = "branches";
		private const string ArchiveFormat = ".gz";
		private const string MainArchiveFormat = ".zip";
		public PackageCompressionHelper(ILogger logger) {
			_logger = logger;
		}
		public Task CompressionProjectsAsync(string sourcePath, string destinationPath, string archiveName = null,
			IEnumerable<string> packages = null) {
			return Task.Run(() => { CompressionProjects(sourcePath, destinationPath, archiveName, packages); });
		}

		public void CompressionProjects(string sourcePath, string destinationPath, string archiveName = null,
			IEnumerable<string> packages = null) {
			try {
				var startDate = DateTime.Now;
				_logger.LogInfo("Start compression");
				if (packages == null) {
					packages = GetPackageNames(sourcePath);
				}
				if (archiveName == null) {
					archiveName = "Packages" + DateTime.Now.ToString("MMddyyyyHHmmss");
				}
				var tempDirectory = CreateTempPath();
				foreach (var packageName in packages) {
					var packagePath = Path.Combine(sourcePath, packageName);
					CompressionProject(packagePath, tempDirectory, packageName);
				}
				Directory.CreateDirectory(destinationPath);
				var archivePath = Path.Combine(destinationPath, archiveName);
				ZipFile.CreateFromDirectory(tempDirectory, archivePath + MainArchiveFormat);
				Directory.Delete(tempDirectory, true);
				_logger.LogInfo($"End compression {DateTime.Now - startDate}");
			} catch (Exception ex) {
				_logger.LogError(ex.Message);
			}
		}

		public void CompressionProject(string sourcePath, string destinationDirectory, string packageName) {
			try {
				var startDate = DateTime.Now;
				var packageContentPath = GetPackageContentPath(sourcePath);
				var tempPackagePath = CreateTempPath();
				CopyProjectFiles(packageContentPath, tempPackagePath);
				var files = Directory.GetFiles(tempPackagePath, "*.*", SearchOption.AllDirectories);
				var archivePath = Path.Combine(destinationDirectory, packageName);
				using (Stream stream = File.Open(archivePath + ArchiveFormat, FileMode.Create, FileAccess.Write,
					FileShare.None)) {
					using (var zipStream = new GZipStream(stream, CompressionMode.Compress)) {
						foreach (var filePath in files)
							CompressionUtilities.ZipFile(filePath, tempPackagePath.Length, zipStream);
					}
				}
				Directory.Delete(tempPackagePath, true);
				_logger.LogInfo($"{packageName} is compression ({(DateTime.Now - startDate)})");
			} catch (Exception ex) {
				_logger.LogError(ex.Message);
			}
		}

		private static IEnumerable<string> GetPackageNames(string sourcePath) {
			var directories = Directory.GetDirectories(sourcePath);
			return directories.Where(directory => GetPackageContentPath(directory) != null)
			.Select(directory => new DirectoryInfo(directory).Name);
		}

		private static string GetPackageContentPath(string packagePath) {
			if (File.Exists(Path.Combine(packagePath, PackageDescriptorFileName))) {
				return packagePath;
			}
			var branchesPath = Path.Combine(packagePath, BranchesDirectoryName);
			if (!Directory.Exists(branchesPath)) {
				return null;
			}
			var versionPath = Directory.GetDirectories(branchesPath).FirstOrDefault();
			if (versionPath != null && File.Exists(Path.Combine(versionPath, PackageDescriptorFileName))) {
				return versionPath;
			}
			return null;
		}


		private static string CreateTempPath(string directoryName = null) {
			var path = Path.Combine(Path.GetTempPath(), directoryName ?? Path.GetRandomFileName());
			if (Directory.Exists(path)) {
				Directory.Delete(path, true);
			}
			Directory.CreateDirectory(path);
			return path;
		}

		private static void CopyProjectFiles(string sourcePath, string destinationPath) {
			var packageContentDirectories = GetPackageContentDirectories();
			foreach (var packageContentDirectory in packageContentDirectories) {
				CopyProjectElement(sourcePath, destinationPath, packageContentDirectory);
			}
			File.Copy(Path.Combine(sourcePath, "descriptor.json"), Path.Combine(destinationPath, "descriptor.json"));
		}

		private static IEnumerable<string> GetPackageContentDirectories() {
			return AppSetting.PackageFolders.Split(new []{ ',' }, StringSplitOptions.RemoveEmptyEntries).Select(folderName => folderName.Trim());
		}

		private static void CopyProjectElement(string sourcePath, string destinationPath, string name) {
			var str = Path.Combine(sourcePath, name);
			if (!Directory.Exists(str))
				return;
			string dest = Path.Combine(destinationPath, name);
			CopyDirectory(str, dest);
		}

		private static void CopyDirectory(string source, string dest) {
			if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(dest))
				return;
			Directory.CreateDirectory(dest);
			foreach (string file in Directory.GetFiles(source)) {
				File.Copy(file, Path.Combine(dest, Path.GetFileName(file)), true);
			}
			foreach (var directory in Directory.GetDirectories(source)) {
				CopyDirectory(directory, Path.Combine(dest, Path.GetFileName(directory)));
			}
		}
	}
}