using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel {
	public class FinishFeatureSvnViewModel : BaseSvnOperationViewModel {
		public FinishFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
		}

		public override SvnOperation GetSvnOperation() {
			return SvnOperation.FinishFeature;
		}
	}
}