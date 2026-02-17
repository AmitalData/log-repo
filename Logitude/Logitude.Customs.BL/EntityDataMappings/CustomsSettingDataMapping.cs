
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsSettingDataMapping: IMapping<CustomsSettingPM, CustomsSetting>
   {

        public void CustomPMToPOCO(CustomsSettingPM entityPM, CustomsSetting entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
               
            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(CustomsSettingPM entityPM, CustomsSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private static void BuildSearchFields(CustomsSettingPM entityPM, CustomsSetting poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.DCAPartnerVault + "," + entityPM.DCAServiceAddress + "," + entityPM.IIGServiceAddress + "," + entityPM.SignServiceAddress + "," + entityPM.UServerServiceAddress;

            if (isNewEntity)
            {

            }

            else
            {

            }

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

   }


}
   