using System.Collections.Generic;
using System.IO;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Resources = Terrasoft.Tools.SvnUI.Properties.Resources;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public abstract class BaseSvnOperationViewModel : BaseViewModel
    {
        private StringProperty _branchFeatureUrl;
        private StringProperty _svnPassword;
        private StringProperty _svnUser;
        private StringProperty _workingCopyPath;

        public BaseSvnOperationViewModel(IBrowserDialog browserDialog) {
            BrowserDialog = browserDialog;
            SelectWorkingCopyPathCommand = new RelayCommand(SelectWorkingCopyPath);
            Messenger.Default.Register<SvnOperation>(this, OnRunSvnOperation);
            InitPropertyValues();
        }

        protected IBrowserDialog BrowserDialog { get; }

        public StringProperty SvnUser {
            get => _svnUser;
            set {
                _svnUser = value;
                RaisePropertyChanged();
            }
        }

        public StringProperty SvnPassword {
            get => _svnPassword;
            set {
                _svnPassword = value;
                RaisePropertyChanged();
            }
        }

        public StringProperty WorkingCopyPath {
            get => _workingCopyPath;
            set {
                _workingCopyPath = value;
                RaisePropertyChanged();
            }
        }

        public StringProperty BranchFeatureUrl {
            get => _branchFeatureUrl;
            set {
                _branchFeatureUrl = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SelectWorkingCopyPathCommand { get; set; }

        private void InitPropertyValues() {
            SvnUser = new StringProperty(Resources.SvnUser, true, SvnUtilsBase.SvnUserOptionName) {
                Description = Resources.SvnUserDescription, Value = AppSetting.DefSvnUser
            };
            SvnPassword = new StringProperty(Resources.SvnPassword, true, SvnUtilsBase.SvnPasswordOptionName) {
                Description = Resources.SvnPasswordDescription, Value = AppSetting.DefSvnPassword
            };
            WorkingCopyPath =
                new StringProperty(Resources.WorkingCopyPath, true, SvnUtilsBase.WorkingCopyPathOptionName) {
                    Description = Resources.WorkingCopyPathDescription, Value = AppSetting.DefWorkingCopyPath
                };
            BranchFeatureUrl =
                new StringProperty(Resources.BranchFeatureUrl, true, SvnUtilsBase.BranchFeatureUrlOptionName) {
                    Description = Resources.BranchFeatureUrlDescription
                };
            WorkingCopyPath.PropertyChanged += (sender, args) => {
                if (args.PropertyName == nameof(WorkingCopyPath.Value)) {
                    SetBranchFeatureUrlFromWorkingCopyPath();
                }
            };
            SetBranchFeatureUrlFromWorkingCopyPath();
        }

        protected virtual void SetBranchFeatureUrlFromWorkingCopyPath() {
            if (string.IsNullOrWhiteSpace(WorkingCopyPath?.Value) ||
                !Directory.Exists(WorkingCopyPath.Value + @"\.svn")) {
                return;
            }

            try {
                BranchFeatureUrl.Value = SvnUtils.GetRepositoryPathWithFolder(WorkingCopyPath.Value);
            } catch {
                BranchFeatureUrl.Value = string.Empty;
            }
        }

        protected virtual void SelectWorkingCopyPath() {
            string path = BrowserDialog.SelectFilder(WorkingCopyPath.Value);
            if (path != null) {
                WorkingCopyPath.Value = path;
            }
        }

        protected virtual void OnRunSvnOperation(SvnOperation operation) {
            if (operation != GetSvnOperation()) {
                return;
            }

            if (!ValidateProperties(out string message)) {
                BrowserDialog.ShowInformationMessage(message);
                return;
            }

            CreateIfNotExistWorkingCopyDirectory();
            StartSvnOperation();
        }

        protected virtual void CreateIfNotExistWorkingCopyDirectory() {
            Directory.CreateDirectory(WorkingCopyPath.Value);
        }

        protected virtual void StartSvnOperation() {
            Dictionary<string, string> svnArguments = GetPropertiesToArguments();
            Messenger.Default.Send(new StartSvnOperationEventArgs {
                    SvnOperation = GetSvnOperation(), Arguments = svnArguments
                }
            );
        }


        public abstract SvnOperation GetSvnOperation();

        protected override IEnumerable<BaseProperty> GetProperties() {
            return new BaseProperty[] {SvnUser, SvnPassword, WorkingCopyPath, BranchFeatureUrl};
        }
    }
}