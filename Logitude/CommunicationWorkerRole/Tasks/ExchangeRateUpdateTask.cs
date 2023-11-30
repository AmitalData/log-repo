using Logitude.Accounting.BL.CoreBL;
using Logitude.SystemLogs;
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
        public ExchangeRateUpdateTask(string Id, int tenant) : base(Id, tenant)
        {
            Tenant = tenant;
        }

        public override void StartTask()
        {        
            try
            {
                UpdateExchangeRateByExternalLink();               
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", $"ExchangeRateUpdateTask()", null);
            }
        }
        private void UpdateExchangeRateByExternalLink()
        {
            ExchangeRatesFromExternalLinkUpdateService exchangeRatesFromExternalLinkUpdateService = new ExchangeRatesFromExternalLinkUpdateService(Tenant);
            exchangeRatesFromExternalLinkUpdateService.UpdateRatesByExternalXml();
        }

    }
}
