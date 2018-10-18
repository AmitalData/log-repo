using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class NotificationDefinitionUpdateService : EntityUpdateService<NotificationDefinition, NotificationDefinitionPM, EntityPM>
    {

        protected override void OnUpdating(NotificationDefinitionPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            NotificationTenantDefinitionRepository definitionRep = new NotificationTenantDefinitionRepository(context);
            NotificationTenantDefinition definition = definitionRep.GetSingleNotificationTenantDefinitionByCode(entityPM.Code, entityPM.Tenant);
            if (definition != null)
            {
                definition.DefaultAssigneeId = entityPM.DefaultAssigneeId;

                definitionRep.Update(definition);

            }
            else
            {
                definition = new NotificationTenantDefinition();
                definition.DefaultAssigneeId = entityPM.DefaultAssigneeId;

                definition.Id = IdCounter.GetNumber("Customs.NotificationTenantDefinition", entityPM.Tenant);
                definition.Code = entityPM.Code;
                definition.Tenant = entityPM.Tenant;
                definitionRep.Add(definition);

            }

        }


    }
}
