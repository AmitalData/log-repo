
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
   
   public partial class ClientDrivingLicenseDataMapping: IMapping<ClientDrivingLicensePM, ClientDrivingLicense>
   {

        public void CustomPMToPOCO(ClientDrivingLicensePM entityPM, ClientDrivingLicense entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ClientId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.ClientId = entityPM.ClientId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Line = entityPM.Line;
            }
        }

        public void CustomPOCOToPM(ClientDrivingLicensePM entityPM, ClientDrivingLicense entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.DrivingLicenseCountryName);

            if (!string.IsNullOrEmpty(entityPOCO.DrivingLicenseCountryID))
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM customsCountryPM = customsCountryQueryService.GetSingle(entityPOCO.DrivingLicenseCountryID, false, true);
                if (customsCountryPM != null)
                {
                    entityPM.DrivingLicenseCountryName = customsCountryPM.LocalName;
                }
            }
        }
   }


}
   