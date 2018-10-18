
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsHouseTypeAdditionalDataMapping: IMapping<CustomsHouseTypeAdditionalPM, CustomsHouseTypeAdditional>
   {

        public void CustomPMToPOCO(CustomsHouseTypeAdditionalPM entityPM, CustomsHouseTypeAdditional entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

          BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);

            
        }
        private static void BuildSearchFields(CustomsHouseTypeAdditionalPM entityPM, CustomsHouseTypeAdditional poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.Code + "," + entityPM.Name;

            if (isNewEntity)
            {

            }

            else
            {

            }

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(CustomsHouseTypeAdditionalPM entityPM, CustomsHouseTypeAdditional entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TransportModeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UnloadPortName);

            if (entityPOCO.Code != null)
            {
                CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(entityPOCO.Tenant);
                CustomsHouseTypePM customsHouseType = customsHouseTypeQueryService.GetSingle(entityPOCO.Code, false, true);
                entityPM.Name = customsHouseType.LocalName;
            }

            if (entityPOCO.TransportModeId != null)
            {
                CustomsTransportModeQueryService customsTransportModeQueryService = new CustomsTransportModeQueryService(entityPOCO.Tenant);
                CustomsTransportModePM customsTransportMode = customsTransportModeQueryService.GetSingle(entityPOCO.TransportModeId, false, true);
                entityPM.TransportModeName = customsTransportMode.LocalName;
            }

            if (entityPOCO.UnloadPortCode != null)
            {
                UnloadingSiteTypeQueryService unloadingSiteTypeQueryService = new UnloadingSiteTypeQueryService(entityPOCO.Tenant);
                UnloadingSiteTypePM unloadingSiteType = unloadingSiteTypeQueryService.GetSingle(entityPOCO.UnloadPortCode, false, true);
                entityPM.UnloadPortName = unloadingSiteType.LocalName;
            }
        }
   }


}
   