using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Terrasoft.Tools.SvnUI.Ioc;
using Terrasoft.Tools.SvnUI.ViewModel.Configuration;
using Terrasoft.Tools.SvnUI.ViewModel.Deploy;
using Terrasoft.Tools.SvnUI.ViewModel.Svn;

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
		public RestoreMsSqlDatabaseViewModel RestoreMsSqlDatabase => IocKernel.Get<RestoreMsSqlDatabaseViewModel>();
		public RestoreOracleDatabaseViewModel RestoreOracleDatabase => IocKernel.Get<RestoreOracleDatabaseViewModel>();
		public ConfigurationViewModel Configuration => IocKernel.Get<ConfigurationViewModel>();
		public ChangeRepositoryViewModel ChangeRepository => IocKernel.Get<ChangeRepositoryViewModel>();
		public LogViewModel Log => IocKernel.Get<LogViewModel>();
	}
}