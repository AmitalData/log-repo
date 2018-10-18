using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class CorrespondencesAttachmentUpdateService
    {
        protected override void OnCreating(CorrespondencesAttachmentPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("CorrespondencesAttachment", entityPM.Tenant);
        }

        protected override void OnUpdating(EntityPMs.CorrespondencesAttachmentPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void OnUpdating(EntityPMs.CorrespondencesAttachmentPM entityPM, CorrespondencesAttachment entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {

            }
        }

        protected override void Trace(CorrespondencesAttachmentPM entityPM, CorrespondencesAttachment entityPOCO, string changesXml)
        {
            

        }

    }
}
