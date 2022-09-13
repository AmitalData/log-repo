using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityQueryServices
{
    public partial class DashboardQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, DashboardPM entityPM)
        {
            IDashboardContext context = MainContext as IDashboardContext;
            DashboardKeys dashboardKeys = entityKeys as DashboardKeys;

            WidgetQueryService widgetQueryService = new WidgetQueryService(context);
            entityPM.Widgets = widgetQueryService.GetMulti(dashboardKeys, true);

            DashboardSharedUserQueryService dashboardSharedUserQuery = new DashboardSharedUserQueryService(context);
            entityPM.DashboardSharedUsers = dashboardSharedUserQuery.GetMulti(dashboardKeys, true);
        }

        public List<DashboardPM> GetDashboardPMs(int tenant)
        {
            return (from a in context.Dashboards
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
                    }).ToList();
        }
    }
}
