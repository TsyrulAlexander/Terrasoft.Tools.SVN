using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public class CloseFeatureSvnViewModel : BaseSvnOperationViewModel
    {
        public CloseFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) { }

        protected override void StartSvnOperation() {
            if (BrowserDialog.ShowModalYesNo(Resources.IfCloseFeatureMessage)) {
                base.StartSvnOperation();
            }
        }

        public override SvnOperation GetSvnOperation() {
            return SvnOperation.CloseFeature;
        }
    }
}