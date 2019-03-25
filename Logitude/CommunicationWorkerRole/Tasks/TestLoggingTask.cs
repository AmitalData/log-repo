
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
namespace CommunicationWorkerRole.Tasks
{
    public class TestLoggingTask : TaskManagerBase
    {
        public TestLoggingTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            for (int i = 0; i <= 10; i++)
            {
                if (i % 2 == 0 && i != 10)
                {
                    LogInfo("Log # " + i + " , # is Event");
                }
                else
                {
                    Logwarning("Log # " + i + " , # is Odd");
                }
                if (i == 10)
                {
                    LogException("# " + i + " is an Exception ^_^");
                }
            }

        }
    }
}