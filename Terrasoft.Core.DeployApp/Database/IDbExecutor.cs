using System;

namespace Terrasoft.Core.DeployApp.Database
{
	public interface IDbExecutor {
		string DbName { get; }
		void RestoreDb(string backupPath, Action callback);
	}
}
