namespace Terrasoft.Tools.SVN
{
    using System.Collections.Generic;
    using System.Net;
    using SharpSvn;

    /// <inheritdoc />
    /// <summary>
    /// Абстрактный класс SVN клиента
    /// </summary>
    public abstract class SvnUtilsBase : SvnClient
    {
        /// <summary>
        /// Название фитчи
        /// </summary>
        protected string FeatureName { get; private set; }

        /// <summary>
        /// URL Ветки с фитчами
        /// </summary>
        protected string BranchFeatureUrl { get; private set; }

        /// <summary>
        /// Издатель
        /// </summary>
        protected string Maintainer { get; private set; }

        /// <summary>
        /// Базовая ветка, из которой выделяется фитча
        /// </summary>
        protected string BranchReleaseUrl { get; private set; }

        /// <summary>
        /// Путь к рабочей копии
        /// </summary>
        protected string WorkingCopyPath { get; private set; }
        
        /// <summary>
        /// Зафиксировать изменения в хранилище в случае отсутствия ошибок
        /// </summary>
        public string CommitIfNoError { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Конструктор SVN клиента
        /// </summary>
        /// <param name="options">Коллекция параметров</param>
        protected SvnUtilsBase(IReadOnlyDictionary<string, string> options) {
            Authentication.Clear();
            Authentication.DefaultCredentials = new NetworkCredential(options["svnuser"], options["svnpassword"]);
            WorkingCopyPath = options["workingcopy"];
            BranchReleaseUrl = options["branchreleaseurl"];
            FeatureName = options["featurename"];
            BranchFeatureUrl = options["branchfeatureurl"];
            Maintainer = options["maintainer"];
            CommitIfNoError = options["commitifnoerror"];
        }
    }
}
