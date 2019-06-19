using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SharpSvn;

namespace Terrasoft.Core.SVN
{
    /// <inheritdoc />
    public sealed partial class SvnUtils
    {
        /// <summary>
        ///     Интеграция родительской ветки в ветку фитчи
        /// </summary>
        /// <returns>Результат успешности слияния</returns>
        public bool UpdateFromReleaseBranch() {
            if (!TryDoSvnAction(() => UpdateWorkingCopy(WorkingCopyPath))) {
                return false;
            }

            long revision = 1;
            TryDoSvnAction(() => {
                    revision = GetFeatureFirstRevisionNumber(WorkingCopyPath);
                    return true;
                }
            );

            string basePath = string.Empty;
            TryDoSvnAction(() => {
                    basePath = GetBaseBranchPath(revision, WorkingCopyPath);
                    return true;
                }
            );

            return TryDoSvnAction(() => MergeBaseBranchIntoFeature(WorkingCopyPath, basePath));
        }

        /// <summary>
        ///     Слияние родительской ветки в ветку фитчи
        /// </summary>
        /// <param name="workingCopyPath">Рабочая папка</param>
        /// <param name="basePathUrl">URL родительской ветки</param>
        /// <returns>Результат слияния</returns>
        private bool MergeBaseBranchIntoFeature(string workingCopyPath, string basePathUrl) {
            var svnMergeArgs = new SvnMergeArgs {Force = false};
            svnMergeArgs.Notify += OnSvnMergeArgsOnNotify;
            svnMergeArgs.Conflict += OnSvnConflict;
            bool mergeResult;
            try {
                var mergeRange = new SvnRevisionRange(SvnRevision.One, SvnRevision.Head);
                mergeResult = Merge(workingCopyPath, SvnUriTarget.FromString(basePathUrl), mergeRange,
                    svnMergeArgs
                );

                for (var index = 0; index < ConflictList.Count; index++) {
                    string json = ConflictList[index];
                    var converters = new JsonSerializerSettings();
                    JsonConverter converter = new StringEnumConverter(new DefaultNamingStrategy(), false);
                    converters.Converters.Add(converter);
                    dynamic svnConflictEventArgs = JsonConvert.DeserializeObject(json, converters);
                    string fullPath = svnConflictEventArgs.Conflict.FullPath.ToString();
                    if (svnConflictEventArgs.ConflictAction == SvnConflictAction.Delete &&
                        svnConflictEventArgs.ConflictReason == SvnConflictReason.Edited) {
                        Resolve(fullPath, SvnAccept.Working);
                        Delete(fullPath, new SvnDeleteArgs {Force = true, KeepLocal = false});
                        ConflictList.Remove(json);
                        index--;
                    } else if (svnConflictEventArgs.ConflictAction == SvnConflictAction.Replace &&
                               svnConflictEventArgs.ConflictReason == SvnConflictReason.Edited) {
                        Resolve(fullPath, SvnAccept.Working);
                        Delete(fullPath,
                            new SvnDeleteArgs {Force = true, KeepLocal = false}
                        );
                        string rightSource = svnConflictEventArgs.Conflict.RightSource.Uri.ToString();
                        Copy(SvnTarget.FromString(rightSource),
                            fullPath
                        );
                        ConflictList.Remove(json);
                        index--;
                    } else {
                        var mergeAppStartInfo = new ProcessStartInfo {
                            FileName = "TortoiseProc.exe",
                            Arguments = $"/command:resolve /path:{svnConflictEventArgs.Conflict.FullPath} /closeonend:3"
                        };
                        using (var mergeApp = new Process()) {
                            mergeApp.StartInfo = mergeAppStartInfo;
                            mergeApp.Start();
                            mergeApp.WaitForExit();
                            if (mergeApp.ExitCode != -1) {
                                continue;
                            }

                            ConflictList.Remove(json);
                            ConflictList.Remove(json);
                        }
                    }
                }

                if (ConflictList.Count > 0) {
                    BugReporter.SendBugReport(ConflictList, ConflictList.GetType());
                }
            } catch (SvnException svnException) {
                SVN.Logger.Error(svnException.Message);
                mergeResult = false;
            } finally {
                svnMergeArgs.Notify -= OnSvnMergeArgsOnNotify;
                svnMergeArgs.Conflict -= OnSvnConflict;
            }

            return mergeResult;
        }
    }
}