using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.EntityQueryServices
{
    public partial class SprintQueryService
    {

        public IQueryable<SprintPM> GetSprintPMsByTenant(int tenant)
        {
            ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
            IQueryable<SprintPM> sprints = from a in myContext.Sprints
                                           where a.Tenant == tenant
                                           select new SprintPM()
                                           {
                                               Id = a.Id,

                                               Tenant = a.Tenant,

                                               CreatedByUserId = a.CreatedByUserId,

                                               UpdatedByUserId = a.UpdatedByUserId,

                                               SearchFields = a.SearchFields,

                                               FromDate = a.FromDate,

                                               ToDate = a.ToDate,

                                               Name = a.Name,
                                           };
            return sprints;
        }
    }
}
