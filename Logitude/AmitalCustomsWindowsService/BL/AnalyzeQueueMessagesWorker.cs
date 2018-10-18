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
    public class AnalyzeQueueMessagesWorker : WorkerBase
    {
        AnalyzeQueueMessagesWR _AnalyzeQueueMessagesWR;
        
        public AnalyzeQueueMessagesWorker(double interval, int id)
            :base(interval, id)
        {
            _AnalyzeQueueMessagesWR = new AnalyzeQueueMessagesWR();
        }
        public override void DoIt()
        {
            //Thread.Sleep(TimeSpan.FromSeconds(10)); 
            //Logger.LogMe("CreateAnalyzeQueueWorker  ..", false);
            _AnalyzeQueueMessagesWR.Run();
            
        }
    }
}
