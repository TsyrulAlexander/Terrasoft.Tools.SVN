using System;
using System.Diagnostics;
using System.IO;
using SharpSvn;
using Terrasoft.Tools.SVN.Properties;

namespace Terrasoft.Tools.SVN
{
    public partial class SvnUtils
    {
        private static void SvnLogArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(e.Path);
        }

        private static void OnSvnMergeArgsOnNotify(object sender, SvnNotifyEventArgs args) {
            Console.WriteLine(args.Path);
        }

        private static void OnSvnMergeArgsOnConflict(object sender, SvnConflictEventArgs args) {
            Console.WriteLine($@"Найден конфликт: {args.Conflict.FullPath}");
            Console.WriteLine($@"Попытка разрешить конфликт...");
            var process = new Process();
            var startInfo = new ProcessStartInfo {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "TortoiseMerge.exe",
                Arguments = $"/base:{args.Conflict.BaseFile} /theirs:{args.Conflict.TheirFile} " +
                            $"/mine:{args.Conflict.MyFile} /merged:{args.Conflict.MergedFile} " +
                            $"/basename:\"{args.Conflict.Name}: Working Base, rev {args.Conflict.LeftSource.Revision}\"" +
                            $"/theirsname:\"{args.Conflict.Name}: Remote file, rev {args.Conflict.RightSource.Revision}\"" +
                            $"/minename:\"{args.Conflict.Name}: Working Copy\"" +
                            $"/mergedname:\"{args.Conflict.Name}: Merged file\" "
            };
            process.StartInfo = startInfo;
            if (process.Start()) {
                process.WaitForExit();
            }

            if (!File.Exists(args.Conflict.BaseFile) && !File.Exists(args.Conflict.TheirFile)) {
                args.Choice = SvnAccept.Merged;
            } else {
                Console.WriteLine(
                    @"Конфликт не был разрешен! Перед фиксацией изменений обязательно разрешите конфликты.");
            }
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

        private static void SvnReintegrationMergeArgsOnConflict(object sender, SvnConflictEventArgs e) {
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

        private static void SvnUpdateArgsOnNotify(object sender, SvnNotifyEventArgs e) {
            Console.WriteLine(Resources.SvnUtils_SvnUpdateArgsOnNotify_Update_to_revision, e.Path, e.Revision);
        }
    }
}