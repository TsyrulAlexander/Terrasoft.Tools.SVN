using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terrasoft.Core.Git;

namespace Terrasoft.Core.Version
{
	public static class AppVersionUtilities {
		public static async Task<long> GetVersionIdAsync(string appName = null) {
			var info = await GetLatestReleaseInfoAsync();
			if (!string.IsNullOrWhiteSpace(appName)) {
				UpdateApp(appName, info);
			}
			return info.Id;
		}

		public static void DownloadNewVersionApp(GitReleaseInfo releaseInfo, string appName) {
			var fileInfo = releaseInfo.Assets.FirstOrDefault(info => info.Name.StartsWith(appName));
			if (fileInfo == null) {
				throw new NullReferenceException(nameof(appName));
			}
			var file = NetworkUtilities.DownloadFileFromUrl(fileInfo.DownloadUrl);
			FileUtilities.UnZip(file, AppSetting.TempFolder);
		}

		public static void ReplaceAppFiles() {
			var args = GetPsArguments();
			Process.Start("powershell.exe", args);
		}

		internal static string GetPsArguments() {
			var currentDirectory = Directory.GetCurrentDirectory();
			var currentProcess = Process.GetCurrentProcess();
			var psPath = AppSetting.TempFolder + @"\" + AppSetting.ReplaceAppFilesPsFileName;
			return $"& '{psPath}' -ProcessName {currentProcess.ProcessName} -AppFolder {currentDirectory} -NewAppFolder {AppSetting.TempFolder}";
		}

		public static async Task UpdateApp(string appName) {
			var	releaseInfo = await GetLatestReleaseInfoAsync();
			UpdateApp(appName, releaseInfo);
		}


		public static void UpdateApp(string appName, GitReleaseInfo releaseInfo) {
			DownloadNewVersionApp(releaseInfo, appName);
			ReplaceAppFiles();
		}

		private static async Task<GitReleaseInfo> GetLatestReleaseInfoAsync() {
			return await GitRepository.GetLatestReleaseInfoAsync(AppSetting.RepositoryOwner, AppSetting.RepositoryName,
				AppSetting.RepositoryToken);
		}
	}
}
