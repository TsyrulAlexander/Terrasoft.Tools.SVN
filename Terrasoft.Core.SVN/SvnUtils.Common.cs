using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpSvn;
using Terrasoft.Core.SVN.Properties;

namespace Terrasoft.Core.SVN
{
    /// <inheritdoc />
    /// <summary>
    ///     Класс реализации SVN клиента
    /// </summary>
    public sealed partial class SvnUtils
    {
        /// <inheritdoc />
        /// <summary>
        ///     Конструктор SVN клиента
        /// </summary>
        /// <param name="programOptions">Словарь с параметрами</param>
        public SvnUtils(IReadOnlyDictionary<string, string> programOptions, ILogger logger) : base(programOptions, logger) { }

		public static string GetRepositoryPathWithFolder(string folderPath) {
			try {
				using (var svnClient = new SvnClient()) {
					svnClient.GetInfo(SvnTarget.FromString(folderPath), out var args);
					return args?.Uri?.AbsoluteUri;
				}
			} catch (SvnInvalidNodeKindException nodeKindException) {
				if (nodeKindException.SvnErrorCode == SvnErrorCode.SVN_ERR_WC_NOT_DIRECTORY) {
					return string.Empty;
				}
				throw;
			}
		}

        /// <summary>
        ///     Получить URL ветки из которой была выделена фитча
        /// </summary>
        /// <param name="revision">Номер ревизии в которой выделена ветка</param>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Строка с URL</returns>
        private string GetBaseBranchPath(long revision, string workingCopyPath) {
            string basePath = string.Empty;
            var svnInfoArgs = new SvnInfoArgs {Revision = new SvnRevision(revision)};
            Info(SvnTarget.FromString(workingCopyPath), svnInfoArgs, (sender, args) => {
                basePath = args.Uri.ToString();
               Logger.LogInfo(args.Uri.ToString());
            });
            return basePath;
        }

        /// <summary>
        ///     Проверка рабочей копии на ошибки
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Результат проверки</returns>
        private bool CheckWorkingCopyForError(string workingCopyPath) {
            var result = true;
            Status(workingCopyPath, (sender, args) => {
                if (result && args.Conflicted) {
                    result = !args.Conflicted;
                }
            });
            return result;
        }

        /// <summary>
        ///     Получить номер ревизии выделения ветки
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Номер ревизии</returns>
        private long GetFeatureFirstRevisionNumber(string workingCopyPath) {
            long revision = 0;
            var svnLogArgs = new SvnLogArgs {StrictNodeHistory = true};
            svnLogArgs.Notify += SvnLogArgsOnNotify;
            Log(workingCopyPath, svnLogArgs, (sender, args) => {
                if (args.ChangedPaths.Count <= 0) {
                    return;
                }

                foreach (SvnChangeItem changeItem in args.ChangedPaths) {
                    if (string.IsNullOrEmpty(changeItem.CopyFromPath)) {
                        continue;
                    }

                    revision = changeItem.CopyFromRevision;
                }
            });
            return revision;
        }

        /// <summary>
        ///     Получить последний номер ревизии родительской ветки
        /// </summary>
        /// <param name="basePath">URL родительской ветки</param>
        /// <returns>Номер ревизии</returns>
        private long GetBaseBranchHeadRevision(string basePath) {
            long headRevision = 0;
            var svnLogArgs = new SvnLogArgs {
                Limit = 1,
                RetrieveAllProperties = false,
                RetrieveChangedPaths = false,
                RetrieveMergedRevisions = false,
                StrictNodeHistory = true
            };

            Log(new Uri(basePath), svnLogArgs, (sender, args) => headRevision = args.Revision);
            return headRevision;
        }

        /// <summary>
        ///     Зафиксировать изменения в хранилище
        /// </summary>
        /// <param name="checkError">Проверить рабочую копию на ошибки перед фиксацией</param>
        /// <param name="logMessage"></param>
        /// <exception cref="SvnRepositoryException">Исключение в случае не разрешённых конфликтов рабочей копии</exception>
        /// <returns>Результат фиксации изменений в хранилище</returns>
        public bool CommitChanges(bool checkError = false, string logMessage = "") {
            if (checkError && !CheckWorkingCopyForError(WorkingCopyPath)) {
                Logger.LogError(Resources.SvnUtils_CommitChanges_Sources_not_resolved);
                return false;
            }

            var svnCommitArgs = new SvnCommitArgs {
                LogMessage = string.IsNullOrEmpty(logMessage)
                    ? Resources
                        .SvnUtils_CommitChanges_Reintegrate_base_branch_to_feature
                    : logMessage
            };

            svnCommitArgs.Committing += SvnCommitArgsOnCommitting;
            svnCommitArgs.Notify += SvnCommitArgsOnNotify;
            svnCommitArgs.Committed += SvnCommitArgsOnCommitted;
            try {
                return Commit(WorkingCopyPath, svnCommitArgs);
            } finally {
                svnCommitArgs.Committing -= SvnCommitArgsOnCommitting;
                svnCommitArgs.Notify += SvnCommitArgsOnNotify;
            }
        }

        private bool SetPackageProperty(string workingCopyPath = "") {
            string localWorkingCopyPath = string.IsNullOrEmpty(workingCopyPath)
                ? WorkingCopyPath
                : workingCopyPath;
            IEnumerable<string> branchPackages =
                Directory.EnumerateDirectories(localWorkingCopyPath, "Schemas", SearchOption.AllDirectories);
            foreach (string packageRootDir in from packagePath in branchPackages
                where !string.IsNullOrEmpty(packagePath)
                let slashPosition = packagePath.LastIndexOf('\\')
                select packagePath.Substring(0, slashPosition)) {
                SetProperty(packageRootDir, "PackageUpdateDate", DateTime.UtcNow.ToLongDateString());
            }

            return true;
        }

        private bool RemovePackageProperty(string workingCopyPath = "") {
            string localWorkingCopyPath = string.IsNullOrEmpty(workingCopyPath)
                ? WorkingCopyPath
                : workingCopyPath;
            IEnumerable<string> branchPackages =
                Directory.EnumerateDirectories(localWorkingCopyPath, "Schemas", SearchOption.AllDirectories);
            foreach (string packageRootDir in from packagePath in branchPackages
                where !string.IsNullOrEmpty(packagePath)
                let slashPosition = packagePath.LastIndexOf('\\')
                select packagePath.Substring(0, slashPosition)) {
                DeleteProperty(packageRootDir, "PackageUpdateDate");
            }

            return true;
        }

        private bool MakePropertiesCommit(string logMessage = "") {
            return Commit(WorkingCopyPath,
                new SvnCommitArgs {
                    LogMessage = !string.IsNullOrEmpty(logMessage)
                        ? logMessage
                        : "#0\nУстановка даты обновления пакетов из релиза."
                });
        }

        public bool FixBranch() {
            return SetPackageProperty(WorkingCopyPath)
                   && MakePropertiesCommit()
                   && RemovePackageProperty(WorkingCopyPath)
                   && MakePropertiesCommit(
                       "#0\nУдаление технического свойства: дата обновления пакетов из релиза.");
        }
    }
}