using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class VehicleUpdateService
    {
       protected override void OnCreating(VehiclePM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.Vehicle", entityPM.Tenant);
        }

       protected override void UpdateComposition(VehiclePM entityPM)
        {
            VehicleOwnerUpdateService vehicleOwnerUpdateService = new VehicleOwnerUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            vehicleOwnerUpdateService.UpdateMulti(entityPM.VehicleOwners, entityPM.DeletedVehicleOwners, entityPM, false);


            VehicleSafetyAccessoryUpdateService vehicleSafetyAccessoryUpdateService = new VehicleSafetyAccessoryUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            vehicleSafetyAccessoryUpdateService.UpdateMulti(entityPM.VehicleSafetyAccessories, entityPM.DeletedVehicleSafetyAccessories, entityPM, false);
            base.UpdateComposition(entityPM);
        }

       protected override void CheckConcurrency(VehiclePM entityPM, Vehicle entityPOCO)
       {
           //if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
           if (entityPM.ConcurrencyGUID != entityPOCO.ConcurrencyGUID && entityPM.NewConcurrencyGUID != entityPOCO.ConcurrencyGUID)
           {
               string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
               throw new OptimisticConcurrencyException(msg);
           }

       }
      


    }
}
