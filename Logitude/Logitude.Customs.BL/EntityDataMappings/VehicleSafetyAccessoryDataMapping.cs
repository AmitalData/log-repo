
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class VehicleSafetyAccessoryDataMapping: IMapping<VehicleSafetyAccessoryPM, VehicleSafetyAccessory>
   {

        public void CustomPMToPOCO(VehicleSafetyAccessoryPM entityPM, VehicleSafetyAccessory entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.VehicleId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.VehicleId = entityPM.VehicleId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.LineNumber = entityPM.LineNumber;

            }
        }

        public void CustomPOCOToPM(VehicleSafetyAccessoryPM entityPM, VehicleSafetyAccessory entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.VehicleSafAccessoryInstlTypName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VehicleSafetyAccessoryName);


            if (entityPOCO.VehicleSafAccessoryInstlTypCod != null)
            {
                VehicleSafeAccessoryInstlTypeQueryService vehicleSafetyAccessoryInstallationTypQueryService = new VehicleSafeAccessoryInstlTypeQueryService(entityPOCO.Tenant);
                VehicleSafeAccessoryInstlTypePM VehicleSafetyAccessoryInstallationType = vehicleSafetyAccessoryInstallationTypQueryService.GetSingle(entityPOCO.VehicleSafAccessoryInstlTypCod, false, true);
                if (VehicleSafetyAccessoryInstallationType != null)
                {
                    entityPM.VehicleSafAccessoryInstlTypName = VehicleSafetyAccessoryInstallationType.LocalName;

                }
            }

            if (entityPOCO.VehicleSafetyAccessoryCode != null)
            {
                VehicleSafetyAccessoryTypeQueryService vehicleSafetyAccessoryTypQueryService = new VehicleSafetyAccessoryTypeQueryService(entityPOCO.Tenant);
                VehicleSafetyAccessoryTypePM vehicleSafetyAccessoryType = vehicleSafetyAccessoryTypQueryService.GetSingle(entityPOCO.VehicleSafetyAccessoryCode, false, true);
                if (vehicleSafetyAccessoryType != null)
                {
                    entityPM.VehicleSafetyAccessoryName = vehicleSafetyAccessoryType.LocalName;

                }
            }

          
        }
   }


}
   