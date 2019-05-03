using System.Data.SqlClient;

namespace Terrasoft.Core.DeployApp.Database.MsSql
{
    public class MsSqlExecutor : IDbExecutor
    {
        private const string DefaultDbName = "master";

        public MsSqlExecutor(string serverName, string login, string password)
        {
            ServerName = serverName;
            Login = login;
            Password = password;
        }

        public string ServerName { get; }
        public string Login { get; }
        public string Password { get; }

        public void RestoreDb(string databaseName, string backupPath)
        {
            SqlConnectionStringBuilder connectionStringBuilder = GetConnectionString(DefaultDbName);
            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString)) {
                connection.Open();
                using (SqlCommand command = GetUseCommand(connection, DefaultDbName)) {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = GetRestoreCommand(connection, databaseName, backupPath)) {
                    command.ExecuteNonQuery();
                }
            }
        }

        private SqlConnectionStringBuilder GetConnectionString(string dbName)
        {
            return new SqlConnectionStringBuilder {
                DataSource = ServerName,
                InitialCatalog = dbName,
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                UserID = Login,
                Password = Password
            };
        }

        private SqlCommand GetUseCommand(SqlConnection connection, string dataBaseName)
        {
            return new SqlCommand($"USE [{dataBaseName}]", connection);
        }

        private SqlCommand GetRestoreCommand(SqlConnection connection, string dataBaseName, string backPath)
        {
            return new SqlCommand($"RESTORE DATABASE [{dataBaseName}] FROM DISK = '{backPath}' WITH  FILE = 1",
                connection
            );
        }
    }
}