namespace Terrasoft.Tools.SVN
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Win32;
    using Terrasoft.Tools.SVN.Properties;

    internal static class Program
    {
        private const string TerrasoftToolsSvnExe = @"	Terrasoft.Tools.SVN.exe";
        private const string OperationCreatefeature = " -Operation=CreateFeature";
        private const string OperationUpdateFeature = " -Operation=UpdateFeature";
        private const string OperationFinishFeature = " -Operation=FinishFeature";
        private const string OperationCloseFeature = " -Operation=CloseFeature";
        private const string SvnuserSvnuser = " -SvnUser=SvnUser";
        private const string SvnpasswordSnvpassword = " -SvnPassword=SnvPassword";

        private const string WorkingcopypathCSvnProjectTerrasoftFeature1 =
            @" -WorkingCopyPath=C:\SVN\Project\Terrasoft_Feature1";

        private const string BranchfeatureurlHttpSvnServerSvnProjectBranches =
            " -BranchFeatureUrl=http://Svn-Server/svn/Project/branches";

        private const string BranchreleaseurlHttpSvnServerSvnProjectTrunkPackagestore =
            " -BranchReleaseUrl=http://Svn-Server/svn/Project/trunk/PackageStore";

        private const string FeaturenameFeature1 = " -FeatureName=Feature1";
        private const string CommitifnoerrorTrue = " -CommitIfNoError=true";
        private const string Maintainer = " -Maintainer=Partner1";

        private static readonly ConcurrentDictionary<string, string> _programOptions =
            new ConcurrentDictionary<string, string>();

        public static int Main(string[] args) {
            IEnumerable<string[]> argsEnumerable =
                args.Select(argument => argument.Split('=')).Where(keyvalue => keyvalue.Length == 2);
            Parallel.ForEach(argsEnumerable, FillParamDelegate);
            if (!_programOptions.ContainsKey(@"operation")) {
                Usage();
                return 0;
            }

            string programOption = _programOptions[@"operation"].ToLowerInvariant();
            switch (programOption) {
                case "createfeature":
                    using (var svnUtils = new SvnUtils(_programOptions)) {
                        return Convert.ToInt32(svnUtils.CreateFeature());
                    }
                case "updatefeature":
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
            string language = Registry.GetValue(@"HKEY_CURRENT_USER\", @"Software\Terrasoft\Tool\Svn", @"Rus")
                .ToString();
            if (language == @"Rus") {
                Console.WriteLine(Resources.Program_Usage);
                string sample1 = GenerateSample1();
                Console.WriteLine(sample1);
                Console.WriteLine();
                string sample2 = GenerateSample2();
                Console.WriteLine();
                Console.WriteLine(sample2);

                string sample3 = GenerateSample3();
                Console.WriteLine();
                Console.WriteLine(sample3);

                string sample4 = GenerateSample4();
                Console.WriteLine();
                Console.WriteLine(sample4);
            }
        }

        private static string GenerateSample4() {
            var sample4 = new StringBuilder();
            sample4.Append(TerrasoftToolsSvnExe);
            sample4.Append(OperationCloseFeature);
            sample4.Append(SvnuserSvnuser);
            sample4.Append(SvnpasswordSnvpassword);
            sample4.Append(WorkingcopypathCSvnProjectTerrasoftFeature1);
            return sample4.ToString();
        }

        private static string GenerateSample3() {
            var sample3 = new StringBuilder();
            sample3.Append(TerrasoftToolsSvnExe);
            sample3.Append(OperationFinishFeature);
            sample3.Append(SvnuserSvnuser);
            sample3.Append(SvnpasswordSnvpassword);
            sample3.Append(WorkingcopypathCSvnProjectTerrasoftFeature1);
            return sample3.ToString();
        }

        private static string GenerateSample2() {
            var sample2 = new StringBuilder();
            sample2.Append(TerrasoftToolsSvnExe);
            sample2.Append(OperationUpdateFeature);
            sample2.Append(SvnuserSvnuser);
            sample2.Append(SvnpasswordSnvpassword);
            sample2.Append(WorkingcopypathCSvnProjectTerrasoftFeature1);
            sample2.Append(CommitifnoerrorTrue);
            return sample2.ToString();
        }

        private static string GenerateSample1() {
            var sample1 = new StringBuilder();
            sample1.Append(TerrasoftToolsSvnExe);
            sample1.Append(OperationCreatefeature);
            sample1.Append(SvnuserSvnuser);
            sample1.Append(SvnpasswordSnvpassword);
            sample1.Append(WorkingcopypathCSvnProjectTerrasoftFeature1);
            sample1.Append(BranchfeatureurlHttpSvnServerSvnProjectBranches);
            sample1.Append(BranchreleaseurlHttpSvnServerSvnProjectTrunkPackagestore);
            sample1.Append(FeaturenameFeature1);
            sample1.Append(Maintainer);
            return sample1.ToString();
        }

        private static void FillParamDelegate(string[] keyValueArray, ParallelLoopState arg2, long arg3) {
            string key = keyValueArray[0].Substring(1, keyValueArray[0].Length - 1);
            int copyLength = keyValueArray.Length - 1 < 0 ? 1 : keyValueArray.Length - 1;
            string value = string.Join(@"=", keyValueArray, 1, copyLength);
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
