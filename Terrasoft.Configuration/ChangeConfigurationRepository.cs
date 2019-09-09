using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SharpSvn;
using Terrasoft.Core;
using Terrasoft.Core.SVN;

namespace Terrasoft.Configuration {
	public class ChangeConfigurationRepository {
		protected ILogger Logger { get; }
		protected ChangeConfigurationRepositoryParameter Parameter { get; }
		private const string PackageContentUrlPath = "branches/7.8.0";
		public ChangeConfigurationRepository(ILogger logger, ChangeConfigurationRepositoryParameter parameter) {
			Logger = logger;
			Parameter = parameter;
		}
		public Task RunAsync() {
			return Task.Run((Action)Run);
		}
		public void Run() {
			var svnParams = new Dictionary<string, string> {
				{SvnUtilsBase.SvnUserOptionName, Parameter.SvnLogin},
				{SvnUtilsBase.SvnPasswordOptionName, Parameter.SvnPassword}
			};
			using (var svnClient = new SvnUtils(svnParams, Logger)) {
				if (svnClient.GetList(SvnTarget.FromString(Parameter.UrlPath), out var repositoryPackages)) {
					ChangePackages(svnClient, repositoryPackages.Where(args => !string.IsNullOrWhiteSpace(args.Path)));
				}
			}
		}

		protected virtual void ChangePackages(SvnClient svnClient, IEnumerable<SvnListEventArgs> repositoryPackages) {
			foreach (var repositoryPackage in repositoryPackages.Where(args => !string.IsNullOrWhiteSpace(args.Path))) {
				var packagePath = Path.Combine(Parameter.PkgPath, repositoryPackage.Name);
				var packageUrl = repositoryPackage.Uri.ToString();
				if (Directory.Exists(packagePath) &&
					!string.IsNullOrWhiteSpace(SvnUtils.GetRepositoryPathWithFolder(packagePath))) {
					SwitchPackage(svnClient, packageUrl, packagePath);
				} else {
					CheckOutPackage(svnClient, packageUrl, packagePath);
				}
			}
		}
		protected virtual void SwitchPackage(SvnClient svnClient, string packageUrl, string packagePath) {
			try {
				var packageContentUrl = GetPackageContentUri(packageUrl);
				svnClient.Switch(packagePath, packageContentUrl);
				Logger.LogInfo($"Switch: {packagePath}");
			} catch (Exception e) {
				Logger.LogError(e.Message);
			}
		}
		protected virtual void CheckOutPackage(SvnClient svnClient, string packageUrl, string packagePath) {
			try {
				var packageContentUrl = GetPackageContentUri(packageUrl);
				svnClient.CheckOut(packageContentUrl, packagePath, new SvnCheckOutArgs {
					AllowObstructions = true
				});
				Logger.LogInfo($"CheckOut: {packagePath}");
			} catch (Exception e) {
				Logger.LogError(e.Message);
			}
		}

		protected virtual Uri GetPackageContentUri(string packageUrl) {
			return new Uri($"{packageUrl.TrimEnd('/')}/{PackageContentUrlPath}");
		}
	}
}