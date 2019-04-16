using System.Data.OracleClient;

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
			var connectionBuilder = GetConnectionString(databaseName);
			using (var connection = new OracleConnection("Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = partner-ora)(PORT = 1521)) (CONNECT_DATA = (SERVER = SHARED) (SERVICE_NAME = partnerora.tscrm.com) ) );User Id= MTS_A_KALUS;Password= MTS_A_KALUS;")) {
				connection.Open();
				LockUser(connection, databaseName);
				KillSession(connection, databaseName);
				DropUser(connection, databaseName);
				DropTableSpace(connection, databaseName);
				//using (var command = new OracleCommand("UPDATE \"Contact\" SET \"Name\" = 'azazaz' WHERE \"Id\" = '{DD18D542-91ED-43E0-8BE5-06DB9BD9520A}'", connection)) {
				//	command.ExecuteNonQuery();
				//}
			}
		}

		protected virtual void LockUser(OracleConnection connection, string userName) {
			using (var command = new OracleCommand($"ALTER USER {userName} ACCOUNT LOCK;", connection)) {
				command.ExecuteNonQuery();
			}
		}

		protected virtual void KillSession(OracleConnection connection, string userName) {
			var commandString = $@"BEGIN
			    FOR i IN (
			        SELECT
			            s.sid,
			            s.serial#
			        FROM
			            gv$session s
			            JOIN gv$process p ON p.addr = s.paddr
			                                 AND p.inst_id = s.inst_id
			        WHERE
			            s.type != 'BACKGROUND'
			            AND s.username = '{userName}'
			    ) LOOP
			        EXECUTE IMMEDIATE 'alter system kill session '
			                          || ''''
			                          || i.sid
			                          || ','
			                          || i.serial#
			                          || ''' immediate';
			    END LOOP;
			END;";
			using (var command = new OracleCommand(commandString, connection)) {
				command.ExecuteNonQuery();
			}
		}

		protected virtual void DropUser(OracleConnection connection, string userName) {
			var commandString = $"DROP USER \"{userName}\" CASCADE;";
			using (var command = new OracleCommand(commandString, connection)) {
				command.ExecuteNonQuery();
			}
		}

		protected virtual void DropTableSpace(OracleConnection connection, string userName) {
			var commandString = $"DROP TABLESPACE \"{userName}\" INCLUDING CONTENTS AND DATAFILES CASCADE CONSTRAINTS;";
			using (var command = new OracleCommand(commandString, connection)) {
				command.ExecuteNonQuery();
			}
		}

		private OracleConnectionStringBuilder GetConnectionString(string dbName) {
			return new OracleConnectionStringBuilder {
				DataSource = ServerName,
				UserID = Login,
				Password = Password
			};
		}
	}
}
