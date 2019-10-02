using System.Configuration;

namespace Terrasoft.Configuration {
	internal class AppSetting {
		public static string PackageFolders => GetAppSetting("packageFolders");

		private static string GetAppSetting(string key) {
			var exeConfigPath = typeof(AppSetting).Assembly.Location;
			var element = ConfigurationManager.OpenExeConfiguration(exeConfigPath).AppSettings?.Settings[key];
			if (element != null && !string.IsNullOrWhiteSpace(element.Value)) {
				return element.Value;
			}
			return string.Empty;
		}
	}
}