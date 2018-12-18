using System;
using SharpSvn;

namespace Terrasoft.Tools.SVN
{
    internal sealed partial class SvnUtils : SvnUtilsBase
    {
        /// <summary>
        ///     Реинтеграция фитчи в родительскую ветку
        /// </summary>
        public void ReintegrationMergeToBaseBranch() {
            string baseWorkingCopyPath = BaseWorkingCopyPath ?? WorkingCopyPath + "_Release";
            string baseWorkingCopyUrl =
                GetBaseBranchPath(GetFeatureFirstRevisionNumber(WorkingCopyPath), WorkingCopyPath);
            var svnCheckOutArgs = new SvnCheckOutArgs();
            svnCheckOutArgs.Notify += SvnCheckOutArgsOnNotify;
            try {
                CheckOut(SvnUriTarget.FromString(baseWorkingCopyUrl), baseWorkingCopyPath, svnCheckOutArgs);
            } finally {
                svnCheckOutArgs.Notify -= SvnCheckOutArgsOnNotify;
            }

            Info(SvnTarget.FromString(WorkingCopyPath), (sender, args) => { });

            var svnReintegrationMergeArgs = new SvnReintegrationMergeArgs();
            svnReintegrationMergeArgs.Notify += SvnReintegrationMergeArgsOnNotify;
            svnReintegrationMergeArgs.Conflict += SvnReintegrationMergeArgsOnConflict;

            try {
                string workingCopyUrl = string.Empty;
                Info(WorkingCopyPath, new SvnInfoArgs {Revision = new SvnRevision(SvnRevisionType.Head)}
                    , (sender, args) => workingCopyUrl = args.Uri.ToString());
                ReintegrationMerge(baseWorkingCopyPath
                    , SvnTarget.FromString(workingCopyUrl)
                    , svnReintegrationMergeArgs);
            } catch (SvnClientNotReadyToMergeException e) {
                Logger.LogError(e.Message, e.Targets.ToString());
            } finally {
                svnReintegrationMergeArgs.Notify -= SvnReintegrationMergeArgsOnNotify;
                svnReintegrationMergeArgs.Conflict -= SvnReintegrationMergeArgsOnConflict;
            }

            RemovePackageProperty(baseWorkingCopyPath);
        }

        public void DeleteClosedFeature() {
            string featureRootUrl = string.Empty;
            Info(SvnTarget.FromString(WorkingCopyPath), (sender, args) => featureRootUrl = args.Uri.ToString());
            var svnDeleteArgs = new SvnDeleteArgs {LogMessage = "Remove closed feature branch"};
            RemoteDelete(new Uri(featureRootUrl), svnDeleteArgs);
        }
    }
}