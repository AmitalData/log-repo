using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsSettingUpdateService
    {
        protected override void OnCreating(CustomsSettingPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = Guid.NewGuid().ToString();
            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
