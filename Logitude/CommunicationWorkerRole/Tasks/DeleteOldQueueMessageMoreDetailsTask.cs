

using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
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
            int numberOfExecuteRows = 1000;
            int numberOfRecords = 0;

            while (numberOfExecuteRows == 1000 && numberOfRecords < 500000)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                   
                        string sql = "delete top(1000) from QueueMessageMoreDetails where CreateDateTime < GETDATE() - 90";
                        if (dbms == "oracle")
                        {
                            sql = "DELETE FROM QueueMessageMoreDetails WHERE ROWID IN  (SELECT ROWID FROM QueueMessageMoreDetails where CreateDateTime < (SELECT SYSDATE FROM DUAL) - 90 and rownum<= 1000);";
                        }

                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
                        cn.Open();
                        numberOfExecuteRows = cmd.ExecuteNonQuery();
                        numberOfRecords += numberOfExecuteRows;
                        cn.Close();
                        scope.Complete();
                    }
                }
                Thread.Sleep(1000);

            }

        }
    }
}