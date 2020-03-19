using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountInterestPeriodQueryService
    {
        public List<GLAccountInterestPeriodPM> GetLAccountInterestPeriodPMsByInterestDate(string glAccountId,DateTime InterestCalculationDate,int tenant)
        {
            GLAccountInterestPeriodRepository gLAccountInterestPeriodRepository = new GLAccountInterestPeriodRepository(tenant);
            List<GLAccountInterestPeriod> gLAccountInterestPeriods = gLAccountInterestPeriodRepository.GetLAccountInterestPeriodPMsByInterestDate(glAccountId, InterestCalculationDate, tenant);
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = new List<GLAccountInterestPeriodPM>();
            for(int i = 0; i < gLAccountInterestPeriods.Count(); i++)
            {
                GLAccountInterestPeriodPM gLAccountInterestPeriodPM = GetEntityPM(gLAccountInterestPeriods[i]);
                gLAccountInterestPeriodPMs.Add(gLAccountInterestPeriodPM);
            }
            return gLAccountInterestPeriodPMs;
        }
    }
}
