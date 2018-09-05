using System;
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
            Console.WriteLine(args.Path);
        }

        private void OnSvnMergeArgsOnConflict(object sender, SvnConflictEventArgs e) {
            if (!AutoMerge) {
                return;
            }

            Logger.LogError($"Найден конфликт, с типом {e.Conflict.NodeKind}: "
              , $"\nАдрес бранчи\t{e.Conflict.LeftSource.Target}\nАдрес релиза\t{e.Conflict.RightSource.Target}");
            e.Choice = e.Conflict.NodeKind == SvnNodeKind.File
                ? SvnAccept.Theirs
                : SvnAccept.Merged;
            Logger.LogError("Производим автоматическое слияние: ", e.Choice == SvnAccept.Theirs
                ? "принимаем входящие изменения как главные."
                : "объеденяем папки.");
            CommitIfNoError = false;
        }

        private static void SvnCommitArgsOnCommitting(object sender, SvnCommittingEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnCommitArgsOnCommitting_Items_to_commit, e.Items.Count);
        }

        private static void SvnCommitArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private static void SvnCommitArgsOnCommitted(object sender, SvnCommittedEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnCommitArgsOnCommitted_Commited_revision, e.Revision);
        }

        private static void SvnCheckOutArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private static void SvnReintegrationMergeArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void SvnReintegrationMergeArgsOnConflict(object sender, SvnConflictEventArgs e) {
            if (!AutoMerge) {
                return;
            }

            Logger.LogError($"Найден конфликт, с типом {e.Conflict.NodeKind}: "
              , $"\nАдрес бранчи\t{e.Conflict.RightSource.Target}\nАдрес релиза\t{e.Conflict.LeftSource.Target}");
            e.Choice = e.Conflict.NodeKind == SvnNodeKind.File
                ? SvnAccept.Theirs
                : SvnAccept.Merged;
            Logger.LogError("Производим автоматическое слияние: ", e.Choice == SvnAccept.Theirs
                ? "принимаем входящие изменения как главные."
                : "объеденяем папки.");
            CommitIfNoError = false;
        }

        private static void SvnCopyArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Uri);
        }

        private static void SvnUpdateArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnUpdateArgsOnNotify_Update_to_revision, e.Path, e.Revision);
        }
    }
}