
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteOldAPILogsTask : TaskManagerBase
    {
        public DeleteOldAPILogsTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            #region APILogsData
            int numberOfExecuteRows = 1000;
            while (numberOfExecuteRows == 1000)
            {

                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        string sql = "delete top(1000) from APILogsData where id in (select id from APILogs where CreateDate < GETDATE() - 90 )";
                    
                        if (dbms == "oracle")
                        {
                            sql = "DELETE FROM APILogsData WHERE ROWID IN  (SELECT ROWID FROM APILogsData where Id in (select Id from APILogs where CreateDate < (SELECT SYSDATE FROM DUAL) - 90 FETCH FIRST 1000 ROWS ONLY));";
                        }

                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
                        cn.Open();
                        numberOfExecuteRows = cmd.ExecuteNonQuery();
                        cn.Close();
                        scope.Complete();
                    }
                }

            }
            #endregion


            #region APILogs
            numberOfExecuteRows = 1000;
            while (numberOfExecuteRows == 1000)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        string sql = "delete top(1000) from APILogs where CreateDate < GETDATE() - 90";
                        if (dbms == "oracle")
                        {
                            sql = "DELETE FROM APILogs WHERE ROWID IN  (SELECT ROWID FROM APILogs where CreateDate < (SELECT SYSDATE FROM DUAL) - 90 FETCH FIRST 1000 ROWS ONLY)";
                        }

                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
                        cn.Open();
                        numberOfExecuteRows = cmd.ExecuteNonQuery();
                        cn.Close();
                        scope.Complete();
                    }
                }
            }

            #endregion

        }
    }
}