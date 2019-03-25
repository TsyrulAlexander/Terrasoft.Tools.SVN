using System.Configuration;

namespace Terrasoft.Tools.SvnUI {
	public static class AppSetting {
		public static string LatestVersionId => ConfigurationManager.AppSettings.Get("latestVersionId");
		public static string DefSvnUser => ConfigurationManager.AppSettings.Get("defSvnUser");
		public static string DefSvnPassword => ConfigurationManager.AppSettings.Get("defSvnPassword");
		public static string DefWorkingCopyPath => ConfigurationManager.AppSettings.Get("defWorkingCopyPath");
		public static bool DefCommitIfNoError => bool.Parse(ConfigurationManager.AppSettings.Get("defCommitIfNoError"));
		public static string DefBranchFeatureUrl => ConfigurationManager.AppSettings.Get("defBranchFeatureUrl");
		public static string DefBranchReleaseUrl => ConfigurationManager.AppSettings.Get("defBranchReleaseUrl");
		public static string DefMaintainer => ConfigurationManager.AppSettings.Get("defMaintainer");
		public static string DefFeatureName => ConfigurationManager.AppSettings.Get("defFeatureName");
	}
}