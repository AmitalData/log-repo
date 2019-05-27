
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace CommunicationWorkerRole.Tasks
{
    public class RetriesAndReschedulingTask : TaskManagerBase
    {
        public RetriesAndReschedulingTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            throw new System.Exception("Exceptins to test Retries And Rescheduling !");

        }
    }
}