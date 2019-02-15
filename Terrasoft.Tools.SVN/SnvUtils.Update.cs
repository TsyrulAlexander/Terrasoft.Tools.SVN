using SharpSvn;

namespace Terrasoft.Tools.SVN
{
    /// <inheritdoc />
    internal sealed partial class SvnUtils
    {
        /// <summary>
        ///     Интеграция родительской ветки в ветку фитчи
        /// </summary>
        /// <returns>Результат успешности слияния</returns>
        public bool UpdateFromReleaseBranch() {
            if (!UpdateWorkingCopy(WorkingCopyPath)) {
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
        private bool MergeBaseBranchIntoFeature(string workingCopyPath, string basePathUrl) {
            var svnMergeArgs = new SvnMergeArgs {
                Force = true
            };
            svnMergeArgs.Notify += OnSvnMergeArgsOnNotify;
            svnMergeArgs.Conflict += OnSvnMergeArgsOnConflict;
            try {
                var mergeRange = new SvnRevisionRange(SvnRevision.One, SvnRevision.Head);
                return Merge(workingCopyPath, SvnUriTarget.FromString(basePathUrl), mergeRange, svnMergeArgs);
            } catch (SvnException svnException) {
                Logger.LogError(svnException.Message);
            } finally {
                svnMergeArgs.Notify -= OnSvnMergeArgsOnNotify;
                svnMergeArgs.Conflict -= OnSvnMergeArgsOnConflict;
            }

            return false;
        }
    }
}