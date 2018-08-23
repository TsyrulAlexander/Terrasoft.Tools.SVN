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
            if (UpdateWorkingCopy(WorkingCopyPath)) {
                long revision = GetFeatureFirstRevisionNumber(WorkingCopyPath);

                string basePath = GetBaseBranchPath(revision, WorkingCopyPath);

                return MergeBaseBranchIntoFeature(WorkingCopyPath, basePath) && SetPackageProperty(WorkingCopyPath);
            } else {
                throw new SvnObstructedUpdateException("Ошибка обновления из репозитария.");
            }
        }

        /// <summary>
        ///     Слияние родительской ветки в ветку фитчи
        /// </summary>
        /// <param name="workingCopyPath">Рабочая папка</param>
        /// <param name="basePathUrl">URL родительской ветки</param>
        /// <returns>Результат слияния</returns>
        private bool MergeBaseBranchIntoFeature(string workingCopyPath, string basePathUrl) {
            var svnMergeArgs = new SvnReintegrationMergeArgs();
            svnMergeArgs.Notify   += OnSvnMergeArgsOnNotify;
            svnMergeArgs.Conflict += OnSvnMergeArgsOnConflict;
            try {
                return ReintegrationMerge(workingCopyPath, SvnUriTarget.FromString(basePathUrl), svnMergeArgs);
            } finally {
                svnMergeArgs.Notify   -= OnSvnMergeArgsOnNotify;
                svnMergeArgs.Conflict -= OnSvnMergeArgsOnConflict;
            }
        }
    }
}