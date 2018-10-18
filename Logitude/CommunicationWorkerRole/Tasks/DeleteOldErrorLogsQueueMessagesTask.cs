using Logitude.SystemLogs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteOldErrorLogsQueueMessagesTask : TaskManagerBase
    {
        public DeleteOldErrorLogsQueueMessagesTask(string Id, int tenant)
            : base(Id,tenant)
        {

        }
        public override void StartTask()
        {
            try
            {
                string strConnString = TenantServerConfigration.GetDbConnection(0);
                if (LogitudeSettings.DeploymentStage == "Simplog")
                {
                    strConnString = "LogitudeSystemLogs,logitudemanager@ebup282itq,!LO852456,ebup282itq.database.windows.net";
                }
                else if (LogitudeSettings.DeploymentStage == "logitudepreprod")
                {
                    strConnString = "LogitudeSystemLogs-PreR2,logitudemanager@logitudetest,!LO009008,logitudetest.database.windows.net"; 
                }
                else if (LogitudeSettings.DeploymentStage == "amitalstorage")
                {
                    strConnString = "Logs,sa,Saas256,amitaldata.cloudapp.net";
                }
                else if (LogitudeSettings.DeploymentStage == "logboxwe1")
                {
                    strConnString = "logbox-logs,logboxadmin,London2015!London2015!,logboxdbs.database.windows.net";
                }
                else
                {
                    strConnString = "LogitudeSystemLogs-Test2,sa,Saas256,logitudetest.cloudapp.net";
                }
                DbConnection connection = DatabaseInitializer.GetConnection(strConnString);
                using (SqlConnection cn = new SqlConnection(connection.ConnectionString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldErrorLog]", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 1000000;
                        cn.Open();
                        var output = cmd.ExecuteNonQuery();
                        cn.Close();
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "DeleteOldErrorLogsQueueMessagesTask", "", null);
            }
          
            //QueueMessageRepository Repo = new QueueMessageRepository(0);
            //var Queues = Repo.GetQueueMessages().Where(a=>a.Status == 1 || a.Status == -1);
            //foreach (var item in Queues)
            //{
            //    Repo.Remove(item); 
            //}
            //Repo.SubmitChanges();
        }
    }
}
