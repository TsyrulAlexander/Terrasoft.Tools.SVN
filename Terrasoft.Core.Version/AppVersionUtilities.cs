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
				UpdateApp(info, appName);
			}
			return info.Id;
		}

		public static void UpdateApp(GitReleaseInfo releaseInfo, string appName) {
			//var fileInfo = releaseInfo.Assets.FirstOrDefault(info => info.Name.StartsWith(appName));
			//if (fileInfo == null) {
			//	throw new NullReferenceException(nameof(appName));
			//}
			//var file = NetworkUtilities.DownloadFileFromUrl(fileInfo.DownloadUrl);
			//FileUtilities.UnZip(file, AppSetting.TempFolder);
			StartPs();
		}

		internal static void StartPs() {
			var args = GetCmdArguments();
			Process.Start("powershell.exe", args);
		}

		internal static string GetCmdArguments() {
			var currentDirectory = Directory.GetCurrentDirectory();
			var currentProcess = Process.GetCurrentProcess();
			var psPath = AppSetting.TempFolder + @"\UpdateApp.ps1";
			return $"& -NoExit '{psPath}' -ProcessName {currentProcess.ProcessName} -AppFolder {currentDirectory} -NewAppFolder {AppSetting.TempFolder}";
		}

		public static async Task UpdateApp(string appName) {
			var info = await GetLatestReleaseInfoAsync();
			UpdateApp(info, appName);
		}

		private static async Task<GitReleaseInfo> GetLatestReleaseInfoAsync() {
			return await GitRepository.GetLatestReleaseInfoAsync(AppSetting.RepositoryOwner, AppSetting.RepositoryName,
				AppSetting.RepositoryToken);
		}
	}
}
