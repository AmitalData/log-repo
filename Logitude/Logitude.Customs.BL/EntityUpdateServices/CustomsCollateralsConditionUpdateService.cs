using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsCollateralsConditionUpdateService
    {
        protected override void OnCreating(CustomsCollateralsConditionPM entityPM, CustomsCollateralPM entityParentPM)
        {
            entityPM.CustomsCollateralId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;
        }
    }
}
