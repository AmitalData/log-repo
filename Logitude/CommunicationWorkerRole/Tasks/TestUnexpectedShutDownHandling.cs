
using CommunicationWorkerRole.Tasks;
using Simplog.Data.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace CommunicationWorkerRole.Tasks
{
    public class TestUnexpectedShutDownHandling : TaskManagerBase
    {
        public TestUnexpectedShutDownHandling(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            LogInfoToDB("Thread will sleep for 1 min. then it will terminate .. you can shut down the WR in this Min.  ..");
            Thread.Sleep(60000);
            LogInfoToDB("Start Aborting the thread ..");
            Thread.CurrentThread.Abort();
            LogInfoToDB("After Aborting the thread ..");
            //for (int i = 0; i <= 3; i++)
            //{ 
            //    Logwarning("Log warning # " + i + " , Be careful !!");
            //}
            //if (DateTime.Now.Minute % 5 == 0)
            //{
            //    Thread.CurrentThread.Abort();
            //}
            //Thread.Sleep(60000);


        }
    }
}