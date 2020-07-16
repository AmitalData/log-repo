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
    public partial class InterestReportLinesByDateQueryService
    {
        public List<InterestReportLinesByDatePM> GetInterestReportLinesByDatePMsForInterestReport(string interestReportId,int tenant)
        {
            InterestReportLinesByDateRepository interestReportLinesByDateRepository = new InterestReportLinesByDateRepository(tenant);
            IQueryable<InterestReportLinesByDate> interestReportLinesByDates = interestReportLinesByDateRepository.GetInterestReportLinesByDatePMsForInterestReport(interestReportId, tenant);

            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = (from a in interestReportLinesByDates
                                                                              select new InterestReportLinesByDatePM()
                                                                              {
                                                                                  Id = a.Id,
                                                                                  Tenant = a.Tenant,
                                                                              }).ToList();
            return interestReportLinesByDatePMs;

        }
    }
}
