using Logitude.Customs.Data.EntityPOCOs;
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
    public partial class CargoSealUpdateService : EntityUpdateService<CargoSeal, CargoSealPM, CargoSealIdentifierPM>
    {
        protected override void OnCreating(CargoSealPM entityPM, CargoSealIdentifierPM entityParentPM)
        {
            if (entityParentPM != null)
            {
                entityPM.Id = IdCounter.GetNumber("Customs.CargoSeal", entityPM.Tenant);
                entityPM.CargoSealIdentifierId = entityParentPM.Id;
                entityPM.Tenant = entityParentPM.Tenant;
            }
        }
    }
}
