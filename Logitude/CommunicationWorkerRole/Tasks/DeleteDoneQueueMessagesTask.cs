using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteDoneQueueMessagesTask : TaskManagerBase
    {
        public DeleteDoneQueueMessagesTask(string Id, int tenant)
            : base(Id,tenant)
        {

        }
        public override void StartTask()
        {
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteDoneQueueMessages]", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                var output = cmd.ExecuteNonQuery();
                cn.Close();
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
