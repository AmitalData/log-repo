using Logitude.Accounting.BL.CoreBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
   public class ExchangeRateUpdateTask :TaskManagerBase
    {


        int Tenant;
        private StringBuilder _SB;
        public ExchangeRateUpdateTask(string Id, int tenant) : base(Id, tenant)
        {
            Tenant = tenant;
            _SB = new StringBuilder();
        }

        public override void StartTask()
        {
            bool failed = false;
            try
            {


                _SB.Append(DateTime.Now.ToString()).AppendLine("ExchangeRateUpdateTask:Start");
                ExchangeRatesFromExternalLinkUpdateService exchangeRatesFromExternalLinkUpdateService = new ExchangeRatesFromExternalLinkUpdateService(Tenant);
                exchangeRatesFromExternalLinkUpdateService.UpdateRatesByExternalXML();

                //}
            }
            finally
            {
                if (failed)
                {
                    throw new Exception(_SB.ToString());
                }
            }
            //base.StartTask();
        }
    }
}
