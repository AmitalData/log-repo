using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteOldContactActivityLogsTask : TaskManagerBase
    {
        public DeleteOldContactActivityLogsTask(string Id, int tenant):base(Id,tenant)
        {
        }

        public override void StartTask()
        {

            string dataBaseConnection = GetSystemLogsConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(dataBaseConnection))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldContactActivityLogs]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut()
                };

                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        private string GetSystemLogsConnectionString()
        {
            string dbConnectionTo = ConfigurationManager.ConnectionStrings["SystemLogsStr"].ConnectionString;
            string destinationConnectionString = BuildConnectionString(GetConnectionStringArguments(dbConnectionTo));
            return destinationConnectionString;
        }

        private string BuildConnectionString(ConnectionStringArguments connectionStringArguments)
        {
            string result = "Data Source=" + connectionStringArguments.Server + ";Initial Catalog=" + connectionStringArguments.Catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + connectionStringArguments.UserName + ";Password= " + connectionStringArguments.Password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        private ConnectionStringArguments GetConnectionStringArguments(string dbConnectionTo)
        {
            string[] destinationConnectionArray = dbConnectionTo.Split(',');
            ConnectionStringArguments connectionStringArguments = new ConnectionStringArguments()
            {
                Catalog = destinationConnectionArray[0],
                UserName = destinationConnectionArray[1],
                Password = destinationConnectionArray[2],
                Server = destinationConnectionArray[3],
            };

            return connectionStringArguments;
        }
    }
}
