
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace CommunicationWorkerRole.Tasks
{
    public class TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask : TaskManagerBase
    {
        public TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            for (int i = 0; i <= 2; i++)
            {
                Thread.Sleep(new TimeSpan(0,0,30));
                if (i != 2)
                {
                    LogInfo("Log Info # " + i + " , # is Event");
                }
                else if (DateTime.Now.Minute % 2 == 0)
                {
                    LogException("Log Exception # " + i + " , # Oh Yea");
                } 
            }

        }
    }
}