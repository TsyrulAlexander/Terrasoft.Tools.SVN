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

                return MergeBaseBranchIntoFeature(WorkingCopyPath, basePath, revision);
            }

            throw new SvnObstructedUpdateException("Ошибка обновления из репозитария.");
        }

        /// <summary>
        ///     Слияние родительской ветки в ветку фитчи
        /// </summary>
        /// <param name="workingCopyPath">Рабочая папка</param>
        /// <param name="basePathUrl">URL родительской ветки</param>
        /// <param name="startRevision">Номер стартовой ревизии</param>
        /// <returns>Результат слияния</returns>
        private bool MergeBaseBranchIntoFeature(string workingCopyPath, string basePathUrl, long startRevision) {
            var svnMergeArgs = new SvnMergeArgs();
            svnMergeArgs.Notify += OnSvnMergeArgsOnNotify;
            svnMergeArgs.Conflict += OnSvnMergeArgsOnConflict;
            try {
                return Merge(workingCopyPath, SvnUriTarget.FromString(basePathUrl, true)
                    , new SvnRevisionRange(startRevision, SvnRevision.Head), svnMergeArgs);
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