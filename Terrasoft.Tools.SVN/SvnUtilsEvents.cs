using SharpSvn;

namespace Terrasoft.Tools.SVN
{
    using System;
    using System.IO;

    public partial class SvnUtils
    {
        private void SvnLogArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void OnSvnMergeArgsOnNotify(object sender, SvnNotifyEventArgs args) {
            Console.WriteLine(args.Path);
        }

        private void OnSvnMergeArgsOnConflict(object sender, SvnConflictEventArgs args) {
            Console.WriteLine(args.Conflict.FullPath);
        }

        private void SvnCommitArgsOnCommitting(object sender, SvnCommittingEventArgs e) {
            Console.WriteLine("Items to commit: {0}", e.Items.Count);
        }

        private void SvnCommitArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void SvnCommitArgsOnCommitted(object sender, SvnCommittedEventArgs e) {
            Console.WriteLine("Commited revision {0}.", e.Revision);
        }

        private void SvnCheckOutArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void SvnReintegrationMergeArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void SvnReintegrationMergeArgsOnConflict(object sender, SvnConflictEventArgs e) {
            Console.WriteLine("Conflict in file {0}", e.Conflict.FullPath);
            ConsoleKeyInfo resolveAction = Console.ReadKey();
            switch (char.ToLower(resolveAction.KeyChar)) {
                case 'm':
                    e.Choice = SvnAccept.Mine;
                    break;
                case 't':
                    e.Choice = SvnAccept.Theirs;
                    break;
                default:
                    e.Choice = SvnAccept.Postpone;
                    break;
            }
        }

        private static void SvnCopyArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Uri);
        }

        private void SvnUpdateArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine("Update {0} to revision #{1}", e.Path, e.Revision);
        }
    }
}
