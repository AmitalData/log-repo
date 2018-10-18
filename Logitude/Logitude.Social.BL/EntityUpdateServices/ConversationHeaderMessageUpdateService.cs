using Logitude.Server.Tools.Counters;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.Helpers;
using Logitude.Social.Data.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityUpdateServices
{
    public partial class ConversationHeaderMessageUpdateService
    {
        SocialMessageAlertsHelper helper = new SocialMessageAlertsHelper();
        protected override void OnCreating(ConversationHeaderMessagePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("ConversationHeaderMessage", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            }
        }

        protected override void Trace(ConversationHeaderMessagePM entityPM, ConversationHeaderMessage entityPOCO, string changesXml)
        {

            if (entityPM != null)
            {
    
                helper.SendEmailAlert(entityPM);

            }


        }

    }

}
