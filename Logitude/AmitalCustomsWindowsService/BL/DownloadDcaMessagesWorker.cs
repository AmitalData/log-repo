using AmitalCustomsWindowsService.Utils;
using CustomsWorkerRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.BL
{
    class DownloadDcaMessagesWorker: WorkerBase
    {

        DownloadDcaMessagesWR _DownloadDcaMessagesWR; 
        public DownloadDcaMessagesWorker(double interval, int id)
            :base(interval, id)
        {
            _DownloadDcaMessagesWR = new DownloadDcaMessagesWR();
        }
        public override void DoIt()
        {
            _DownloadDcaMessagesWR.Run();
            return;
            throw new Exception("wwww");
  
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Logger.LogMe("DownloadDcaMessagesWorker  ..", false);
        }
    }
}
