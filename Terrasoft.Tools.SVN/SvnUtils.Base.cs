using System;
using System.Collections.Generic;
using System.Net;
using SharpSvn;
using SharpSvn.Security;

namespace Terrasoft.Tools.Svn
{
    /// <inheritdoc />
    /// <summary>
    ///     Абстрактный класс SVN клиента
    /// </summary>
    internal abstract class SvnUtilsBase : SvnClient
    {
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
                    case "baseworkingcopypath":
                        BaseWorkingCopyPath = options["baseworkingcopypath"];
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
                        CommitIfNoError = Convert.ToBoolean(options["commitifnoerror"]);
                        break;
                    case "svnpassword":
                        Password = options["svnpassword"];
                        break;
                    case "automerge":
                        break;
                    default:
                        continue;
                }
            }

            Authentication.Clear();
            Authentication.DefaultCredentials = new NetworkCredential(UserName, Password);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Authentication.SslServerTrustHandlers += delegate(object o, SvnSslServerTrustEventArgs args) {
                args.AcceptedFailures = args.Failures;
                args.Save = true;
            };
        }

        private string UserName { get; }
        private string Password { get; }

        /// <summary>
        ///     Название фитчи
        /// </summary>
        protected string FeatureName { get; }

        /// <summary>
        ///     URL Ветки с фитчами
        /// </summary>
        protected string BranchFeatureUrl { get; }

        /// <summary>
        ///     Издатель
        /// </summary>
        protected string Maintainer { get; }

        /// <summary>
        ///     Базовая ветка, из которой выделяется фитча
        /// </summary>
        protected string BranchReleaseUrl { get; }

        /// <summary>
        ///     Путь к рабочей копии
        /// </summary>
        protected string WorkingCopyPath { get; }

        /// <summary>
        ///     Путь к базовой рабочей копии (родитель)
        /// </summary>
        protected string BaseWorkingCopyPath { get; }

        /// <summary>
        ///     Зафиксировать изменения в хранилище в случае отсутствия ошибок
        /// </summary>
        internal bool CommitIfNoError { get; }
    }
}