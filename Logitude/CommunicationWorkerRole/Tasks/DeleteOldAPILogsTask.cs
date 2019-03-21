
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;

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
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldAPILogsTask]", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                var output = cmd.ExecuteNonQuery();
                cn.Close();
            }

        }
    }
}