using System;
using System.Configuration;
using Terrasoft.Tools.SvnUI.Model.Enums;

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
		public static FilePathType DefBackupPathType => GetEnumValue<FilePathType>("defBackupPathType");
		public static string DefBackupFtpPath => GetSettingValue("defBackupFtpPath");
		public static string DefFtpLogin => GetSettingValue("defFtpLogin");
		public static string DefFtpPassword => GetSettingValue("defFtpPassword");
		public static string DefFtpTempFile => GetSettingValue("defFtpTempFile");

		private static T GetEnumValue<T>(string key) where T : struct {
			var value = GetSettingValue(key);
			if (Enum.TryParse(value, out T enumValue)) {
				return enumValue;
			}
			return default(T);
		}
		private static string GetSettingValue(string key) {
			return ConfigurationManager.AppSettings.Get(key);
		}
	}
}