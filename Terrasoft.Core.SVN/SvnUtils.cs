using System;
using System.Collections.Generic;
using SharpSvn;

namespace Terrasoft.Core.SVN
{
    public sealed partial class SvnUtils : SvnUtilsBase
    {
        /// <summary>
        ///     Коллекция с перечнем путей которые нуждаются в разрешении конфликтов
        /// </summary>
        private Dictionary<string, SvnConflictAction> _needResolveList;

        /// <summary>
        ///     Коллекция с перечнем путей которые нуждаются в разрешении конфликтов
        /// </summary>
        private Dictionary<string, SvnConflictAction> NeedResolveList {
            get {
                if (_needResolveList == null) {
                    _needResolveList = new Dictionary<string, SvnConflictAction>();
                }

                return _needResolveList;
            }
        }

        public List<string> ConflictList = new List<string>();

        /// <summary>
        ///     Реинтеграция фитчи в родительскую ветку
        /// </summary>
        public void ReintegrationMergeToBaseBranch() {
            string baseWorkingCopyPath = BaseWorkingCopyPath;
            if (string.IsNullOrEmpty(baseWorkingCopyPath)) {
                baseWorkingCopyPath = WorkingCopyPath + "_Release";
            }

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
            svnReintegrationMergeArgs.Conflict += OnSvnConflict;

            try {
                string workingCopyUrl = string.Empty;
                Info(WorkingCopyPath, new SvnInfoArgs {Revision = new SvnRevision(SvnRevisionType.Head)},
                    (sender, args) => workingCopyUrl = args.Uri.ToString()
                );
                ReintegrationMerge(baseWorkingCopyPath,
                    SvnTarget.FromString(workingCopyUrl),
                    svnReintegrationMergeArgs
                );
            } catch (SvnClientNotReadyToMergeException e) {
                SVN.Logger.Error(e.Message, e.Targets.ToString());
            } finally {
                svnReintegrationMergeArgs.Notify -= SvnReintegrationMergeArgsOnNotify;
                svnReintegrationMergeArgs.Conflict -= OnSvnConflict;
            }

            RemovePackageProperty(baseWorkingCopyPath);
        }

        /// <summary>
        ///     Удаление закрытой фитчи
        /// </summary>
        public void DeleteClosedFeature() {
            string featureRootUrl = string.Empty;
            Info(SvnTarget.FromString(WorkingCopyPath), (sender, args) => featureRootUrl = args.Uri.ToString());
            var svnDeleteArgs = new SvnDeleteArgs {LogMessage = "Remove closed feature branch"};
            RemoteDelete(new Uri(featureRootUrl), svnDeleteArgs);
        }

        private bool TryDoSvnAction(Func<bool> doAction) {
            for (var i = 1; i < 3; i++) {
                try {
                    if (doAction.Invoke()) {
                        return true;
                    }
                } catch (SvnException svnException) {
                    SVN.Logger.Error(svnException.Message, svnException.StackTrace);
                }

                switch (i) {
                    case 1:
                        CleanupFull();
                        RevertRecursively();
                        break;
                    case 2:
                        RevertRecursively();
                        break;
                }
            }

            return false;
        }

        private void RevertRecursively() {
            var svnRevertArgs =
                new SvnRevertArgs {Depth = SvnDepth.Infinity};

            Revert(WorkingCopyPath, svnRevertArgs);
        }

        private void CleanupFull() {
            var svnCleanUpArgs =
                new SvnCleanUpArgs {
                    BreakLocks = true,
                    ClearDavCache = true,
                    FixTimestamps = true,
                    IncludeExternals = true,
                    VacuumPristines = true
                };
            CleanUp(WorkingCopyPath, svnCleanUpArgs);
        }
    }
}