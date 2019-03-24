

using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;

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
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteOldQueueMessageMoreDetailsTask]", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                numberOfExecuteRow = cmd.ExecuteNonQuery();
                cn.Close();
            }
        }


    }
}
