using System.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Terrasoft.Tools.SvnUI.Model;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class MainViewModel : ViewModelBase {
		private OperationType _operationType;
		public OperationType OperationType {
			get => _operationType;
			set {
				_operationType = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand<OperationType> SetOperationTypeCommand { get; set; }

		public MainViewModel() {
			SetOperationTypeCommand = new RelayCommand<OperationType>(SetOperationType);
		}

		private void SetOperationType(OperationType operationType) {
			OperationType = operationType;
		}
	}
}