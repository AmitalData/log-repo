
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace CommunicationWorkerRole.Tasks
{
    public class TestLoggingWarningTask : TaskManagerBase
    {
        public TestLoggingWarningTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            //for (int i = 0; i <= 3; i++)
            //{

            //    Logwarning("Log warning # " + i + " , Be careful !!");
            //}
            Thread.Sleep(120000);

        }
    }
}