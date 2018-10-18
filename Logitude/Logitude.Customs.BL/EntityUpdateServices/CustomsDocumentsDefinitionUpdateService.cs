using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsDocumentsDefinitionUpdateService
    {
        protected override void OnCreating(CustomsDocumentsDefinitionPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsDocumentsDefinition", entityPM.Tenant);
            //entityPM.Tenant = entityParentPM.Tenant;
        }
    }
}
