using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Infrastructure.BL.EntityUpdateServices
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

                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
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
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
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
        }
    }
}
