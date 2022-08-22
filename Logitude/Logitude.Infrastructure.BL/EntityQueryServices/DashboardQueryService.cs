using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DashboardQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, DashboardPM entityPM)
        {
            IInfrastructureContext context = MainContext as InfrastructureContext;
            DashboardKeys dashboardKeys = entityKeys as DashboardKeys;

            WidgetQueryService widgetQueryService = new WidgetQueryService(context);
            entityPM.Widgets = widgetQueryService.GetMulti(dashboardKeys, true);
        }

        public IQueryable<DashboardPM> GetDashboardPMs(int tenant)
        {
            IQueryable<DashboardPM> query = (from a in context.Dashboards
                                             where a.Tenant == tenant
                                             select new DashboardPM()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 CreateDate = a.CreateDate,
                                                 CreatedByUserId = a.CreatedByUserId,
                                                 UpdateDate = a.UpdateDate,
                                                 UpdatedByUserId = a.UpdatedByUserId,
                                                 SearchFields = a.SearchFields,
                                                 Name = a.Name,
                                             });
            return query;
        }
    }
}
