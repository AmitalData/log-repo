using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class CustomsCloseCourierMasterTask : TaskManagerBase
    {
        public CustomsCloseCourierMasterTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            //Thread.CurrentThread.Abort();
            for (int i = 0; i <= 3; i++)
            {
                Logwarning("Log warning # " + i + " , Be careful !!");
                LogInfoToDB("Log LogInfoToDB # " + i + " !!");
                LogInfo($"LogInfo({i})");
                LogException($"LogException{i}");
            }

            //if (DateTime.Now.Minute % 5 == 0)
            //{
            //    Thread.CurrentThread.Abort();
            //}
            //Thread.Sleep(60000);


        }
    }
}