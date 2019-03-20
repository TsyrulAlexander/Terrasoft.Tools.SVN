using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class CloseFeatureSvnViewModel : BaseSvnOperationViewModel {
		public CloseFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
		}

		public override SvnOperation GetSvnOperation() {
			return SvnOperation.CloseFeature;
		}
	}
}
