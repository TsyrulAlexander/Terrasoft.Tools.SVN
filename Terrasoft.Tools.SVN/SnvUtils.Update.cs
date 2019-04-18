using System.Diagnostics;
using System.IO;
using SharpSvn;

namespace Terrasoft.Tools.Svn
{
    /// <inheritdoc />
    internal sealed partial class SvnUtils
    {
        /// <summary>
        ///     Интеграция родительской ветки в ветку фитчи
        /// </summary>
        /// <returns>Результат успешности слияния</returns>
        public bool UpdateFromReleaseBranch()
        {
            if (!TryDoSvnAction(() => UpdateWorkingCopy(WorkingCopyPath))) {
                return false;
            }

            long revision = GetFeatureFirstRevisionNumber(WorkingCopyPath);

            string basePath = GetBaseBranchPath(revision, WorkingCopyPath);

            return MergeBaseBranchIntoFeature(WorkingCopyPath, basePath);
        }

        /// <summary>
        ///     Слияние родительской ветки в ветку фитчи
        /// </summary>
        /// <param name="workingCopyPath">Рабочая папка</param>
        /// <param name="basePathUrl">URL родительской ветки</param>
        /// <returns>Результат слияния</returns>
        private bool MergeBaseBranchIntoFeature(string workingCopyPath, string basePathUrl)
        {
            var svnMergeArgs = new SvnMergeArgs {Force = false};
            svnMergeArgs.Notify += OnSvnMergeArgsOnNotify;
            svnMergeArgs.Conflict += OnSvnMergeArgsOnConflict;
            try {
                var mergeRange = new SvnRevisionRange(SvnRevision.One, SvnRevision.Head);
                bool mergeResult = Merge(workingCopyPath, SvnUriTarget.FromString(basePathUrl), mergeRange,
                    svnMergeArgs
                );
                if (mergeResult) {
                    foreach (string resolvePath in NeedResolveList) {
                        var mergeAppStartInfo = new ProcessStartInfo {
                            FileName = "TortoiseProc.exe",
                            Arguments = $"/command:resolve /path:{Path.GetDirectoryName(resolvePath)} /closeonend:3"
                        };
                        using (var mergeApp = new Process()) {
                            mergeApp.StartInfo = mergeAppStartInfo;
                            mergeApp.Start();
                            mergeApp.WaitForExit();
                        }
                    }
                }
            } catch (SvnException svnException) {
                Logger.Error(svnException.Message);
            } finally {
                svnMergeArgs.Notify -= OnSvnMergeArgsOnNotify;
                svnMergeArgs.Conflict -= OnSvnMergeArgsOnConflict;
            }

            return false;
        }
    }
}