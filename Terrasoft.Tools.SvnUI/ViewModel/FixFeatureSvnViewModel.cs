using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class FixFeatureSvnViewModel : BaseSvnOperationViewModel {
		public FixFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
		}

		public override SvnOperation GetSvnOperation() {
			return SvnOperation.FixFeature;
		}
	}
}
