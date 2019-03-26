using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.Core.Version
{
	internal class AppSetting {
		public static string RepositoryOwner => GetAppSetting("repositoryOwner");
		public static string RepositoryName => GetAppSetting("repositoryName");
		public static string RepositoryToken => GetAppSetting("repositoryToken");

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
