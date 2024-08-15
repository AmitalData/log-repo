using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IInterestReportsConnectedInvoiceUpdateServiceExt
    {
        void UpdateInterestLastBatchService(string ReportId, int Tenant, IAccountingContext MainContext, string ARInvoiceId);
        bool CheckInterestReportsConnected(string ReportId, int Tenant, IAccountingContext MainContext);

    }

}
