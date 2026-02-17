using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class GLAccountWithholdingTaxQueryService
    {

        public int GetMaxLineNumber(string glAccountId, int tenant)
        {
            return repository.GetMaxLineNumber(glAccountId, tenant);
        }

        public GLAccountWithholdingTaxPM GetAccountWithholdingTaxPMByglAccountAndDate(string glaccountId, DateTime? date)
        {
            GLAccountWithholdingTax poco=(from a in context.GLAccountWithholdingTax
                    where a.GLAccountId == glaccountId && !a.Inactive && ((a.FromDate < date || a.FromDate == date) && (a.ToDate > date || a.ToDate== date) )  
                    select a).FirstOrDefault();

            var result = GetEntityPM(poco);
            return result;

        }
    }
}
