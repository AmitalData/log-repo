using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsRequiredFieldUpdateService : EntityUpdateService<CustomsRequiredField, CustomsRequiredFieldPM, EntityPM>
    {

        protected override void OnCreating(CustomsRequiredFieldPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsRequiredField", entityPM.Tenant);
        }

     

    }
}
