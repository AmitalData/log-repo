
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
namespace CommunicationWorkerRole.Tasks
{
    public class TestLoggingInfoTask : TaskManagerBase
    {
        public TestLoggingInfoTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            for (int i = 0; i <= 5; i++)
            {

                LogInfo("Log Info # " + i + " , # is Event");
            }

        }
    }
}