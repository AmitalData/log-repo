
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.CloseTables;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CargoSealIdentifierDataMapping: IMapping<CargoSealIdentifierPM, CargoSealIdentifier>
   {

        public void CustomPMToPOCO(CargoSealIdentifierPM entityPM, CargoSealIdentifier entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(CargoSealIdentifierPM entityPM, CargoSealIdentifier entityPOCO)
        {

            CustomMappedPMProperties.Add(PMPropertyNames.StatusName);

            if (entityPOCO.Status != null)
            {
                CustomsCargoSealStatus customsCargoSealStatus = new CustomsCargoSealStatus();
                InterfaceDetails cargoSealStatus = customsCargoSealStatus.GetAllCustomsCargoSealStatus().First(x=>x.Code== entityPOCO.Status);
                if (cargoSealStatus != null)
                {
                    entityPM.StatusName = cargoSealStatus.Name;
                }
            }
            //throw new NotImplementedException();
        }
   }


}
   