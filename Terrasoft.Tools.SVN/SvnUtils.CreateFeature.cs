namespace Terrasoft.Tools.SVN
{
    using System;
    using System.IO;
    using SharpSvn;
    using Properties;

    public partial class SvnUtils
    {
        /// <summary>
        /// Копирование базового ветки в ветку фитчи
        /// </summary>
        /// <param name="featureName">Название фитчи</param>
        /// <param name="featureNewUrl">URL новой ветки фитчи</param>
        /// <param name="revision">Номер ревизии из которой выделяется копия</param>
        /// <returns>Результат</returns>
        private bool CopyBaseBranch(string featureName, string featureNewUrl, long revision) {
            var svnCopyArgs = new SvnCopyArgs {
                LogMessage = string.Format(Resources.SvnUtils_CopyBaseBranch_Init_Feature, featureName),
                Revision = new SvnRevision(revision)
            };
            svnCopyArgs.Notify += SvnCopyArgsOnNotify;
            try {
                return RemoteCopy(SvnTarget.FromString(BranchReleaseUrl), new Uri(featureNewUrl), svnCopyArgs);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                svnCopyArgs.Notify -= SvnCopyArgsOnNotify;
            }
        }

        /// <summary>
        /// Создание нового ветки для фитчи из базовой
        /// </summary>
        /// <returns>Результат</returns>
        public bool CreateFeature() {
            string workingCopyPath = WorkingCopyPath;
            if (!ExtractWorkingCopy(workingCopyPath)) {
                return false;
            }

            long lastBranchRevision = GetBaseBranchHeadRevision(1, BranchReleaseUrl);
            string featureNewUrl = $"{BranchFeatureUrl}/{Maintainer}_{FeatureName}";
            return CopyBaseBranch(FeatureName, featureNewUrl, lastBranchRevision) &&
                   Switch(workingCopyPath, SvnUriTarget.FromString(featureNewUrl));
        }

        /// <summary>
        /// Выгрузить рабочую копия в локальную папку
        /// </summary>
        /// <param name="workingCopyPath">Путь к папке</param>
        /// <returns>Результат</returns>
        private bool ExtractWorkingCopy(string workingCopyPath) {
            return Directory.Exists(workingCopyPath)
                ? UpdateWorkingCopy(workingCopyPath)
                : CheckouWorkingCopy(workingCopyPath);
        }

        /// <summary>
        /// Выгрузка бранча в рабочую копию
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Результат</returns>
        private bool CheckouWorkingCopy(string workingCopyPath) {
            var svnCheckOutArgs = new SvnCheckOutArgs() {IgnoreExternals = false};
            svnCheckOutArgs.Notify += SvnCheckOutArgsOnNotify;
            try {
                string branchRelease = BranchReleaseUrl;
                return CheckOut(SvnUriTarget.FromString(branchRelease), workingCopyPath, svnCheckOutArgs);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                svnCheckOutArgs.Notify -= SvnCheckOutArgsOnNotify;
            }
        }

        /// <summary>
        /// Обновить рабочую копию
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Результат</returns>
        private bool UpdateWorkingCopy(string workingCopyPath) {
            Info(SvnTarget.FromString(workingCopyPath), (sender, args) => { Console.WriteLine(args); });
            var svnUpdateArgs = new SvnUpdateArgs {IgnoreExternals = false};
            svnUpdateArgs.Notify += SvnUpdateArgsOnNotify;
            try {
                return Update(workingCopyPath, svnUpdateArgs);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                svnUpdateArgs.Notify -= SvnUpdateArgsOnNotify;
            }
        }
    }
}