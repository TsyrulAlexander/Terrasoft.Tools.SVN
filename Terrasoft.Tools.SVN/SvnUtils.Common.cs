namespace Terrasoft.Tools.SVN
{
    using System;
    using System.Collections.Generic;
    using SharpSvn;
    using Terrasoft.Tools.SVN.Properties;

    /// <inheritdoc />
    /// <summary>
    /// Класс реализации SVN клиента
    /// </summary>
    public partial class SvnUtils
    {
        /// <inheritdoc />
        /// <summary>
        /// Конструктор SVN клиента
        /// </summary>
        /// <param name="programOptions">Словарь с параметрами</param>
        public SvnUtils(IReadOnlyDictionary<string, string> programOptions) : base(programOptions) {
        }

        /// <summary>
        /// Получить URL ветки из которой была веделена фитча
        /// </summary>
        /// <param name="revision">Номер ревизии в которой выделена ветка</param>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Строка с URL</returns>
        private string GetBaseBranchPath(long revision, string workingCopyPath) {
            string basePath = string.Empty;
            var svnInfoArgs = new SvnInfoArgs { Revision = new SvnRevision(revision) };
            Info(SvnTarget.FromString(workingCopyPath), svnInfoArgs, (sender, args) => {
                basePath = args.Uri.ToString();
                Console.WriteLine(args.Uri.ToString());
            });
            return basePath;
        }

        /// <summary>
        /// Проверка рабочей копии на ошибки
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
        /// Получить номер ревизии выделения ветки
        /// </summary>
        /// <param name="workingCopyPath">Путь к рабочей копии</param>
        /// <returns>Номер ревизии</returns>
        private long GetFeatureFirstRevisionNumber(string workingCopyPath) {
            long revision = 0;
            var svnLogArgs = new SvnLogArgs { StrictNodeHistory = true };
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
        /// Получить последний номер ревизии родительской ветки
        /// </summary>
        /// <param name="startRevision">Номер версии для отсчета</param>
        /// <param name="basePath">URL родительской ветки</param>
        /// <returns>Номер ревизии</returns>
        private long GetBaseBranchHeadRevision(long startRevision, string basePath) {
            long headRevision = 0;
            var svnLogArgs = new SvnLogArgs(new SvnRevisionRange(startRevision, SvnRevision.Head));

            Log(new Uri(basePath), svnLogArgs, (sender, args) => headRevision = args.Revision);
            return headRevision;
        }

        /// <summary>
        /// Зафиксировать изменения в хранилище
        /// </summary>
        /// <param name="checkEror">Проверить рабочую копию на ошибки перед фиксацией</param>
        /// <exception cref="SvnRepositoryException">Исключение в случае не разрешенных конфилктов рабочей копии</exception>
        /// <returns>Резальт фиксации изменений в хранилище</returns>
        public bool CommitChanges(bool checkEror = false) {
            if (checkEror && !CheckWorkingCopyForError(WorkingCopyPath)) {
                throw new SvnRepositoryException(Resources.SvnUtils_CommitChanges_Sources_not_resolved);
            }

            var svnCommitArgs = new SvnCommitArgs { LogMessage = Resources.SvnUtils_CommitChanges_Reintegrate_base_branch_to_feature };
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
    }
}
