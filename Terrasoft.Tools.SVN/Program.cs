namespace Terrasoft.Tools.SVN
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class Program
    {
        private static readonly ConcurrentDictionary<string, string> _programOptions =
            new ConcurrentDictionary<string, string>();

        public static int Main(string[] args) {
            IEnumerable<string[]> argsEnumerable =
                args.Select(argument => argument.Split('=')).Where(keyvalue => keyvalue.Length == 2);
            Parallel.ForEach(argsEnumerable, FillParamDelegate);
            switch (_programOptions["operation"].ToLowerInvariant()) {
                case "createfeature":
                    using (var svnUtils = new SvnUtils(_programOptions)) {
                        return Convert.ToInt32(svnUtils.CreateFeature());
                    }
                case "updatefeaturefromrelease":
                    using (var svnUtils = new SvnUtils(_programOptions)) {
                        if (svnUtils.UpdateFromReleaseBranch() && Convert.ToBoolean(svnUtils.CommitIfNoError)) {
                            return Convert.ToInt32(svnUtils.CommitChanges(true));
                        }
                    }

                    break;
                case "finishfeature":
                    using (var svnUtils = new SvnUtils(_programOptions)) {
                        svnUtils.ReintegrationMergeToBaseBranch();
                    }

                    break;
                case "closefeature":
                    using (var svnUtils = new SvnUtils(_programOptions)) {
                        svnUtils.DeleteClosedFeature();
                    }
                    break;
                default:
                    Usage();
                    break;
            }

            return 0;
        }

        private static void Usage() {
            Console.WriteLine("Usage:");
        }

        private static void FillParamDelegate(string[] keyValueArray, ParallelLoopState arg2, long arg3) {
            string key = keyValueArray[0].Substring(1, keyValueArray[0].Length - 1);
            int copyLength = keyValueArray.Length - 1 < 0 ? 1 : keyValueArray.Length - 1;
            string value = string.Join("=", keyValueArray, 1, copyLength);
            FillParam(key.ToLowerInvariant(), value);
        }

        private static void FillParam(string key, string value) {
            if (_programOptions.ContainsKey(key)) {
                _programOptions[key] = value;
            } else {
                _programOptions.TryAdd(key, value);
            }
        }
    }
}
