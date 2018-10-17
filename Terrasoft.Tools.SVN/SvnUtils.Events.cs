using System;
using System.IO;
using SharpSvn;
using Terrasoft.Tools.SVN.Properties;

namespace Terrasoft.Tools.SVN
{
    internal sealed partial class SvnUtils
    {
        private static void SvnLogArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private static void OnSvnMergeArgsOnNotify(object sender, SvnNotifyEventArgs args) {
            Logger.LogInfo(args.Action.ToString(), args.Path);
        }

        private void OnSvnMergeArgsOnConflict(object sender, SvnConflictEventArgs e) {
            if (!AutoMerge) {
                Logger.LogError($"Найден конфликт, с типом {e.Conflict.NodeKind}: "
                    , $"\nАдрес бранчи\t{e.Conflict.RightSource.Target}\nАдрес релиза\t{e.Conflict.LeftSource.Target}");
                return;
            }

            AutoResolveConflict(e);
        }

        private static void SvnCommitArgsOnCommitting(object sender, SvnCommittingEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnCommitArgsOnCommitting_Items_to_commit, e.Items.Count.ToString());
        }

        private static void SvnCommitArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private static void SvnCommitArgsOnCommitted(object sender, SvnCommittedEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnCommitArgsOnCommitted_Commited_revision, e.Revision.ToString());
        }

        private static void SvnCheckOutArgsOnNotify(object sender, SvnNotifyEventArgs args) {
            Logger.LogInfo(args.Action.ToString(), args.Path);
        }

        private static void SvnReintegrationMergeArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            if (e.Action == SvnNotifyAction.TreeConflict) {
                Logger.LogError(e.Action.ToString(), e.FullPath);
            } else {
                Logger.LogInfo(e.Action.ToString(), e.FullPath);
            }
        }

        private void SvnReintegrationMergeArgsOnConflict(object sender, SvnConflictEventArgs e) {
            if (!AutoMerge) {
                Logger.LogError($"Найден конфликт, с типом {e.Conflict.NodeKind}: "
                    , $"\nАдрес бранчи\t{e.Conflict.RightSource.Target}\nАдрес релиза\t{e.Conflict.LeftSource.Target}");
                return;
            }

            AutoResolveConflict(e);
        }

        private static void SvnCopyArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Uri);
        }

        private static void SvnUpdateArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnUpdateArgsOnNotify_Update_to_revision, e.Path, e.Revision.ToString());
        }

        private void SvnUpdateArgsOnSvnError(object sender, SvnErrorEventArgs e) {
            throw new NotImplementedException();
        }

        private void SvnUpdateArgsOnConflict(object sender, SvnConflictEventArgs e) {
            AutoResolveConflict(e);
        }

        private void AutoResolveConflict(SvnConflictEventArgs e) {
            switch (e.ConflictAction) {
                case SvnConflictAction.Add when e.ConflictType == SvnConflictType.Tree &&
                                                e.NodeKind == SvnNodeKind.Directory &&
                                                e.ConflictReason == SvnConflictReason.Obstructed &&
                                                Directory.Exists(e.Conflict.FullPath):
                    Logger.LogError("Попытка добавить папку, которая уже существует:"
                        , $"\nАдрес бранчи\t{e.Conflict.RightSource.Target}\nАдрес релиза\t{e.Conflict.LeftSource.Target}");
                    Logger.LogError("Производим автоматическое слияние:", "оставляем существующую.");
                    e.Choice = SvnAccept.Working;
                    return;
                case SvnConflictAction.Delete when e.ConflictReason == SvnConflictReason.Missing &&
                                                   e.NodeKind == SvnNodeKind.Directory &&
                                                   !Directory.Exists(e.Conflict.FullPath):
                    Logger.LogError("Попытка удалить папку, которая уже удалена:"
                        , $"\nАдрес бранчи\t{e.Conflict.RightSource.Target}\nАдрес релиза\t{e.Conflict.LeftSource.Target}");
                    Logger.LogError("Производим автоматическое слияние:", "принимаем удаление.");
                    e.Choice = SvnAccept.Working;
                    return;
                case SvnConflictAction.Add when e.ConflictType == SvnConflictType.Tree &&
                                                e.NodeKind == SvnNodeKind.File &&
                                                e.ConflictReason == SvnConflictReason.Obstructed &&
                                                File.Exists(e.Conflict.FullPath):
                    Logger.LogError("Попытка добавить файл который уже существует:"
                        , $"\nАдрес бранчи\t{e.Conflict.RightSource.Target}\nАдрес релиза\t{e.Conflict.LeftSource.Target}");
                    Logger.LogError("Не возможно произвести автоматическое слияние:",
                        "требуется провести слияние в ручном режиме.");
                    e.Choice = SvnAccept.Postpone;
                    CommitIfNoError = false;
                    return;
                case SvnConflictAction.Delete when e.ConflictReason == SvnConflictReason.Missing &&
                                                   e.NodeKind == SvnNodeKind.None &&
                                                   !Directory.Exists(e.Conflict.FullPath):
                    Logger.LogError("Попытка удалить ветку, которая уже удалена:"
                        , $"\nАдрес бранчи\t{e.Conflict.RightSource.Target}\nАдрес релиза\t{e.Conflict.LeftSource.Target}");
                    Logger.LogError("Производим автоматическое слияние:", "принимаем удаление.");
                    e.Choice = SvnAccept.Working;
                    return;
                default:
                    e.Choice = SvnAccept.Postpone;
                    Logger.LogError("Не возможно произвести автоматическое слияние:", "не найдено подходящее правило.");
                    break;
            }

            CommitIfNoError = false;
        }
    }
}