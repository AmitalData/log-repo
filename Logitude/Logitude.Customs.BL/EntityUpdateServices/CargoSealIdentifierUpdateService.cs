using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CargoSealIdentifierUpdateService
    {
        protected override void OnCreating(CargoSealIdentifierPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CargoSealIdentifier", entityPM.Tenant);
            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(CargoSealIdentifierPM entityPM)
        {
            base.OnUpdating(entityPM);
        }

        protected override void UpdateComposition(CargoSealIdentifierPM entityPM)
        {
            CargoSealUpdateService cargoSealUpdateService = new CargoSealUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            cargoSealUpdateService.UpdateMulti(entityPM.CargoSeals, entityPM.DeletedCargoSeals, entityPM, false);
        }
    }
}
