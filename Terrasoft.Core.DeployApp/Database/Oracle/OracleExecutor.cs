namespace Terrasoft.Core.DeployApp.Database.Oracle {
	public class OracleExecutor : IDbExecutor {
		public string ServerName { get; }
		public string Login { get; }
		public string Password { get; }

		public OracleExecutor(string serverName, string login, string password) {
			ServerName = serverName;
			Login = login;
			Password = password;
		}
		public void RestoreDb(string databaseName, string backupPath) {
			//var connectionBuilder = GetConnectionString(databaseName);
			//using (OracleConnection connection = new OracleConnection(connectionBuilder.ConnectionString)) {
			//	connection.Open();
			//}
		}

		//private OracleConnectionStringBuilder GetConnectionString(string dbName) {
		//	return new OracleConnectionStringBuilder {
		//		DataSource = ServerName,
		//		UserID = Login,
		//		Password = Password
		//	};
		//}
	}
}
