using CustomsWorkerRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.BL
{
    public class UpdateClosedTablesWorker : WorkerBase
    {

        UpdateClosedTablesWR _UpdateClosedTablesWR;
        public UpdateClosedTablesWorker(double interval, int id)
            : base(interval, id)
        {
            _UpdateClosedTablesWR = new UpdateClosedTablesWR();
        }
        public override void DoIt()
        {
            _UpdateClosedTablesWR.OnStart();
            _UpdateClosedTablesWR.Run(); 
        }
    }
}
