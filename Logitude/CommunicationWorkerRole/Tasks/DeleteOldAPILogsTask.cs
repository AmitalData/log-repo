
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;

public class DeleteOldAPILogsTask : TaskManagerBase
{
    public DeleteOldAPILogsTask(string Id, int tenant)
        : base(Id, tenant)
    {

        }
        public override void StartTask()
        {
            string strConnString = TenantServerConfigration.GetDbConnection(0);


            #region APILogsData
            int numberOfExecuteRows = 1000;
            while (numberOfExecuteRows == 1000)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("delete top(1000) from [dbo].[APILogsData] where id in (select id from [dbo].[APILogs] where [CreateDate] < GETDATE() - 90 )", cn);
                    cn.Open();
                    numberOfExecuteRows = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            #endregion


            #region APILogs
            numberOfExecuteRows = 1000;
            while (numberOfExecuteRows == 1000)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("delete top(1000) from [dbo].[APILogs] where [CreateDate] < GETDATE() - 90", cn);
                    cn.Open();
                    numberOfExecuteRows = cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }

            #endregion

        }
    }
}