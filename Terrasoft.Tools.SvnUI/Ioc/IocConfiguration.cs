using Ninject.Modules;
using Terrasoft.Core;
using Terrasoft.Core.DeployApp.Database;
using Terrasoft.Core.DeployApp.Database.MsSql;
using Terrasoft.Core.DeployApp.Database.Oracle;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Log;
using Terrasoft.Tools.SvnUI.ViewModel;
using Terrasoft.Tools.SvnUI.ViewModel.Deploy;
using Terrasoft.Tools.SvnUI.ViewModel.Svn;

namespace Terrasoft.Tools.SvnUI.Ioc {
	class IocConfiguration : NinjectModule {
		public override void Load() {
			Bind<MainViewModel>().ToSelf().InTransientScope();
			Bind<SvnToolViewModel>().ToSelf().InTransientScope();
			Bind<CreateFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<UpdateFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<FinishFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<CloseFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<FixFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<DeployAppViewModel>().ToSelf().InTransientScope();
			Bind<RestoreDatabaseViewModel>().ToSelf().InTransientScope();
			Bind<RestoreOracleDatabaseViewModel>().ToSelf().InTransientScope();
			Bind<RestoreMsSqlDatabaseViewModel>().ToSelf().InTransientScope();
			Bind<LogViewModel>().ToSelf().InTransientScope();
			Bind<IBrowserDialog>().To<BrowserDialog>();
			Bind<ILogger>().To<UILogger>().InSingletonScope();
			Bind<IDbExecutor>().To<MsSqlExecutor>().Named("MSSql");
			Bind<IDbExecutor>().To<OracleExecutor>().Named("Oracle");
		}
	}
}