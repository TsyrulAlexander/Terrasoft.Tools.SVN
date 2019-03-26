using System.Configuration;

namespace Terrasoft.Tools.SvnUI {
	internal static class AppSetting {
		public static bool CheckNewVersionIsAppStart => bool.Parse(ConfigurationManager.AppSettings.Get("checkNewVersionIsAppStart") ?? "False");
		public static long LatestVersionId => long.Parse(ConfigurationManager.AppSettings.Get("latestVersionId") ?? "0");
		public static string DefSvnUser => ConfigurationManager.AppSettings.Get("defSvnUser");
		public static string DefSvnPassword => ConfigurationManager.AppSettings.Get("defSvnPassword");
		public static string DefWorkingCopyPath => ConfigurationManager.AppSettings.Get("defWorkingCopyPath");
		public static bool DefCommitIfNoError => bool.Parse(ConfigurationManager.AppSettings.Get("defCommitIfNoError") ?? "True");
		public static string DefBranchFeatureUrl => ConfigurationManager.AppSettings.Get("defBranchFeatureUrl");
		public static string DefBranchReleaseUrl => ConfigurationManager.AppSettings.Get("defBranchReleaseUrl");
		public static string DefMaintainer => ConfigurationManager.AppSettings.Get("defMaintainer");
		public static string DefFeatureName => ConfigurationManager.AppSettings.Get("defFeatureName");
	}
}