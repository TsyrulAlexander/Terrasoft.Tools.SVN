using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Terrasoft.Core.DeployApp.Database;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public class DeployAppViewModel: ViewModelBase
    {
		private bool _inProgress;
		public bool InProgress {
			get => _inProgress;
			set {
				_inProgress = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand ExecuteCommand {
			get; set;
		}

		public DeployAppViewModel() {
			ExecuteCommand = new RelayCommand(Execute);
		}

		private void Execute() {
			//IDbExecutor executor = new MsSqlExecutor("MTS7131DevATsyrul", @"Partner-MS-02\MSSQL2017", "MTS7131DevATsyrul", "MTS7131DevATsyrul");
			//executor.RestoreDb(@"C:\dfghjkl\BPMonline7123SalesEnterprise_Marketing_ServiceEnterprise.bak", Callback);
		}

		private void Callback() {
			
		}
	}
}
