using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
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
    public partial class SupportMailboxUpdateService
    {
        protected override void OnCreating(SupportMailboxPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("SupportMailbox", entityPM.Tenant);

                if (entityPM.IsDefault)
                {
                    ICRMContext currentContext = this.MainContext as ICRMContext;
                    SupportMailboxRepository supportMailboxRepository = new SupportMailboxRepository(currentContext);
                    SupportMailbox defaultMailBox = this.GetDefaultMailBoxId(supportMailboxRepository, entityPM.Tenant);
                    if (defaultMailBox != null)
                    {
                        defaultMailBox.IsDefault = false;
                        supportMailboxRepository.Update(defaultMailBox);
                    }
                }
            }
        }
        
        protected override void OnUpdating(SupportMailboxPM entityPM, SupportMailbox entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(entityPM.Tenant), entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                if (entityPM.IsDefault && !entityPOCO.IsDefault)
                {
                    ICRMContext currentContext = this.MainContext as ICRMContext;
                    SupportMailboxRepository supportMailboxRepository = new SupportMailboxRepository(currentContext);
                    SupportMailbox defaultMailBox = this.GetDefaultMailBoxId(supportMailboxRepository, entityPM.Tenant);
                    if (defaultMailBox != null && defaultMailBox.Id != entityPM.Id)
                    {
                        defaultMailBox.IsDefault = false;
                        supportMailboxRepository.Update(defaultMailBox);
                    }
                }
            }
        }

        private SupportMailbox GetDefaultMailBoxId(SupportMailboxRepository supportMailboxRepository, int tenant)
        {
            SupportMailbox defaultEntity = supportMailboxRepository.GetDefaultMailBox(tenant);
            return defaultEntity;
        }
    }
}
