using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class SLAHeaderUpdateService
    {

        protected override void OnCreating(EntityPMs.SLAHeaderPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("SLAHeader", entityPM.Tenant);
            }
        }

        protected override void OnUpdating(EntityPMs.SLAHeaderPM entityPM)
        {
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }

            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdateDate = myDate;
            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                entityPM.CreatedByUserId = myLoggedUserId;
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void OnUpdating(EntityPMs.SLAHeaderPM entityPM, SLAHeader entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void UpdateComposition(SLAHeaderPM entityPM)
        {
            //reset line nunmbers
            CheckLineNumber(entityPM);

            SLALineUpdateService noteUpdateService = new SLALineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            noteUpdateService.UpdateMulti(entityPM.SLALines, entityPM.DeletedSLALines, entityPM, false);

            SLAEscalationUpdateService escalationUpdateService = new SLAEscalationUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            escalationUpdateService.UpdateMulti(entityPM.SLAEscalations, entityPM.DeletedSLAEscalations, entityPM, false);
        }

        protected override void Trace(SLAHeaderPM entityPM, SLAHeader entityPOCO, string changesXml)
        {
           
        }

        protected override void CheckConcurrency(SLAHeaderPM entityPM, SLAHeader entityPOCO)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            {

            }

            base.CheckConcurrency(entityPM, entityPOCO);
        }

        private void CheckLineNumber(SLAHeaderPM entityPM)
        {
            List<SLAEscalationPM> escalationsList = entityPM.SLAEscalations.Where(d => d.EscalationFor == "FR" && d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            if (escalationsList.Count > 0)
            {
                int count = 1;
                foreach (SLAEscalationPM item in escalationsList.OrderBy(o=>o.LineNumber))
                {
                    if (item.ChangeSetOp == ChangeSetOperation.None)
                    {
                        item.ChangeSetOp = ChangeSetOperation.Update;
                    }

                    item.LineNumber = count;
                    count += 1;
                }
            }

            escalationsList.Clear();
            escalationsList = entityPM.SLAEscalations.Where(d => d.EscalationFor == "RW" && d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            if (escalationsList.Count > 0)
            {
                int count = 1;
                foreach (SLAEscalationPM item in escalationsList.OrderBy(o => o.LineNumber))
                {
                    if (item.ChangeSetOp == ChangeSetOperation.None)
                    {
                        item.ChangeSetOp = ChangeSetOperation.Update;
                    }

                    item.LineNumber = count;
                    count += 1;
                }
            }
        }

    }
}
