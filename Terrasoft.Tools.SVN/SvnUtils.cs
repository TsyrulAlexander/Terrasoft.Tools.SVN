using System;
using SharpSvn;

namespace Terrasoft.Tools.SVN
{
    public partial class SvnUtils : SvnUtilsBase
    {
        /// <summary>
        ///     Реинтеграция фитчи в родительскую ветку
        /// </summary>
        public void ReintegrationMergeToBaseBranch() {
            string baseWorkingCopyPath = WorkingCopyPath + "_Release";
            string baseWorkingCopyUrl =
                GetBaseBranchPath(GetFeatureFirstRevisionNumber(WorkingCopyPath), WorkingCopyPath);
            var svnCheckOutArgs = new SvnCheckOutArgs();
            svnCheckOutArgs.Notify += SvnCheckOutArgsOnNotify;
            try {
                CheckOut(SvnUriTarget.FromString(baseWorkingCopyUrl), baseWorkingCopyPath, svnCheckOutArgs);
            } finally {
                svnCheckOutArgs.Notify -= SvnCheckOutArgsOnNotify;
            }

            string featureRootUrl = string.Empty;
            Info(SvnTarget.FromString(WorkingCopyPath), (sender, args) => { featureRootUrl = args.Uri.ToString(); });

            var svnReintegrationMergeArgs = new SvnReintegrationMergeArgs();
            svnReintegrationMergeArgs.Notify += SvnReintegrationMergeArgsOnNotify;
            svnReintegrationMergeArgs.Conflict += SvnReintegrationMergeArgsOnConflict;

            try {
                ReintegrationMerge(baseWorkingCopyPath, SvnTarget.FromString(featureRootUrl),
                    svnReintegrationMergeArgs);
            } finally {
                svnReintegrationMergeArgs.Notify -= SvnReintegrationMergeArgsOnNotify;
                svnReintegrationMergeArgs.Conflict -= SvnReintegrationMergeArgsOnConflict;
            }

            RemovePackagePropery(baseWorkingCopyPath);
        }

        public void DeleteClosedFeature() {
            string featureRootUrl = string.Empty;
            Info(SvnTarget.FromString(WorkingCopyPath), (sender, args) => { featureRootUrl = args.Uri.ToString(); });
            var svnDeleteArgs = new SvnDeleteArgs {LogMessage = "Remove closed feature branch"};
            RemoteDelete(new Uri(featureRootUrl), svnDeleteArgs);
        }
    }
}