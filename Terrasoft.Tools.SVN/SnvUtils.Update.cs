using SharpSvn;

namespace Terrasoft.Tools.SVN
{
    /// <inheritdoc />
    public partial class SvnUtils
    {
        /// <summary>
        ///     Интеграция родительской ветки в ветку фитчи
        /// </summary>
        /// <returns>Результат успешности слияния</returns>
        public bool UpdateFromReleaseBranch() {
            if (UpdateWorkingCopy(WorkingCopyPath)) {
                long revision = GetFeatureFirstRevisionNumber(WorkingCopyPath);

                string basePath = GetBaseBranchPath(revision, WorkingCopyPath);

                long headRevision = GetBaseBranchHeadRevision(revision, basePath);

                return MergeBaseBranchIntoFeature(revision, headRevision, WorkingCopyPath, basePath) &&
                       SetPackagePropery(WorkingCopyPath);
            } else {
                throw new SvnObstructedUpdateException("Ошибка обновления из репозитария.");
            }
        }

        /// <summary>
        ///     Слияние родительской ветки в ветку фитчи
        /// </summary>
        /// <param name="startRevision">Номер начальной ревизии</param>
        /// <param name="headRevision">Номер головной ревизии</param>
        /// <param name="workingCopyPath">Рабочая папка</param>
        /// <param name="basePathUrl">URL родительской ветки</param>
        /// <returns>Результат слияния</returns>
        private bool MergeBaseBranchIntoFeature(long startRevision, long headRevision, string workingCopyPath,
            string basePathUrl) {
            var svnMergeArgs = new SvnMergeArgs();
            svnMergeArgs.Notify += OnSvnMergeArgsOnNotify;
            svnMergeArgs.Conflict += OnSvnMergeArgsOnConflict;
            var revs = new SvnRevisionRange(new SvnRevision(startRevision), new SvnRevision(headRevision));
            try {
                return Merge(workingCopyPath, SvnTarget.FromString(basePathUrl), revs, svnMergeArgs);
            } finally {
                svnMergeArgs.Notify -= OnSvnMergeArgsOnNotify;
                svnMergeArgs.Conflict -= OnSvnMergeArgsOnConflict;
            }
        }
    }
}