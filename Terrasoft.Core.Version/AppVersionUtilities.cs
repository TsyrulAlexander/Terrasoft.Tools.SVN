using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core.Git;
using Terrasoft.Core.Version.Properties;

namespace Terrasoft.Core.Version {
	public static class AppVersionUtilities {
		public static async Task<long> GetVersionIdAsync(bool isUpdateApp = false) {
			var info = await GetLatestReleaseInfoAsync();
			if (isUpdateApp) {
				UpdateApp(info);
			}
			return info.Id;
		}

		public static string DownloadNewVersionApp(GitReleaseInfo releaseInfo) {
			var fileInfo = releaseInfo.Assets.FirstOrDefault();
			if (fileInfo == null) {
				throw new NullReferenceException(nameof(releaseInfo.Assets));
			}
			var file = NetworkUtilities.DownloadFileFromUrl(fileInfo.DownloadUrl);
			var tempFolderName = $"{Path.GetTempPath()}\\{Path.GetRandomFileName()}";
			FileUtilities.UnZip(file, tempFolderName);
			return tempFolderName;
		}

		public static void ReplaceAppFiles(string tempFolderName, string tempScriptPath) {
			var psi = new ProcessStartInfo {
				FileName = "powershell.exe",
				Arguments = GetPsArguments(tempFolderName, tempScriptPath)
			};
			Process.Start(psi);
		}

		internal static string GetPsArguments(string tempFolderPath, string tempScriptPath) {
			var currentDirectory = Directory.GetCurrentDirectory();
			var currentProcess = Process.GetCurrentProcess();
			return
				$"-ExecutionPolicy bypass -File {tempScriptPath} -ProcessName \"{currentProcess.ProcessName}\" -AppFolder \"{currentDirectory}\" -TempAppFolder \"{tempFolderPath}\"";
		}

		public static async Task UpdateApp() {
			var releaseInfo = await GetLatestReleaseInfoAsync();
			UpdateApp(releaseInfo);
		}

		public static void UpdateApp(GitReleaseInfo releaseInfo) {
			var tempFolderName = DownloadNewVersionApp(releaseInfo);
			var tempFileName = Path.GetTempPath()+"\\updateApp.ps1";
			var scriptContent = Encoding.UTF8.GetString(Resources.UpdateApp);
			File.WriteAllText(tempFileName, scriptContent);
			ReplaceAppFiles(tempFolderName, tempFileName);
		}

		//private string GetScript

		private static async Task<GitReleaseInfo> GetLatestReleaseInfoAsync() {
			return await GitRepository.GetLatestReleaseInfoAsync(AppSetting.RepositoryOwner, AppSetting.RepositoryName,
				AppSetting.RepositoryToken);
		}
	}
}