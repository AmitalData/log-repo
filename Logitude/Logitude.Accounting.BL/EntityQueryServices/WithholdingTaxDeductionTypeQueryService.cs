using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class WithholdingTaxDeductionTypeQueryService
    {
        public WithholdingTaxDeductionTypePM GetByCode(string code, int tenant, bool getFromCache=false)
        {
            WithholdingTaxDeductionType poco = this.repository.GetSingleWithholdingTaxDeductionType(code, tenant, getFromCache);
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
