using System;

namespace Terrasoft.Core.DeployApp.Database
{
	public interface IDbExecutor {
		void RestoreDb(string databaseName, string backupPath);
	}
}
