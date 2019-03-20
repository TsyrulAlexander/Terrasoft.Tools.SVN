using System.Collections.Generic;
using System.Linq;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class CreateFeatureSvnViewModel : BaseSvnOperationViewModel {
		private StringProperty _branchFeatureUrl;
		private StringProperty _branchReleaseUrl;
		private StringProperty _maintainer;
		private StringProperty _featureName;

		public StringProperty BranchFeatureUrl {
			get => _branchFeatureUrl;
			set {
				_branchFeatureUrl = value;
				RaisePropertyChanged();
			}
		}

		public StringProperty BranchReleaseUrl {
			get => _branchReleaseUrl;
			set {
				_branchReleaseUrl = value;
				RaisePropertyChanged();
			}
		}

		public StringProperty Maintainer {
			get => _maintainer;
			set {
				_maintainer = value;
				RaisePropertyChanged();
			}
		}

		public StringProperty FeatureName {
			get => _featureName;
			set {
				_featureName = value;
				RaisePropertyChanged();
			}
		}

		public CreateFeatureSvnViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
			BranchFeatureUrl = new StringProperty(Resources.BranchFeatureUrl, true, "BranchFeatureUrl") {
				Description = Resources.BranchFeatureUrlDescription, Value = AppSetting.DefBranchFeatureUrl
			};
			BranchReleaseUrl = new StringProperty(Resources.BranchReleaseUrl, true, "BranchReleaseUrl") {
				Description = Resources.BranchReleaseUrlDescription, Value = AppSetting.DefBranchReleaseUrl
			};
			Maintainer = new StringProperty(Resources.Maintainer, true, "Maintainer") {
				Description = Resources.MaintainerDescription, Value = AppSetting.DefMaintainer
			};
			FeatureName = new StringProperty(Resources.FeatureName, true, "FeatureName") {
				Description = Resources.FeatureNameDescription, Value = AppSetting.DefFeatureName
			};
		}

		protected override IEnumerable<BaseProperty> GetOperationProperties() {
			return base.GetOperationProperties().Concat(new[] {
				BranchFeatureUrl, BranchReleaseUrl, Maintainer, FeatureName
			});
		}

		public override SvnOperation GetSvnOperation() {
			return SvnOperation.CreateFeature;
		}
	}
}
