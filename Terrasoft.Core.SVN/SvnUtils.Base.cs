using System;
using System.Collections.Generic;
using System.Net;
using SharpSvn;
using SharpSvn.Security;

namespace Terrasoft.Core.SVN
{
    /// <inheritdoc />
    /// <summary>
    ///     Абстрактный класс SVN клиента
    /// </summary>
    public abstract class SvnUtilsBase : SvnClient
    {
	    public ILogger Logger { get; set; }

		public const string SvnUserOptionName = "svnuser";
		public const string WorkingCopyPathOptionName = "workingcopypath";
		public const string BaseWorkingCopyPathOptionName = "baseworkingcopypath";
		public const string BranchReleaseUrlOptionName = "branchreleaseurl";
		public const string FeatureNameOptionName = "featurename";
		public const string BranchFeatureUrlOptionName = "branchfeatureurl";
		public const string MaintainerOptionName = "maintainer";
		public const string CommitIfNoErrorOptionName = "commitifnoerror";
		public const string SvnPasswordOptionName = "svnpassword";
		public const string AutoMergeOptionName = "automerge";

		/// <inheritdoc />
		/// <summary>
		///     Конструктор SVN клиента
		/// </summary>
		/// <param name="options">Коллекция параметров</param>
		/// <param name="logger">Логгер</param>
		protected SvnUtilsBase(IReadOnlyDictionary<string, string> options, ILogger logger) {
		    Logger = logger;
		    foreach (KeyValuePair<string, string> option in options) {
                switch (option.Key) {
                    case SvnUserOptionName:
                        UserName = options[SvnUserOptionName];
                        break;
                    case WorkingCopyPathOptionName:
                        WorkingCopyPath = options[WorkingCopyPathOptionName];
                        break;
                    case BaseWorkingCopyPathOptionName:
                        BaseWorkingCopyPath = options[BaseWorkingCopyPathOptionName];
                        break;
                    case BranchReleaseUrlOptionName:
                        BranchReleaseUrl = options[BranchReleaseUrlOptionName];
                        break;
                    case FeatureNameOptionName:
                        FeatureName = options[FeatureNameOptionName];
                        break;
                    case BranchFeatureUrlOptionName:
                        BranchFeatureUrl = options[BranchFeatureUrlOptionName];
                        break;
                    case MaintainerOptionName:
                        Maintainer = options[MaintainerOptionName];
                        break;
                    case CommitIfNoErrorOptionName:
                        CommitIfNoError = Convert.ToBoolean(options[CommitIfNoErrorOptionName]);
                        break;
                    case SvnPasswordOptionName:
                        Password = options[SvnPasswordOptionName];
                        break;
                    case AutoMergeOptionName:
                        AutoMerge = Convert.ToBoolean(options[AutoMergeOptionName]);
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

        protected bool AutoMerge { get; }

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
        public bool CommitIfNoError { get; set; }
    }
}