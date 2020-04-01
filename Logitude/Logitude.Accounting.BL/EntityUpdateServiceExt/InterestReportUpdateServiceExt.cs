using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.InterestService;
namespace Logitude.Accounting.BL.EntityUpdateServiceExt
{
    public class InterestReportUpdateServiceExt : IInterestReportUpdateServiceExt
    {
        public InterestReportUpdateServiceExt()
        {

        }

        public void Update(InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext)
        {
            InterestReportService interestReportService = new InterestReportService();
            interestReportService.PutConfirmCreateInvoice(interestReportPM, Tenant, MainContext);
        }
    }
}
