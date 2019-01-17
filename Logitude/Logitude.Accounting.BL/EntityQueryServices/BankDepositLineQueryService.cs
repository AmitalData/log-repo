using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class BankDepositLineQueryService
    {
        public List<BankDepositLinePM> GetDepositLinePMsByDepositIds(List<string> depositIds, int tenant)
        {
            List<BankDepositLine> depositLines = (from a in context.BankDepositLines
                                                  where depositIds.Contains(a.DepositId) && a.Tenant == tenant
                                                  select a).ToList();
            return depositLines.Select(rec => this.GetEntityPM(rec)).ToList();
        

        }

    }
}
