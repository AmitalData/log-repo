

using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteOldQueueMessageMoreDetailsTask : TaskManagerBase
    {
        public DeleteOldQueueMessageMoreDetailsTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            string strConnString = TenantServerConfigration.GetDbConnection(0);

            int numberOfExecuteRow = 1000;

            while (numberOfExecuteRow == 1000)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldQueueMessageMoreDetailsTask]", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 1000000;
                        cn.Open();
                        numberOfExecuteRow = cmd.ExecuteNonQuery();
                        cn.Close();
                        scope.Complete();
                    }
                }
            }

        }
    }
}