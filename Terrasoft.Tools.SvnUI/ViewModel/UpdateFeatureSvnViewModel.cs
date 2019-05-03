using System.Collections.Generic;
using System.Linq;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Resources = Terrasoft.Tools.SvnUI.Properties.Resources;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public class UpdateFeatureSvnViewModel : BaseSvnOperationViewModel
    {
        private BooleanProperty _commitIfNoError;

        public UpdateFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
            CommitIfNoError =
                new BooleanProperty(Resources.CommitIfNoError, false, SvnUtilsBase.CommitIfNoErrorOptionName) {
                    Description = Resources.CommitIfNoErrorDescription, Value = AppSetting.DefCommitIfNoError
                };
        }

        public BooleanProperty CommitIfNoError {
            get => _commitIfNoError;
            set {
                _commitIfNoError = value;
                RaisePropertyChanged();
            }
        }

        protected override IEnumerable<BaseProperty> GetProperties() {
            return base.GetProperties().Concat(new[] {CommitIfNoError});
        }

        public override SvnOperation GetSvnOperation() {
            return SvnOperation.UpdateFeature;
        }
    }
}