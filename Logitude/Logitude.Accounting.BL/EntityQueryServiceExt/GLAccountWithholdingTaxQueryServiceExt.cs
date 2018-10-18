using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
    public class GLAccountWithholdingTaxQueryServiceExt : IGLAccountWithholdingTaxQueryServiceExt
    {
        public GLAccountWithholdingTaxQueryServiceExt()
        {

        }

        public GLAccountWithholdingTaxPM GetAccountWithholdingTaxPMByglAccountAndDate(string glaccountId, DateTime? date, int tenant)
        {
            GLAccountWithholdingTaxQueryService query = new GLAccountWithholdingTaxQueryService(tenant);
            return query.GetAccountWithholdingTaxPMByglAccountAndDate(glaccountId, date);
        }
    }
}
