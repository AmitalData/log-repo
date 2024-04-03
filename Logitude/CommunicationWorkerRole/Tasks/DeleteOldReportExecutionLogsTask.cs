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
    public class DeleteOldReportExecutionLogsTask : TaskManagerBase
    {
        public DeleteOldReportExecutionLogsTask(string Id, int tenant) : base(Id, tenant)
        {
        }

        public override void StartTask()
        {

            string dataBaseConnection = TenantServerConfigration.GetDbConnection(0);
            using (SqlConnection sqlConnection = new SqlConnection(dataBaseConnection))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldReportExecutionLogs]", sqlConnection)
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
