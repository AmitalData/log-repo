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
    class SendDataToAmitalWorker: WorkerBase
    {
        SendDataToAmitalWR _SendDataToAmitalWR;
        public SendDataToAmitalWorker(double interval, int id)
            :base(interval, id)
        {
            _SendDataToAmitalWR = new SendDataToAmitalWR();
        }
        public override void DoIt()
        {
            _SendDataToAmitalWR.OnStart();
            _SendDataToAmitalWR.Run();
            return;
            Thread.Sleep(TimeSpan.FromSeconds (10));
            Logger.LogMe("SendDataToAmitalWorker  ..", false);
        }
    }
}
