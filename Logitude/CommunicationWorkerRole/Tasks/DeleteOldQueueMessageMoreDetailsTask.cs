

using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
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
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            int numberOfExecuteRow = 1000;

            while (numberOfExecuteRow == 1000)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                   
                        string sql = "delete top(1000) from QueueMessageMoreDetails where CreateDateTime < GETDATE() - 90";
                        if (dbms == "oracle")
                        {
                            sql = "DELETE FROM QueueMessageMoreDetails WHERE ROWID IN  (SELECT ROWID FROM QueueMessageMoreDetails where CreateDateTime < (SELECT SYSDATE FROM DUAL) - 90 FETCH FIRST 1000 ROWS ONLY);";
                        }

                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
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