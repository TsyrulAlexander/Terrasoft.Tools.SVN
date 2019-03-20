using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel {
	public class UpdateFeatureSvnViewModel : BaseSvnOperationViewModel {
		private BooleanProperty _commitIfNoError;

		public BooleanProperty CommitIfNoError {
			get => _commitIfNoError;
			set {
				_commitIfNoError = value;
				RaisePropertyChanged();
			}
		}

		public UpdateFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
			CommitIfNoError = new BooleanProperty(Resources.CommitIfNoError, false, "CommitIfNoError") {
				Description = Resources.CommitIfNoErrorDescription, Value = AppSetting.DefCommitIfNoError
			};
		}

		protected override IEnumerable<BaseProperty> GetOperationProperties() {
			return base.GetOperationProperties().Concat(new[] {CommitIfNoError});
		}

		public override SvnOperation GetSvnOperation() {
			return SvnOperation.UpdateFeature;
		}
	}
}
