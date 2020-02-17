using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CargoSealIdentifierQueryService : EntityQueryService<CargoSealIdentifier, CargoSealIdentifierKeys, CargoSealIdentifierPM, object, CargoSealIdentifierKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, CargoSealIdentifierPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CargoSealIdentifierKeys cargoSealIdentifierKeys = entityKeys as CargoSealIdentifierKeys;
            CargoSealQueryService cargoSealQueryService = new CargoSealQueryService(context);

            entityPM.CargoSeals = cargoSealQueryService.GetMulti(cargoSealIdentifierKeys, true);
        }

        public List<CargoSealIdentifierPM> GetDeclarationCargoSealIdentifierList(string declarationId, int tenant)
        {
            List<CargoSealIdentifier> cargoSealIdentifier = repository.GetDeclarationCargoSealIdentifierList(declarationId, tenant);
            List<CargoSealIdentifierPM> cargoSealIdentifierPMList = new List<CargoSealIdentifierPM>();
            if (cargoSealIdentifier != null)
            {
                foreach (var cargoSealIdentifierItem in cargoSealIdentifier)
                {
                    CargoSealIdentifierPM cargoSealIdentifierPM = this.GetSingle(cargoSealIdentifierItem.Id, true, false);
                    cargoSealIdentifierPMList.Add(cargoSealIdentifierPM);
                }
            }
            return cargoSealIdentifierPMList;
        }
    }
}
