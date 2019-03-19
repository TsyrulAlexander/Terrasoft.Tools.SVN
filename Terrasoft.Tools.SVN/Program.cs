using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Tools.Svn.Properties;

namespace Terrasoft.Tools.Svn
{
    public static class Program
    {
        private const string TerrasoftToolsSvnExe = @"	Terrasoft.Tools.SVN.exe";
        private const string OperationCreateFeature = " -Operation=CreateFeature";
        private const string OperationUpdateFeature = " -Operation=UpdateFeature";
        private const string OperationFinishFeature = " -Operation=FinishFeature";
        private const string OperationCloseFeature = " -Operation=CloseFeature";
        private const string SvnUserSvnUser = " -SvnUser=SvnUser";
        private const string SvnPasswordSvnPassword = " -SvnPassword=SvnPassword";

        private const string WorkingCopyPathCSvnProjectTerrasoftFeature1 =
            @" -WorkingCopyPath=C:\SVN\Project\Terrasoft_Feature1";

        private const string BranchFeatureUrlHttpSvnServerSvnProjectBranches =
            " -BranchFeatureUrl=http://Svn-Server/svn/Project/branches";

        private const string BranchReleaseUrlHttpSvnServerSvnProjectTrunkPackageStore =
            " -BranchReleaseUrl=http://Svn-Server/svn/Project/trunk/PackageStore";

        private const string FeatureNameFeature1 = " -FeatureName=Feature1";
        private const string CommitIfNoErrorTrue = " -CommitIfNoError=true";
        private const string Maintainer = " -Maintainer=Partner1";

        private static readonly ConcurrentDictionary<string, string> ProgramOptions =
            new ConcurrentDictionary<string, string>();

        public static int Main(string[] args) {
            Resources.Culture = CultureInfo.CurrentCulture;
            IEnumerable<string[]> argsEnumerable =
                args.Select(argument => argument.Split('=')).Where(keyValue => keyValue.Length == 2);
            Parallel.ForEach(argsEnumerable, FillParamDelegate);
            if (!ProgramOptions.ContainsKey(@"operation")) {
                Usage();
                return 0;
            }

            string programOption = ProgramOptions[@"operation"].ToLowerInvariant();
            switch (programOption) {
                // ReSharper disable once StringLiteralTypo
                case "createfeature":
                    using (var svnUtils = new SvnUtils(ProgramOptions)) {
                        return Convert.ToInt32(svnUtils.CreateFeature());
                    }
                // ReSharper disable once StringLiteralTypo
                case "updatefeature":
                    using (var svnUtils = new SvnUtils(ProgramOptions)) {
                        if (svnUtils.UpdateFromReleaseBranch() && svnUtils.CommitIfNoError) {
                            return Convert.ToInt32(svnUtils.CommitChanges(true));
                        }
                    }

                    break;
                // ReSharper disable once StringLiteralTypo
                case "finishfeature":
                    using (var svnUtils = new SvnUtils(ProgramOptions)) {
                        svnUtils.ReintegrationMergeToBaseBranch();
                    }

                    break;
                // ReSharper disable once StringLiteralTypo
                case "closefeature":
                    using (var svnUtils = new SvnUtils(ProgramOptions)) {
                        svnUtils.DeleteClosedFeature();
                    }

                    break;
                // ReSharper disable once StringLiteralTypo
                case "fixfeature":
                    using (var svnUtils = new SvnUtils(ProgramOptions)) {
                        return Convert.ToInt32(
                            svnUtils.FixBranch()
                        );
                    }

                default:
                    Usage();
                    break;
            }

            return 0;
        }

        private static void Usage() {
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

        private static string GenerateSample4() {
            var sample4 = new StringBuilder();
            sample4.Append(TerrasoftToolsSvnExe);
            sample4.Append(OperationCloseFeature);
            sample4.Append(SvnUserSvnUser);
            sample4.Append(SvnPasswordSvnPassword);
            sample4.Append(WorkingCopyPathCSvnProjectTerrasoftFeature1);
            return sample4.ToString();
        }

        private static string GenerateSample3() {
            var sample3 = new StringBuilder();
            sample3.Append(TerrasoftToolsSvnExe);
            sample3.Append(OperationFinishFeature);
            sample3.Append(SvnUserSvnUser);
            sample3.Append(SvnPasswordSvnPassword);
            sample3.Append(WorkingCopyPathCSvnProjectTerrasoftFeature1);
            return sample3.ToString();
        }

        private static string GenerateSample2() {
            var sample2 = new StringBuilder();
            sample2.Append(TerrasoftToolsSvnExe);
            sample2.Append(OperationUpdateFeature);
            sample2.Append(SvnUserSvnUser);
            sample2.Append(SvnPasswordSvnPassword);
            sample2.Append(WorkingCopyPathCSvnProjectTerrasoftFeature1);
            sample2.Append(CommitIfNoErrorTrue);
            return sample2.ToString();
        }

        private static string GenerateSample1() {
            var sample1 = new StringBuilder();
            sample1.Append(TerrasoftToolsSvnExe);
            sample1.Append(OperationCreateFeature);
            sample1.Append(SvnUserSvnUser);
            sample1.Append(SvnPasswordSvnPassword);
            sample1.Append(WorkingCopyPathCSvnProjectTerrasoftFeature1);
            sample1.Append(BranchFeatureUrlHttpSvnServerSvnProjectBranches);
            sample1.Append(BranchReleaseUrlHttpSvnServerSvnProjectTrunkPackageStore);
            sample1.Append(FeatureNameFeature1);
            sample1.Append(Maintainer);
            return sample1.ToString();
        }

        private static void FillParamDelegate(string[] keyValueArray, ParallelLoopState arg2, long arg3) {
            string key = keyValueArray[0].Substring(1, keyValueArray[0].Length - 1);
            int copyLength = keyValueArray.Length - 1 < 0
                ? 1
                : keyValueArray.Length - 1;
            string value = string.Join("=", keyValueArray, 1, copyLength);
            FillParam(key.ToLowerInvariant(), value);
        }

        private static void FillParam(string key, string value) {
            if (ProgramOptions.ContainsKey(key)) {
                ProgramOptions[key] = value;
            } else {
                ProgramOptions.TryAdd(key, value);
            }
        }
    }
}