using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.BL.Helpers;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomBankUpdateService : EntityUpdateService<CustomBank, CustomBankPM, EntityPM>
    {

        protected override void OnCreating(CustomBankPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomBank", entityPM.Tenant);
        }

        protected override void UpdateComposition(CustomBankPM entityPM)
        {
            CustomBanksCardUpdateService customBanksCardUpdateService = new CustomBanksCardUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            customBanksCardUpdateService.UpdateMulti(entityPM.CustomBanksCards, entityPM.DeletedCustomBanksCards, entityPM, false);
            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(CustomBankPM entityPM, EntityPM entityParentPM)
        {
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant,
                "Customs.CustomBank"
                //"CustomBank"
                );
            base.AfterUpdating(entityPM, entityParentPM);
        }

    }
}
