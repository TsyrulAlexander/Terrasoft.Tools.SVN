using Ninject.Modules;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.ViewModel;

namespace Terrasoft.Tools.SvnUI.Ioc
{
	class IocConfiguration: NinjectModule
	{
		public override void Load() {
			Bind<MainViewModel>().ToSelf().InTransientScope();
			Bind<CreateFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<UpdateFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<FinishFeatureSvnViewModel>().ToSelf().InTransientScope();
			Bind<LogViewModel>().ToSelf().InTransientScope();
			Bind<IBrowserDialog>().To<BrowserDialog>();
			Bind<ILogger>().To<UILogger>().InSingletonScope();
		}
	}
}
