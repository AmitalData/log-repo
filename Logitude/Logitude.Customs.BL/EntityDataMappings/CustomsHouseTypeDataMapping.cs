
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
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsHouseTypeDataMapping: IMapping<CustomsHouseTypePM, CustomsHouseType>
   {

        public void CustomPMToPOCO(CustomsHouseTypePM entityPM, CustomsHouseType entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
        }

        public void CustomPOCOToPM(CustomsHouseTypePM entityPM, CustomsHouseType entityPOCO)
        {

            CustomsHouseTypeAdditionalRepository additionalRep = new CustomsHouseTypeAdditionalRepository(entityPM.Tenant);
            CustomsHouseTypeAdditional additional = additionalRep.GetSingleAdditionalByCode(entityPM.Code, entityPM.Tenant);
            if (additional != null)
            {
                entityPM.Tenant = additional.Tenant;
                entityPM.UnloadPortCode = additional.UnloadPortCode;
                entityPM.TransportModeId = additional.TransportModeId;
            }
        }
   }


}
   