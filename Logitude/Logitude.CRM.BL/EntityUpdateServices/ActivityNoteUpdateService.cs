using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Social.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class ActivityNoteUpdateService
    {
        protected override void OnCreating(ActivityNotePM entityPM, ActivityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("ActivityNote", entityPM.Tenant);
                entityPM.ActivityId = entityParentPM.Id;

                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.CreateDate = todayDate;
                entityPM.UpdateDate = todayDate;

                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                if (entityPM.PostToFollowers)
                {
                    string myBodyText = "Sales Notes updated: " + Environment.NewLine + entityPM.Notes;
                    AutomaticPosting.CreatePost(entityParentPM.Id, "Activity", entityPM.CreatedByUserId, entityParentPM.ActivityTypeCode + "-" + entityParentPM.Subject, myBodyText, true, entityPM.Tenant);
                    entityPM.PostToFollowers = false;
                }
            }            
        }

        protected override void OnUpdating(ActivityNotePM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = todayDate;

                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }
            }
        }
    }
}
