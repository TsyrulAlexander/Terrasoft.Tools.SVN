using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SharpSvn;
using Terrasoft.Tools.Svn.Properties;

namespace Terrasoft.Tools.Svn
{
    /// <inheritdoc />
    /// <summary>
    ///     Класс реализации SVN клиента
    /// </summary>
    internal sealed partial class SvnUtils
    {
        /// <inheritdoc />
        /// <summary>
        ///     Конструктор SVN клиента
        /// </summary>
        /// <param name="programOptions">Словарь с параметрами</param>
        internal SvnUtils(IReadOnlyDictionary<string, string> programOptions) : base(programOptions) { }

        /// <summary>
        ///     Получить URL ветки из которой была выделена фитча
        /// </summary>
        /// <param name="revision">Номер ревизии в которой выделена ветка</param>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Строка с URL</returns>
        private string GetBaseBranchPath(long revision, string workingCopyPath)
        {
            string basePath = string.Empty;
            var svnInfoArgs = new SvnInfoArgs {Revision = new SvnRevision(revision)};
            Info(SvnTarget.FromString(workingCopyPath), svnInfoArgs, (sender, args) => {
                    basePath = args.Uri.ToString();
                    Console.WriteLine(args.Uri.ToString());
                }
            );
            return basePath;
        }

        /// <summary>
        ///     Проверка рабочей копии на ошибки
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Результат проверки</returns>
        private bool CheckWorkingCopyForError(string workingCopyPath)
        {
            var result = true;
            Status(workingCopyPath, (sender, args) => {
                    if (result && args.Conflicted) {
                        result = !args.Conflicted;
                    }
                }
            );
            return result;
        }

        /// <summary>
        ///     Получить номер ревизии выделения ветки
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Номер ревизии</returns>
        private long GetFeatureFirstRevisionNumber(string workingCopyPath)
        {
            long revision = 0;
            var svnLogArgs = new SvnLogArgs {StrictNodeHistory = false};
            svnLogArgs.Notify += SvnLogArgsOnNotify;
            string branchLocalPath = string.Empty;
            Info(SvnTarget.FromString(workingCopyPath),
                (sender, args) => {
                    branchLocalPath = args.Uri.LocalPath.Remove(0, args.RepositoryRoot.LocalPath.Length - 1);
                    branchLocalPath = branchLocalPath.Remove(branchLocalPath.Length - 1);
                }
            );
            Log(workingCopyPath, svnLogArgs, (sender, args) => {
                    if (args.ChangedPaths.Count != 1) {
                        return;
                    }

                    foreach (SvnChangeItem changeItem in args.ChangedPaths) {
                        if (string.IsNullOrEmpty(changeItem.CopyFromPath)) {
                            continue;
                        }

                        if (changeItem.Action == SvnChangeAction.Add) {
                            if (changeItem.CopyFromPath != branchLocalPath && changeItem.Path == branchLocalPath) {
                                revision = changeItem.CopyFromRevision;
                            }
                        }
                    }
                }
            );
            return revision;
        }

        /// <summary>
        ///     Получить последний номер ревизии родительской ветки
        /// </summary>
        /// <param name="basePath">URL родительской ветки</param>
        /// <returns>Номер ревизии</returns>
        private long GetBaseBranchHeadRevision(string basePath)
        {
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
        internal bool CommitChanges(bool checkError = false, string logMessage = "")
        {
            if (checkError && !CheckWorkingCopyForError(WorkingCopyPath)) {
                Logger.Error(Resources.SvnUtils_CommitChanges_Sources_not_resolved);
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

        /// <summary>
        ///     Установка технических свойств рабочей копии
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns></returns>
        private bool SetPackageProperty(string workingCopyPath = "")
        {
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

        /// <summary>
        ///     Удаление технических свойств
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns></returns>
        private bool RemovePackageProperty(string workingCopyPath = "")
        {
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

        /// <summary>
        ///     Фиксация технических свойств
        /// </summary>
        /// <param name="logMessage">Комментарий</param>
        /// <returns>Результат выполнения</returns>
        private bool MakePropertiesCommit(string logMessage = "")
        {
            return Commit(WorkingCopyPath,
                new SvnCommitArgs {
                    LogMessage = !string.IsNullOrEmpty(logMessage)
                        ? logMessage
                        : "#0\nУстановка даты обновления пакетов из релиза."
                }
            );
        }

        /// <summary>
        ///     Исправление ветки путём добавления и удаления технического свойства
        /// </summary>
        /// <returns></returns>
        internal bool FixBranch()
        {
            return SetPackageProperty(WorkingCopyPath) &&
                   MakePropertiesCommit() &&
                   RemovePackageProperty(WorkingCopyPath) &&
                   MakePropertiesCommit(
                       "#0\nУдаление технического свойства: дата обновления пакетов из релиза."
                   );
        }

        /// <summary>
        ///     Поиск автора входящих изменений в истории
        /// </summary>
        /// <param name="targetPath">URL репозитория.</param>
        /// <param name="conflictRelativePath">Относительный путь конфликтного контента.</param>
        /// <returns>Результат</returns>
        private static bool FindOwnerInLog(string targetPath, string conflictRelativePath)
        {
            var fended = false;
            using (var svnClient = new SvnClient()) {
                void LogHandler(object sender, SvnLogEventArgs args)
                {
                    if (args?.ChangedPaths is null) {
                        return;
                    }

                    foreach (SvnChangeItem changeItem in args.ChangedPaths) {
                        if (changeItem.Action != SvnChangeAction.Add) {
                            continue;
                        }

                        if (!changeItem.Path.StartsWith(conflictRelativePath, StringComparison.Ordinal)) {
                            continue;
                        }

                        Logger.Warning(changeItem.Path == conflictRelativePath
                                ? Resources.ResourceManager.GetString("FolderExistAndWouldBackuped",
                                    CultureInfo.CurrentCulture
                                )
                                : Resources.ResourceManager.GetString("RemoveFolderContainFiles",
                                    CultureInfo.CurrentCulture
                                ),
                            string.Format(CultureInfo.CurrentCulture,
                                Resources.ResourceManager.GetString("FolderAddedInRevision", CultureInfo.CurrentCulture
                                ) ??
                                throw new InvalidOperationException(), args.Author, args.Revision
                            )
                        );
                        fended = true;
                    }
                }

                svnClient.Log(new Uri(targetPath),
                    new SvnLogArgs {RetrieveChangedPaths = true, RetrieveMergedRevisions = false},
                    LogHandler
                );
                return fended;
            }
        }

        /// <summary>
        ///     Выгрузка источника в указанную папку
        /// </summary>
        /// <param name="sourceRepository">URL источник</param>
        /// <param name="destinationFolder">Папка получатель</param>
        /// <returns>Результат</returns>
        private static bool ExtractContentInMergedFolder(string sourceRepository, string destinationFolder)
        {
            using (var client = new SvnClient()) {
                var svnExportArgs = new SvnExportArgs {Overwrite = true};

                void OnSvnExportArgsOnNotify(object sender, SvnNotifyEventArgs args)
                {
                    Logger.Info(args.Action.ToString("G"), args.FullPath);
                }

                svnExportArgs.Notify += OnSvnExportArgsOnNotify;
                svnExportArgs.SvnError += (sender, args) => Logger.Error(args.Exception.Message);
                bool exportResult = client.Export(SvnTarget.FromString(sourceRepository), destinationFolder,
                    svnExportArgs
                );
                return exportResult;
            }
        }

        /// <summary>
        ///     Создание резервной копии указанной папки
        /// </summary>
        /// <param name="src"></param>
        private static void BackupExistsFolder(string src)
        {
            string[] files = Directory.GetFiles(src);
            string targetPath = src + ".tmp";
            if (!Directory.Exists(targetPath)) {
                Directory.CreateDirectory(targetPath);
            }

            string[] directories = Directory.GetDirectories(src);
            foreach (string directory in directories.Where(directory => !Directory.Exists(targetPath))) {
                Directory.CreateDirectory(Path.Combine(targetPath, directory));
            }

            foreach (string file in files) {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetPath, fileName);
                File.Copy(file, destFile, true);
            }
        }
    }
}