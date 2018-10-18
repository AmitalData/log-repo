using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomBanksCardUpdateService
    {

        protected override void OnCreating(CustomBanksCardPM entityPM, CustomBankPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomBanksCard", entityPM.Tenant);
            entityPM.CustomBankId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;
        }

    }
}
