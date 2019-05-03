using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public class RestoreDatabaseViewModel : BaseDeployViewModel
    {
        private EnumProperty<DataBaseType> _dataBaseType;

        public RestoreDatabaseViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
            DataBaseType =
                new EnumProperty<DataBaseType>(Resources.DataBase, true, DeployArgumentNameConstant.DataBaseType);
        }

        public EnumProperty<DataBaseType> DataBaseType {
            get => _dataBaseType;
            set {
                _dataBaseType = value;
                RaisePropertyChanged();
            }
        }

        //private void SelectBackup() { TODO
        //	var backPath = BrowserDialog.SelectFile("Backup file (*.bak)|*.bak");
        //	if (string.IsNullOrWhiteSpace(backPath)) {
        //		return;
        //	}
        //	BackupPath.Value = backPath;
        //}

        protected override DeployOperation GetOperation() {
            return DeployOperation.RestoreDb;
        }
    }
}