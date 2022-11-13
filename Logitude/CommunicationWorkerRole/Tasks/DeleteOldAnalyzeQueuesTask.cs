using CommunicationWorkerRole.Services.Scheduler;
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
    public class DeleteOldAnalyzeQueuesTask : TaskManagerBase
    {
        public DeleteOldAnalyzeQueuesTask(string Id, int tenant) : base(Id, tenant)
        {
        }

        public override void StartTask()
        {

            string dataBaseConnection = ConnectionStringConfiguration.GetConnection("Globalstr");
            using (SqlConnection sqlConnection = new SqlConnection(dataBaseConnection))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldAnalyzeQueues]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut()
                };

                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        
    }
}
