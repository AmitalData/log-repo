
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

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
            for (int i = 0; i <= 2; i++)
            {
                Thread.Sleep(new TimeSpan(0,0,30));
                LogInfo("Log Info # " + i + " , # is Event");
            }

        }
    }
}