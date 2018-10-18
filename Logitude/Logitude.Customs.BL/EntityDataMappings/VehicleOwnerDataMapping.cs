
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
   
   public partial class VehicleOwnerDataMapping: IMapping<VehicleOwnerPM, VehicleOwner>
   {

        public void CustomPMToPOCO(VehicleOwnerPM entityPM, VehicleOwner entityPOCO)
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

        public void CustomPOCOToPM(VehicleOwnerPM entityPM, VehicleOwner entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ClientName);

            ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
            ClientPM client = clientQueryService.GetSingle(entityPOCO.ClientId, false, true);
            if (client != null)
            {
                entityPM.ClientName = client.FullName;

            }

            if (entityPOCO.PassCountryCode != null)
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(entityPOCO.PassCountryCode, false, true);
                if (customsCountry != null)
                {
                    entityPM.PassCountryName = customsCountry.LocalName;

                }
            }

            if (entityPOCO.ImporterPassportTypeCode != null)
            {
                PassportTypeQueryService passportTypeQueryService = new PassportTypeQueryService(entityPOCO.Tenant);
                PassportTypePM passportType = passportTypeQueryService.GetSingle(entityPOCO.ImporterPassportTypeCode, false, true);
                if (passportType != null)
                {
                    entityPM.ImporterPassportTypeName = passportType.LocalName;

                }
            }
        }
   }


}
   