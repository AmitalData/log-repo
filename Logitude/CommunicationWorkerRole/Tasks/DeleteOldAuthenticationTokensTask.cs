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
    public class DeleteOldAuthenticationTokensTask : TaskManagerBase
    {
        public DeleteOldAuthenticationTokensTask(string Id, int tenant)
            : base(Id,tenant)
        {

        }
        public override void StartTask()
        {
            try
            {
                string strConnString = TenantServerConfigration.GetDbConnection(0);
                //DbConnection connection = DatabaseInitializer.GetConnection(strConnString);
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldAuthenticationTokens]", cn);
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

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "DeleteOldAuthenticationTokensTask", "", null);
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
