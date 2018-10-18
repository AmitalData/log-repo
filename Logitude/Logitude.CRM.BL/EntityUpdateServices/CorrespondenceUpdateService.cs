using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class CorrespondenceUpdateService
    {
        protected override void OnCreating(CorrespondencePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                ICRMContext crmContext = this.MainContext as ICRMContext;

                entityPM.Id = IdCounter.GetNumber("Correspondence", entityPM.Tenant);

                if (entityPM.Attachments.Count > 0)
                {
                    foreach (string item in entityPM.Attachments)
                    {
                        CorrespondencesAttachment attachment = new CorrespondencesAttachment()
                        {
                            Id = IdCounter.GetNumber("CorrespondencesAttachment", entityPM.Tenant),
                            Tenant = entityPM.Tenant,
                            DocumentFilingId = item,
                            CorrespondenceId = entityPM.Id,
                        };

                        crmContext.CorrespondencesAttachments.Add(attachment);
                    }
                }
            }
        }

        protected override void OnUpdating(EntityPMs.CorrespondencePM entityPM)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            string myLoggedUserId = null;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
            if (loggedContact != null)
            {
                myLoggedUserId = loggedContact.Id;
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityPM.CreatedByContactId == null)
                {
                    entityPM.CreatedByContactId = myLoggedUserId;
                }

                entityPM.CreateDate = todayDate;
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void OnUpdating(EntityPMs.CorrespondencePM entityPM, Correspondence entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void Trace(CorrespondencePM entityPM, Correspondence entityPOCO, string changesXml)
        {

        }
    }
}
