using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
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
    public partial class SLAEscalationUpdateService
    {
        protected override void OnCreating(SLAEscalationPM entityPM, SLAHeaderPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("SLAEscalation", entityPM.Tenant);
                entityPM.SLAHeaderId = entityParentPM.Id;
            } 
        }

        protected override void OnUpdating(EntityPMs.SLAEscalationPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }

        }

        protected override void OnUpdating(EntityPMs.SLAEscalationPM entityPM, SLAEscalation entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }

        }

        protected override void UpdateComposition(SLAEscalationPM entityPM)
        {
            SLAEscalationRecepientUpdateService escalationsUpdateService = new SLAEscalationRecepientUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            escalationsUpdateService.UpdateMulti(entityPM.SLAEscalationRecepients, entityPM.DeletedSLAEscalationRecepients, entityPM, false);
        }

        protected override void Trace(SLAEscalationPM entityPM, SLAEscalation entityPOCO, string changesXml)
        {

        }

        protected override void CheckConcurrency(SLAEscalationPM entityPM, SLAEscalation entityPOCO)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            {

            }

            base.CheckConcurrency(entityPM, entityPOCO);
        }

    }
}
