using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial   class TaxWithholdingAssessOfficeQueryService
    {

        public bool CheckIfTaxOfficeExists(string code, int tenant)
        {
            return (from a in context.TaxWithholdingAssessOffices where a.Code == code && a.Tenant == tenant
                   select a).Any();
        }
    }
}
