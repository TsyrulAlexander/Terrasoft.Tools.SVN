using System.Configuration;

namespace Terrasoft.Core.Version
{
    internal class AppSetting
    {
        public static string RepositoryOwner => GetAppSetting("repositoryOwner");
        public static string RepositoryName => GetAppSetting("repositoryName");
        public static string RepositoryToken => GetAppSetting("repositoryToken");

        private static string GetAppSetting(string key) {
            string exeConfigPath = typeof(AppSetting).Assembly.Location;
            KeyValueConfigurationElement element =
                ConfigurationManager.OpenExeConfiguration(exeConfigPath).AppSettings?.Settings[key];
            if (element != null && !string.IsNullOrWhiteSpace(element.Value)) {
                return element.Value;
            }

            return string.Empty;
        }
    }
}