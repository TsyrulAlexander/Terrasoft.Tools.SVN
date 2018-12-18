using System;
using System.Collections.Generic;
using System.Linq;
using SharpSvn;

namespace Terrasoft.Tools.SVN
{
    internal sealed partial class SvnUtils
    {
        /// <summary>
        ///     Импорт схемы из соседней ветки
        /// </summary>
        /// <returns></returns>
        public bool ShareSchemas() {
            string relativePath = string.Empty;

            Info(SvnTarget.FromString(SchemasUrl), (sender, args) => {
                int rootLength = args.RepositoryRoot.ToString().Length;
                string fullPath = args.Uri.ToString();

                relativePath = fullPath.Substring(rootLength);
            });

            Dictionary<string, SortedSet<long>> revisionList = GetSchemasRevisionFromLog(relativePath);
            var svnMergeArgs = new SvnMergeArgs();
            var mergeResult = true;
            try {
                svnMergeArgs.Notify += OnSvnMergeNotify;
                svnMergeArgs.Conflict += OnSvnMergeConflict;
                foreach (KeyValuePair<string, SortedSet<long>> revisionInfo in revisionList) {
                    string mergeUrl = string.Concat(relativePath, revisionInfo.Key);
                    List<SvnRevisionRange> mergeRange =
                        revisionInfo.Value.Select(revision => new SvnRevisionRange(revision - 1, revision)).ToList();
                    mergeResult = mergeResult && Merge(WorkingCopyPath, SvnUriTarget.FromString(mergeUrl), mergeRange,
                                      svnMergeArgs);
                }
            } catch (Exception e) {
                Logger.LogError(e.Message, e.ToString());
                mergeResult = false;
            } finally {
                svnMergeArgs.Notify -= OnSvnMergeNotify;
                svnMergeArgs.Conflict -= OnSvnMergeConflict;
            }

            return mergeResult;
        }

        /// <summary>
        ///     Получить перечень ревизий в которых присутствуют указанные схемы.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns>Список ревизий.</returns>
        private Dictionary<string, SortedSet<long>> GetSchemasRevisionFromLog(string relativePath) {
            var args = new SvnLogArgs {
                StrictNodeHistory = true
            };

            Dictionary<string, SortedSet<long>> schemasInfo = new Dictionary<string, SortedSet<long>>();

            void LogHandler(object sender, SvnLogEventArgs eventArgs) {
                foreach (SvnChangeItem changedPath in eventArgs.ChangedPaths) {
                    if (changedPath.NodeKind != SvnNodeKind.Directory) {
                        continue;
                    }

                    if (!IsExistsSchemasInRevision(changedPath.Path)) {
                        continue;
                    }

                    string key = changedPath.Path.Substring(relativePath.Length);
                    if (!schemasInfo.ContainsKey(key)) {
                        schemasInfo.Add(key, new SortedSet<long>());
                    }

                    SortedSet<long> info = schemasInfo[key];
                    info.Add(eventArgs.Revision);
                }
            }

            try {
                Log(new Uri(SchemasUrl), args, LogHandler);
            } catch (Exception e) {
                Logger.LogError(e.Message, e.ToString());
            }

            return schemasInfo;
        }

        /// <summary>
        ///     Существует ли схема или ресурсы в текущей ревизии
        /// </summary>
        /// <param name="changedPath">Путь</param>
        /// <returns>Да/Нет</returns>
        private bool IsExistsSchemasInRevision(string changedPath) {
            foreach (string schema in Schemas) {
                string schemaSuffix = $"Schemas/{schema}";
                string resourceSuffix = $"Resources/{schema}";
                if (changedPath.EndsWith(schemaSuffix)) {
                    return true;
                }

                if (changedPath.Contains(resourceSuffix)) {
                    return true;
                }
            }

            return false;
        }
    }
}