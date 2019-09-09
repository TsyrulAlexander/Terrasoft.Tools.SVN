using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel.Svn {
	public class FinishFeatureSvnViewModel : BaseSvnOperationViewModel {
		public FinishFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
		}

		public override SvnOperation GetSvnOperation() {
			return SvnOperation.FinishFeature;
		}
	}
}