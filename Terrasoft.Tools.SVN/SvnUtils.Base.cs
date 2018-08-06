namespace Terrasoft.Tools.SVN
{
    using System.Collections.Generic;
    using System.Net;
    using SharpSvn;

    /// <inheritdoc />
    /// <summary>
    ///     Абстрактный класс SVN клиента
    /// </summary>
    public abstract class SvnUtilsBase : SvnClient
    {
        private string UserName { get; set; }
        private string Password { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Конструктор SVN клиента
        /// </summary>
        /// <param name="options">Коллекция параметров</param>
        protected SvnUtilsBase(IReadOnlyDictionary<string, string> options) {
            foreach (KeyValuePair<string, string> option in options) {
                switch (option.Key) {
                    case "svnuser":
                        UserName = options["svnuser"];
                        break;
                    case "workingcopypath":
                        WorkingCopyPath = options["workingcopypath"];
                        break;
                    case "branchreleaseurl":
                        BranchReleaseUrl = options["branchreleaseurl"];
                        break;
                    case "featurename":
                        FeatureName = options["featurename"];
                        break;
                    case "branchfeatureurl":
                        BranchFeatureUrl = options["branchfeatureurl"];
                        break;
                    case "maintainer":
                        Maintainer = options["maintainer"];
                        break;
                    case "commitifnoerror":
                        CommitIfNoError = options["commitifnoerror"];
                        break;
                    case "svnpassword":
                        Password = options["svnpassword"];
                        break;
                    default:
                        continue;
                }
            }

            Authentication.Clear();
            Authentication.DefaultCredentials = new NetworkCredential(UserName, Password);
        }

        /// <summary>
        ///     Название фитчи
        /// </summary>
        protected string FeatureName { get; set; }

        /// <summary>
        ///     URL Ветки с фитчами
        /// </summary>
        protected string BranchFeatureUrl { get; set; }

        /// <summary>
        ///     Издатель
        /// </summary>
        protected string Maintainer { get; set; }

        /// <summary>
        ///     Базовая ветка, из которой выделяется фитча
        /// </summary>
        protected string BranchReleaseUrl { get; set; }

        /// <summary>
        ///     Путь к рабочей копии
        /// </summary>
        protected string WorkingCopyPath { get; set; }

        /// <summary>
        ///     Зафиксировать изменения в хранилище в случае отсутствия ошибок
        /// </summary>
        public string CommitIfNoError { get; set; }
    }
}