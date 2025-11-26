using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class OceanCarrierAPITask : TaskManagerBase
    {
        private StringBuilder _SB;
        public OceanCarrierAPITask(string Id, int tenant) : base(Id, tenant)
        {
            _SB = new StringBuilder();
        }
        public override void StartTask() { }

    }
}
