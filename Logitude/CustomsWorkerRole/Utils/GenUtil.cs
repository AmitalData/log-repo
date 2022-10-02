using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.Utils
{
    public class GenUtil
    {
        public static void CollectGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public static int GetQueueTimeOutInMin()
        {
            int iQueueTimeOutInMin = 20;
            string QueueTimeOutInMin = ConfigurationManager.AppSettings.Get("QueueTimeOutInMin") ?? "";
            if (string.IsNullOrWhiteSpace(QueueTimeOutInMin))
            {
                return iQueueTimeOutInMin;
            }
            int.TryParse(QueueTimeOutInMin, out iQueueTimeOutInMin);
            if (iQueueTimeOutInMin < 1)
            {
                iQueueTimeOutInMin = 1;
            }

            return iQueueTimeOutInMin;
        }
    }
}
