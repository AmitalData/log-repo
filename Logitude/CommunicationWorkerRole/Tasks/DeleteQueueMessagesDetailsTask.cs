using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteQueueMessagesDetailsTask : TaskManagerBase
    {
        public DeleteQueueMessagesDetailsTask(string Id, int tenant)
            : base(Id,tenant)
        {

        }
        public override void StartTask()
        {
            QueueMessageMoreDetailsRepository Repo = new QueueMessageMoreDetailsRepository(0);
            var Queues = Repo.GetQueueMessageMoreDetails(DateTime.Now);
            foreach (var item in Queues)
            {
                Repo.Remove(item); 
            }
            Repo.SubmitChanges();
        }
    }
}
