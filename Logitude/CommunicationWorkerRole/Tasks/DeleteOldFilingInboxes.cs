using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteOldFilingInboxes : TaskManagerBase
    {
        public DeleteOldFilingInboxes(string Id, int tenant)
            : base(Id, tenant)
        {
        }

        public override void StartTask()
        {
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldFilingInboxes]", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                var output = cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
