using System;
using System.IO;
using SharpSvn;
using Terrasoft.Tools.SVN.Properties;

namespace Terrasoft.Tools.SVN
{
    internal sealed partial class SvnUtils
    {
        /// <summary>
        ///     Копирование базового ветки в ветку фитчи
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
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            } finally {
                svnCopyArgs.Notify -= SvnCopyArgsOnNotify;
            }
        }

        /// <summary>
        ///     Создание нового ветки для фитчи из базовой
        /// </summary>
        /// <returns>Результат</returns>
        public bool CreateFeature() {
            long lastBranchRevision = GetBaseBranchHeadRevision(1, BranchReleaseUrl);
            string featureNewUrl = $"{BranchFeatureUrl}/{Maintainer}_{FeatureName}";
            return CopyBaseBranch(FeatureName, featureNewUrl, lastBranchRevision)
                   && ExtractWorkingCopy(WorkingCopyPath, featureNewUrl)
                   && FixBranch();
        }

        /// <summary>
        ///     Выгрузить рабочую копия в локальную папку
        /// </summary>
        /// <param name="workingCopyPath">Путь к папке</param>
        /// <param name="url"></param>
        /// <returns>Результат</returns>
        private bool ExtractWorkingCopy(string workingCopyPath, string url) {
            return Directory.Exists(workingCopyPath)
                ? UpdateWorkingCopy(workingCopyPath)
                : CheckoutWorkingCopy(workingCopyPath, url);
        }

        /// <summary>
        ///     Выгрузка бранча в рабочую копию
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <param name="url"></param>
        /// <returns>Результат</returns>
        private bool CheckoutWorkingCopy(string workingCopyPath, string url) {
            var svnCheckOutArgs = new SvnCheckOutArgs {IgnoreExternals = false};
            svnCheckOutArgs.Notify += SvnCheckOutArgsOnNotify;
            try {
                return CheckOut(SvnUriTarget.FromString(url), workingCopyPath, svnCheckOutArgs);
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            } finally {
                svnCheckOutArgs.Notify -= SvnCheckOutArgsOnNotify;
            }
        }

        /// <summary>
        ///     Обновить рабочую копию
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Результат</returns>
        private bool UpdateWorkingCopy(string workingCopyPath) {
            Info(SvnTarget.FromString(workingCopyPath), (sender, args) => Console.WriteLine(args));
            var svnUpdateArgs = new SvnUpdateArgs {IgnoreExternals = false, UpdateParents = true};
            svnUpdateArgs.Notify += SvnUpdateArgsOnNotify;
            svnUpdateArgs.Conflict += SvnUpdateArgsOnConflict;
            svnUpdateArgs.SvnError += SvnUpdateArgsOnSvnError;
            try {
                return Update(workingCopyPath, svnUpdateArgs);
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            } finally {
                svnUpdateArgs.Notify -= SvnUpdateArgsOnNotify;
                svnUpdateArgs.Conflict -= SvnUpdateArgsOnConflict;
                svnUpdateArgs.SvnError -= SvnUpdateArgsOnSvnError;
            }
        }
    }
}