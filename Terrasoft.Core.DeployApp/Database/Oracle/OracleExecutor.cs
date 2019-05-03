namespace Terrasoft.Core.DeployApp.Database.Oracle
{
    public class OracleExecutor : IDbExecutor
    {
        public OracleExecutor(string serverName, string login, string password) {
            ServerName = serverName;
            Login = login;
            Password = password;
        }

        public string ServerName { get; }
        public string Login { get; }
        public string Password { get; }

        public void RestoreDb(string databaseName, string backupPath) {
            //todo
        }
    }
}