using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
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

        public TaxWithholdingAssessOfficePM GetByNumber(string number, int tenant)
        {
            TaxWithholdingAssessOffice poco = this.repository.GetSingleTaxWithholdingAssessOffice(number, tenant);
            if (poco != null)
            {
                return GetEntityPM(poco);
            }
            else
            {
                return null;
            }
        }

    }
}
