using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityUpdateServices
{
    public partial class DashboardUpdateService
    {
        protected override void OnCreating(DashboardPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Dashboard", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }
            }
        }

        protected override void OnUpdating(DashboardPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }
            }
        }

        protected override void UpdateComposition(DashboardPM entityPM)
        {
            WidgetUpdateService widgetUpdateService = new WidgetUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            widgetUpdateService.UpdateMulti(entityPM.Widgets, entityPM.DeletedWidgets, entityPM, false);

            DashboardSharedUserUpdateService dashboardSharedUserUpdateService = new DashboardSharedUserUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            dashboardSharedUserUpdateService.UpdateMulti(entityPM.DashboardSharedUsers, entityPM.DeletedDashboardSharedUsers, entityPM, false);

            DashboardGlobalFilterUpdateService dashboardGlobalFilterUpdateService = new DashboardGlobalFilterUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            dashboardGlobalFilterUpdateService.UpdateMulti(entityPM.DashboardGlobalFilters, entityPM.DeletedDashboardGlobalFilters, entityPM, false);
        }

        public void Delete(string dashboardId, int tenant)
        {
            IDashboardContext context = MainContext as DashboardContext;
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            Dashboard entityPOCO = dashboardRepository.GetSingle(dashboardId, tenant);
            if (entityPOCO == null) return;

            this.DeleteWidgets(context, entityPOCO);
            this.DeleteSharedUsers(context, entityPOCO);
            this.DeleteGlobalFilters(context, entityPOCO);

            dashboardRepository.Remove(entityPOCO);
            dashboardRepository.SubmitChanges();
        }
        private void DeleteWidgets(IDashboardContext context, Dashboard entityPOCO)
        {
            WidgetRepository widgetRepository = new WidgetRepository(context);
            List<Widget> widgets = widgetRepository.GetWidgetsByDashboardId(entityPOCO.Id, entityPOCO.Tenant).ToList();
            foreach (Widget item in widgets)
            {
                this.DeleteWidgetMeasures(context, item);
                widgetRepository.Remove(item);
            }
        }
        private void DeleteWidgetMeasures(IDashboardContext context, Widget entityPOCO)
        {
            WidgetMeasureRepository widgetMeasureRepository = new WidgetMeasureRepository(context);
            List<WidgetMeasure> measures = widgetMeasureRepository.GetWidgetMeasuresByWidgetId(entityPOCO.Id, entityPOCO.Tenant).ToList();
            foreach (WidgetMeasure item in measures)
            {
                widgetMeasureRepository.Remove(item);
            }
        }
        private void DeleteSharedUsers(IDashboardContext context, Dashboard entityPOCO)
        {
            DashboardSharedUserRepository sharedUserRepository = new DashboardSharedUserRepository(context);
            List<DashboardSharedUser> users = sharedUserRepository.GetDashboardSharedUsersByDashboardId(entityPOCO.Id, entityPOCO.Tenant).ToList();
            foreach (DashboardSharedUser item in users)
            {
                sharedUserRepository.Remove(item);
            }
        }

        private void DeleteGlobalFilters(IDashboardContext context, Dashboard entityPOCO)
        {
            DashboardGlobalFilterRepository dashboardGlobalFilterRepository = new DashboardGlobalFilterRepository(context);
            List<DashboardGlobalFilter> users = dashboardGlobalFilterRepository.GetDashboardFiltersByDashboardId(entityPOCO.Id, entityPOCO.Tenant).ToList();
            foreach (DashboardGlobalFilter item in users)
            {
                dashboardGlobalFilterRepository.Remove(item);
            }
        }
    }
}
