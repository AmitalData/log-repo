using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IInterestReportUpdateServiceExt
    {
        void UpdateConfirmCreateInvoice(InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext,string ARInvoiceId=null, string InvoiceNumber=null, double? AmountInLocalCurrency=null, string InvoiceEntitiId=null);
    }
}
