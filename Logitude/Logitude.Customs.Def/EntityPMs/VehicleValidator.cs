using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
   public class VehicleValidator
    {

       public static ValidationResult IsVehicleChassisNumberValid(VehiclePM vehiclePM, System.ComponentModel.DataAnnotations.ValidationContext context)
       {
           bool valid = true;

           VehicleRepository repository = new VehicleRepository(vehiclePM.Tenant);

           Vehicle vehicle = repository.GetSingle(new VehicleKeys() { Id = vehiclePM.Id });
           if (vehicle != null)
           {
               if (vehiclePM.VehicleChassisNumber != vehicle.VehicleChassisNumber)
               {

                   bool exist = (repository.GetAll(vehiclePM.Tenant).Where(d => d.VehicleChassisNumber == vehiclePM.VehicleChassisNumber && d.Tenant == vehiclePM.Tenant)).Any();
                   if (exist)
                   {
                       valid = false;
                       //return new ValidationResult(TranslateTextsClass.Translate("Customs.Vehicle.O.AlreadyExist", vehiclePM.Tenant));
                       throw new Exception(TranslateTextsClass.Translate("Customs.Vehicle.O.AlreadyExist", vehiclePM.Tenant));
                   }

                   return null;


               }
           }

           return null;
        
       }

    }
}
