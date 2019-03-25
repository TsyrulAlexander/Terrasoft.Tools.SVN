using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Terrasoft.Tools.SvnUI.Ioc;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class ViewModelLocator {
		public ViewModelLocator() {
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
		}

		public MainViewModel Main => IocKernel.Get<MainViewModel>();
		public SvnToolViewModel SvnTool => IocKernel.Get<SvnToolViewModel>();
		public CreateFeatureSvnViewModel CreateFeature => IocKernel.Get<CreateFeatureSvnViewModel>();
		public UpdateFeatureSvnViewModel UpdateFeature => IocKernel.Get<UpdateFeatureSvnViewModel>();
		public FinishFeatureSvnViewModel FinishFeature => IocKernel.Get<FinishFeatureSvnViewModel>();
		public CloseFeatureSvnViewModel CloseFeature => IocKernel.Get<CloseFeatureSvnViewModel>();
		public FixFeatureSvnViewModel FixFeature => IocKernel.Get<FixFeatureSvnViewModel>();
		public DeployAppViewModel DeployApp => IocKernel.Get<DeployAppViewModel>();
		public RestoreDatabaseViewModel RestoreDatabase => IocKernel.Get<RestoreDatabaseViewModel>();
		public LogViewModel Log => IocKernel.Get<LogViewModel>();
	}
}