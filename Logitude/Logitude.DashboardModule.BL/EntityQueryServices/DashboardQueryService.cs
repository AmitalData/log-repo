using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityKeys;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
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

            DashboardGlobalFilterQueryService dashboardGlobalFilterQuery = new DashboardGlobalFilterQueryService(context);
            entityPM.DashboardGlobalFilters = dashboardGlobalFilterQuery.GetMulti(dashboardKeys, true);
        }

        public string GetDefaultDashboardId(int tenant, string loggedContactId)
        {
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            DashboardSharedUserRepository dashboardSharedUserRepository = new DashboardSharedUserRepository(context);
            IQueryable<Dashboard> dashboards = dashboardRepository.GetAll(tenant);
            IQueryable<string> dashboardIds = dashboards.Select(s => s.Id);
            IQueryable<DashboardSharedUser> users = dashboardSharedUserRepository.GetDashboardSharedUsersByDashboardsIds(dashboardIds, tenant);

            DashboardPM dashboard = (from d in dashboards
                    where d.Tenant == tenant
                    && ((d.PermissionLevelCode == "ONM" && d.CreatedByUserId == loggedContactId)
                    || (d.PermissionLevelCode == "SPF" && users.Select(s => s.UserId).Contains(loggedContactId))
                    || (d.PermissionLevelCode == "PUB"))
                    select new DashboardPM()
                    {
                        Id = d.Id,
                        Tenant = d.Tenant,
                        CreateDate = d.CreateDate,
                        CreatedByUserId = d.CreatedByUserId,
                        UpdateDate = d.UpdateDate,
                        UpdatedByUserId = d.UpdatedByUserId,
                        SearchFields = d.SearchFields,
                        Name = d.Name,
                    }).FirstOrDefault();

            if (dashboard != null) return dashboard.Id;
            else return null;
        }
    }
}
