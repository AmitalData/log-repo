
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
    public class DeleteOldCommunicationLogsTask : TaskManagerBase
    {
        public DeleteOldCommunicationLogsTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            #region Communications
            int numberOfExecuteRows = 3000;
            int numberOfRecords = 0;
            while (numberOfExecuteRows == 3000 && numberOfRecords < 3000000)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldCommunicationLogs]", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
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

            #endregion

        }
    }
}