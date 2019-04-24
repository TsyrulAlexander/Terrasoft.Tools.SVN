using System;
using System.Globalization;
using System.IO;
using SharpSvn;

namespace Terrasoft.Tools.Svn
{
#pragma warning disable CA1501 // Avoid excessive inheritance
    internal sealed partial class SvnUtils
    {
        /// <summary>
        ///     Отображение лога при работе с историей
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnNotifyEventArgs">Элемент истории</param>
        private static void SvnLogArgsOnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            if (svnNotifyEventArgs is null) {
                throw new ArgumentNullException(nameof(svnNotifyEventArgs));
            }

            Logger.Info(svnNotifyEventArgs.NodeKind.ToString("G"), svnNotifyEventArgs.Path);
        }

        /// <summary>
        ///     Обработчик слияния
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnNotifyEventArgs">Аргумент</param>
        private static void OnSvnMergeArgsOnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            Logger.Info(svnNotifyEventArgs.Action.ToString(), svnNotifyEventArgs.Path);
        }

        /// <summary>
        ///     Обработчик конфликтов при слиянии
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnConflictEventArgs">Аргумент</param>
        private void OnSvnMergeArgsOnConflict(object sender, SvnConflictEventArgs svnConflictEventArgs)
        {
            AutoResolveConflict(svnConflictEventArgs);
        }

        /// <summary>
        ///     Обработчик фиксации изменений
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnCommittingEventArgs">Аргумент</param>
        private static void SvnCommitArgsOnCommitting(object sender, SvnCommittingEventArgs svnCommittingEventArgs)
        {
            Logger.Info(Resources.ResourceManager.GetString(
                    "SvnUtils_SvnCommitArgsOnCommitting_Items_to_commit", CultureInfo.CurrentCulture
                ),
                svnCommittingEventArgs.Items.Count.ToString(CultureInfo.CurrentCulture)
            );
        }

        /// <summary>
        ///     Обработчик информации при фиксации изменений
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnNotifyEventArgs">Аргумент</param>
        private static void SvnCommitArgsOnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            Logger.Info(svnNotifyEventArgs.Action.ToString("G"), svnNotifyEventArgs.Path);
        }

        /// <summary>
        ///     Обработчик после фиксации изменений
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnCommittedEventArgs">Аргумент</param>
        private static void SvnCommitArgsOnCommitted(object sender, SvnCommittedEventArgs svnCommittedEventArgs)
        {
            Logger.Info(Resources.ResourceManager.GetString(
                    "SvnUtils_SvnCommitArgsOnCommitted_Commited_revision",CultureInfo.CurrentCulture),
                svnCommittedEventArgs.Revision.ToString(CultureInfo.CurrentCulture)
            );
        }

        /// <summary>
        ///     Обработчик выгрузки изменений в локальную копию
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnNotifyEventArgs">Аргумент</param>
        private static void SvnCheckOutArgsOnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            Logger.Info(svnNotifyEventArgs.Action.ToString(), svnNotifyEventArgs.Path);
        }

        /// <summary>
        ///     Обработчик информирования об ре интеграции
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnNotifyEventArgs">Аргумент</param>
        private static void SvnReintegrationMergeArgsOnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            if (svnNotifyEventArgs.Action == SvnNotifyAction.TreeConflict) {
                Logger.Error(svnNotifyEventArgs.Action.ToString(), svnNotifyEventArgs.FullPath);
            } else {
                Logger.Info(svnNotifyEventArgs.Action.ToString(), svnNotifyEventArgs.FullPath);
            }
        }

        /// <summary>
        ///     Обработчик конфликтов при ре интеграции
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnConflictEventArgs">Аргумент</param>
        private void SvnReintegrationMergeArgsOnConflict(object sender, SvnConflictEventArgs svnConflictEventArgs)
        {
            AutoResolveConflict(svnConflictEventArgs);
        }

        /// <summary>
        ///     Обработчик информации о копировании
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnNotifyEventArgs">Аргумент</param>
        private static void SvnCopyArgsOnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            Logger.Info(svnNotifyEventArgs.Uri.ToString());
        }

        /// <summary>
        ///     Обработчик информации об обновлении
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnNotifyEventArgs">Аргумент</param>
        private static void SvnUpdateArgsOnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            Logger.Info(string.Format(CultureInfo.CurrentCulture,
                    Resources.ResourceManager.GetString("SvnUtils_SvnUpdateArgsOnNotify_Update_to_revision",
                        CultureInfo.CurrentCulture
                    ) ??
                    throw new InvalidOperationException(),
                    svnNotifyEventArgs.Path,
                    svnNotifyEventArgs.Revision.ToString(CultureInfo.CurrentCulture)
                )
            );
        }

        /// <summary>
        ///     Обработчик ошибок при обновлении
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnErrorEventArgs">Аргумент</param>
        private static void SvnUpdateArgsOnSvnError(object sender, SvnErrorEventArgs svnErrorEventArgs)
        {
            Logger.Error(svnErrorEventArgs.Exception.Message, svnErrorEventArgs.Exception.SvnErrorCategory.ToString());
        }

        /// <summary>
        ///     Обработчик ошибок при обновлении
        /// </summary>
        /// <param name="sender">Контекст</param>
        /// <param name="svnConflictEventArgs">Аргумент</param>
        private void SvnUpdateArgsOnConflict(object sender, SvnConflictEventArgs svnConflictEventArgs)
        {
            AutoResolveConflict(svnConflictEventArgs);
        }

        /// <summary>
        ///     Обработчик конфликтов по типу
        /// </summary>
        /// <param name="svnConflictEventArgs">Аргумент</param>
        /// <exception cref="ArgumentOutOfRangeException">Возможные ошибки</exception>
        private void ResolveConflictByType(SvnConflictEventArgs svnConflictEventArgs)
        {
            switch (svnConflictEventArgs.ConflictType) {
                case SvnConflictType.Tree:
                    ResolveConflictByTreeReason(svnConflictEventArgs);
                    break;
                case SvnConflictType.Content:
                    svnConflictEventArgs.Choice = SvnAccept.Theirs;
                    break;
                case SvnConflictType.Property:
                    svnConflictEventArgs.Choice = SvnAccept.Theirs;
                    break;
            }
        }

        /// <summary>
        ///     Обработка конфликта по причине
        /// </summary>
        /// <param name="svnConflictEventArgs">Аргумент</param>
        /// <exception cref="ArgumentOutOfRangeException">Возможные ошибки</exception>
        private void ResolveConflictByTreeReason(SvnConflictEventArgs svnConflictEventArgs)
        {
            switch (svnConflictEventArgs.ConflictReason) {
                case SvnConflictReason.Added:
                    ResolveConflictByTreeAddedAction(svnConflictEventArgs);
                    break;
                case SvnConflictReason.Edited:
                    break;
                case SvnConflictReason.MovedHere:
                    break;
                case SvnConflictReason.MovedAway:
                    break;
                case SvnConflictReason.Replaced:
                    break;
                case SvnConflictReason.NotVersioned:
                    break;
                case SvnConflictReason.Missing:
                    break;
                case SvnConflictReason.Deleted:
                    break;
                case SvnConflictReason.Obstructed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(svnConflictEventArgs));
            }
        }

        /// <summary>
        ///     Обработка конфликт существующей папки
        /// </summary>
        /// <param name="svnConflictEventArgs">Аргумент</param>
        private static void AddToExistsFolderConflict(SvnConflictEventArgs svnConflictEventArgs)
        {
            if (!Directory.Exists(svnConflictEventArgs.Conflict.FullPath)) {
                return;
            }

            string targetPath = svnConflictEventArgs.Conflict.RightSource.RepositoryRoot.ToString();
            string conflictRelativePath =
                svnConflictEventArgs.Conflict.RightSource.RepositoryPath.ToString();
            if (conflictRelativePath.EndsWith("/", StringComparison.Ordinal)) {
                conflictRelativePath =
                    $"/{conflictRelativePath.Remove(conflictRelativePath.Length - 1, 1)}";
            }

            if (FindOwnerInLog(targetPath, conflictRelativePath)) {
                string destinationFolder = svnConflictEventArgs.Conflict.FullPath.Clone().ToString();
                BackupExistsFolder(destinationFolder);
                svnConflictEventArgs.Choice = ExtractContentInMergedFolder(
                    svnConflictEventArgs.Conflict.RightSource.Uri.ToString(),
                    destinationFolder
                )
                    ? SvnAccept.Working
                    : SvnAccept.Postpone;
            } else {
                svnConflictEventArgs.Choice = SvnAccept.Postpone;
            }
        }


        /// <summary>
        ///     Обработчик конфликта по действию
        /// </summary>
        /// <param name="svnConflictEventArgs">Аргумент</param>
        /// <exception cref="ArgumentOutOfRangeException">Возможная ошибка</exception>
        private void ResolveConflictByTreeAddedAction(SvnConflictEventArgs svnConflictEventArgs)
        {
            switch (svnConflictEventArgs.ConflictAction) {
                case SvnConflictAction.Replace:
                    break;
                case SvnConflictAction.Delete:
                    break;
                case SvnConflictAction.Edit:
                    break;
                case SvnConflictAction.Add:
                    ResolveConflictByTreeAddedAddNode(svnConflictEventArgs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(svnConflictEventArgs));
            }
        }

        /// <summary>
        ///     Обработчик ошибок по типу узла
        /// </summary>
        /// <param name="svnConflictEventArgs"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ResolveConflictByTreeAddedAddNode(SvnConflictEventArgs svnConflictEventArgs)
        {
            if (svnConflictEventArgs.NodeKind == SvnNodeKind.Directory &&
                svnConflictEventArgs.ConflictAction == SvnConflictAction.Add) {
                AddToExistsFolderConflict(svnConflictEventArgs);
                return;
            }

            if (svnConflictEventArgs.NodeKind == SvnNodeKind.File &&
                svnConflictEventArgs.ConflictAction == SvnConflictAction.Add) {
                NeedResolveList.Add(svnConflictEventArgs.Conflict.FullPath.Clone().ToString());
                svnConflictEventArgs.Choice = SvnAccept.Postpone;
            }
        }


        /// <summary>
        ///     Обработчик ошибки
        /// </summary>
        /// <param name="e">Аргумент</param>
        private void AutoResolveConflict(SvnConflictEventArgs e)
        {
            ResolveConflictByType(e);
            if (e.Choice == SvnAccept.Postpone) {
                BugReporter.SendBugReport(e, e.GetType());
            }

            /*switch (svnNotifyEventArgs.ConflictAction) {
                case SvnConflictAction.Add when svnNotifyEventArgs.ConflictType == SvnConflictType.Tree &&
                                                svnNotifyEventArgs.NodeKind == SvnNodeKind.Directory &&
                                                svnNotifyEventArgs.ConflictReason == SvnConflictReason.Obstructed &&
                                                Directory.Exists(svnNotifyEventArgs.Conflict.FullPath):
                    Logger.Error("Попытка добавить папку, которая уже существует:"
                        , $"\nАдрес бранчи\t{svnNotifyEventArgs.Conflict.RightSource.Target}\nАдрес релиза\t{svnNotifyEventArgs.Conflict.LeftSource.Target}");
                    Logger.Error("Производим автоматическое слияние:", "оставляем существующую.");
                    svnNotifyEventArgs.Choice = SvnAccept.Working;
                    return;
                case SvnConflictAction.Delete when svnNotifyEventArgs.ConflictReason == SvnConflictReason.Missing &&
                                                   svnNotifyEventArgs.NodeKind == SvnNodeKind.Directory &&
                                                   !Directory.Exists(svnNotifyEventArgs.Conflict.FullPath):
                    Logger.Error("Попытка удалить папку, которая уже удалена:"
                        , $"\nАдрес бранчи\t{svnNotifyEventArgs.Conflict.RightSource.Target}\nАдрес релиза\t{svnNotifyEventArgs.Conflict.LeftSource.Target}");
                    Logger.Error("Производим автоматическое слияние:", "принимаем удаление.");
                    svnNotifyEventArgs.Choice = SvnAccept.Working;
                    return;
                case SvnConflictAction.Add when svnNotifyEventArgs.ConflictType == SvnConflictType.Tree &&
                                                svnNotifyEventArgs.NodeKind == SvnNodeKind.File &&
                                                svnNotifyEventArgs.ConflictReason == SvnConflictReason.Obstructed &&
                                                File.Exists(svnNotifyEventArgs.Conflict.FullPath):
                    Logger.Error("Попытка добавить файл который уже существует:"
                        , $"\nАдрес бранчи\t{svnNotifyEventArgs.Conflict.RightSource.Target}\nАдрес релиза\t{svnNotifyEventArgs.Conflict.LeftSource.Target}");
                    Logger.Error("Не возможно произвести автоматическое слияние:",
                        "требуется провести слияние в ручном режиме.");
                    svnNotifyEventArgs.Choice = SvnAccept.Postpone;
                    CommitIfNoError = false;
                    return;
                case SvnConflictAction.Delete when svnNotifyEventArgs.ConflictReason == SvnConflictReason.Missing &&
                                                   svnNotifyEventArgs.NodeKind == SvnNodeKind.None &&
                                                   !Directory.Exists(svnNotifyEventArgs.Conflict.FullPath):
                    Logger.Error("Попытка удалить ветку, которая уже удалена:"
                        , $"\nАдрес бранчи\t{svnNotifyEventArgs.Conflict.RightSource.Target}\nАдрес релиза\t{svnNotifyEventArgs.Conflict.LeftSource.Target}");
                    Logger.Error("Производим автоматическое слияние:", "принимаем удаление.");
                    svnNotifyEventArgs.Choice = SvnAccept.Working;
                    return;
                default:
                    svnNotifyEventArgs.Choice = SvnAccept.Postpone;
                    Logger.Error("Не возможно произвести автоматическое слияние:", "не найдено подходящее правило.");
                    break;
            }*/
        }
#pragma warning restore CA1501 // Avoid excessive inheritance
    }
}