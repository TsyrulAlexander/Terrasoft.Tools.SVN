using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using SharpSvn;
using SharpSvn.Security;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

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
        protected SvnUtilsBase(IReadOnlyDictionary<string, string> options)
        {
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
        internal bool CommitIfNoError { get; set; }

        private void InitializeOptions(IReadOnlyDictionary<string, string> options)
        {
            foreach (KeyValuePair<string, string> option in options) {
                foreach (PropertyInfo propertyInfo in typeof(SvnUtilsBase).GetProperties(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                )) {
                    foreach (OptionName optionName in propertyInfo.GetCustomAttributes<OptionName>()) {
                        if (optionName.Name.Equals(option.Key, StringComparison.OrdinalIgnoreCase)) {
                            propertyInfo.SetValue(this, option.Value);
                        }
                    }
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class OptionName : Attribute
    {
        public OptionName(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}