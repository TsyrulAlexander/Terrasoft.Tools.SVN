using SharpSvn;

namespace Terrasoft.Tools.SVN
{
    using System;
    using Terrasoft.Tools.SVN.Properties;

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
            Console.WriteLine(Resources.SvnUtils_SvnCommitArgsOnCommitting_Items_to_commit, e.Items.Count);
        }

        private void SvnCommitArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void SvnCommitArgsOnCommitted(object sender, SvnCommittedEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnCommitArgsOnCommitted_Commited_revision, e.Revision);
        }

        private void SvnCheckOutArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void SvnReintegrationMergeArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private void SvnReintegrationMergeArgsOnConflict(object sender, SvnConflictEventArgs e) {
            while (e.Choice == SvnAccept.Unspecified) {
                Console.WriteLine(Resources.SvnUtils_SvnReintegrationMergeArgsOnConflict_Conflict_in_file,
                    e.Conflict.FullPath);
                string resolveAction = Console.ReadLine();
                if (resolveAction != null) {
                    switch (resolveAction.ToLowerInvariant()) {
                        case "m":
                            e.Choice = SvnAccept.Mine;
                            break;
                        case "mf":
                            e.Choice = SvnAccept.MineFull;
                            break;
                        case "t":
                            e.Choice = SvnAccept.Theirs;
                            break;
                        case "tf":
                            e.Choice = SvnAccept.TheirsFull;
                            break;
                        case "p":
                            e.Choice = SvnAccept.Postpone;
                            break;
                        case "w":
                            e.Choice = SvnAccept.Working;
                            break;

                        default:
                            e.Choice = SvnAccept.Unspecified;
                            break;
                    }
                } else {
                    Console.WriteLine(Resources.SvnUtils_SvnReintegrationMergeArgsOnConflict_Action_Not_Selected);
                }
            }
        }

        private static void SvnCopyArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Uri);
        }

        private void SvnUpdateArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnUpdateArgsOnNotify_Update_to_revision, e.Path, e.Revision);
        }
    }
}
