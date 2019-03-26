namespace Terrasoft.Core.DeployApp.Database.MsSql
{
	//public class MsSqlExecutor : IDbExecutor {
	//	public string DbName { get; set; }
	//	public string ServerName { get; }
	//	public string Login { get; }
	//	public string Password { get; }

	//	public MsSqlExecutor(string dbName, string serverName, string login, string password) {
	//		DbName = dbName;
	//		ServerName = serverName;
	//		Login = login;
	//		Password = password;
	//	}

	//	public void RestoreDb(string backupPath, Action callback) {
	//		var server = GetServer();
	//		var restore = GetRestore(backupPath, callback);
	//		restore.SqlRestore(server);
	//	}

	//	protected virtual Restore GetRestore(string backupPath, Action callback) {
	//		var restore = new Restore {
	//			Database = DbName, Action = RestoreActionType.Database, ReplaceDatabase = true, NoRecovery = false
	//		};
	//		restore.Devices.AddDevice(backupPath, DeviceType.File);
	//		void OnComplite(object sender, ServerMessageEventArgs e) {
	//			restore.Complete -= OnComplite;
	//			callback?.Invoke();
	//		}
	//		restore.Complete += OnComplite;
	//		return restore;
	//	}

	//	protected virtual ServerConnection GetConnection() {
	//		return new ServerConnection(ServerName, Login, Password);
	//	}

	//	protected virtual Server GetServer() {
	//		return new Server(GetConnection());
	//	}
	//}
}
