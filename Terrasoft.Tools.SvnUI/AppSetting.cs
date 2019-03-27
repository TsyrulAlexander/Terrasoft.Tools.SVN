using System.Configuration;

namespace Terrasoft.Tools.SvnUI {
	internal static class AppSetting {
		public static bool CheckNewVersionIsAppStart => bool.Parse(GetSettingValue("checkNewVersionIsAppStart") ?? "False");
		public static long LatestVersionId => long.Parse(GetSettingValue("latestVersionId") ?? "0");
		public static string DefSvnUser => GetSettingValue("defSvnUser");
		public static string DefSvnPassword => GetSettingValue("defSvnPassword");
		public static string DefWorkingCopyPath => GetSettingValue("defWorkingCopyPath");
		public static bool DefCommitIfNoError => bool.Parse(GetSettingValue("defCommitIfNoError") ?? "True");
		public static string DefBranchFeatureUrl => GetSettingValue("defBranchFeatureUrl");
		public static string DefBranchReleaseUrl => GetSettingValue("defBranchReleaseUrl");
		public static string DefMaintainer => GetSettingValue("defMaintainer");
		public static string DefFeatureName => GetSettingValue("defFeatureName");
		public static string DefDbServerName => GetSettingValue("defDbServerName");
		public static string DefDbServerUserLogin => GetSettingValue("defDbServerUserLogin");
		public static string DefDbServerUserPassword => GetSettingValue("defDbServerUserPassword");
		public static string DefDatabaseName => GetSettingValue("defDatabaseName");
		public static string DefBackupPath => GetSettingValue("defBackupPath");

		private static string GetSettingValue(string key) {
			return ConfigurationManager.AppSettings.Get(key);
		}
	}
}