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
    public partial class CustomsExchangeRateUpdateService : EntityUpdateService<CustomsExchangeRate, CustomsExchangeRatePM, EntityPM>
    {
        protected override void OnCreating(CustomsExchangeRatePM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsExchangeRate", entityPM.Tenant);
        }

    }
}
