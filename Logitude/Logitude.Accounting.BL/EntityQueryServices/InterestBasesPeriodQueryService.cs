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
    public partial class InterestBasesPeriodQueryService
    {
        public InterestBasesPeriodPM GetInterestBasesPeriodPMByBaseTypeIdAndStartDate(string interestBaseTypeId,DateTime startDate,int tenant)
        {
            InterestBasesPeriodRepository interestBasesPeriodRepository = new InterestBasesPeriodRepository(tenant);
            InterestBasesPeriod interestBasesPeriod = interestBasesPeriodRepository.GetInterestBasesPeriodByBaseTypeIdWithinInterestBaseStartDate(interestBaseTypeId, startDate, tenant);
            InterestBasesPeriodPM interestBasesPeriodPM = GetEntityPM(interestBasesPeriod);
            return interestBasesPeriodPM;
        }
    }
}
