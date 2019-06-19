using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using SharpSvn;
using SharpSvn.Security;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Terrasoft.Core.SVN
{
    /// <inheritdoc />
    /// <summary>
    ///     Абстрактный класс SVN клиента
    /// </summary>
    public abstract class SvnUtilsBase : SvnClient
    {
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
        /// <param name="logger"></param>
        protected SvnUtilsBase(IReadOnlyDictionary<string, string> options, ILogger logger) {
            Logger = logger;
            InitializeOptions(options);

            Authentication.Clear();
            Authentication.DefaultCredentials = new NetworkCredential(UserName, Password);
#pragma warning disable CA5359 // Do Not Disable Certificate Validation
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
#pragma warning restore CA5359 // Do Not Disable Certificate Validation
            Authentication.SslServerTrustHandlers += delegate(object o, SvnSslServerTrustEventArgs args) {
                args.AcceptedFailures = args.Failures;
                args.Save = true;
            };
        }

        public ILogger Logger { get; set; }

        [OptionName("SvnUser")] private string UserName { get; set; }

        [OptionName("SvnPassword")] private string Password { get; set; }

        /// <summary>
        ///     Название фитчи
        /// </summary>
        [OptionName("FeatureName")]
        protected string FeatureName { get; set; }

        /// <summary>
        ///     URL Ветки с фитчами
        /// </summary>
        [OptionName("BranchFeatureUrl")]
        protected string BranchFeatureUrl { get; set; }

        /// <summary>
        ///     Издатель
        /// </summary>
        [OptionName("Maintainer")]
        protected string Maintainer { get; set; }

        /// <summary>
        ///     Базовая ветка, из которой выделяется фитча
        /// </summary>
        [OptionName("BranchReleaseUrl")]
        protected string BranchReleaseUrl { get; set; }

        /// <summary>
        ///     Путь к рабочей копии
        /// </summary>
        [OptionName("WorkingCopyPath")]
        protected string WorkingCopyPath { get; set; }

        /// <summary>
        ///     Путь к базовой рабочей копии (родитель)
        /// </summary>
        [OptionName("BaseWorkingCopyPath")]
        protected string BaseWorkingCopyPath { get; set; }

        /// <summary>
        ///     Зафиксировать изменения в хранилище в случае отсутствия ошибок
        /// </summary>
        [OptionName("CommitIfNoError")]
        public bool CommitIfNoError { get; set; }

        private void InitializeOptions(IReadOnlyDictionary<string, string> options) {
            foreach (KeyValuePair<string, string> option in options) {
                foreach (PropertyInfo propertyInfo in typeof(SvnUtilsBase).GetProperties(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                )) {
                    foreach (OptionName optionName in propertyInfo.GetCustomAttributes<OptionName>()) {
                        if (optionName.Name.Equals(option.Key, StringComparison.OrdinalIgnoreCase)) {
                            if (propertyInfo.PropertyType == typeof(bool)) {
                                propertyInfo.SetValue(this, Convert.ToBoolean(option.Value));
                            } else {
                                propertyInfo.SetValue(this, option.Value);
                            }
                        }
                    }
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class OptionName : Attribute
    {
        public OptionName(string name) {
            Name = name;
        }

        public string Name { get; }
    }
}