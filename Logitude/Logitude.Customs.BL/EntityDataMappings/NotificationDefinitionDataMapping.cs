
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
   
   public partial class NotificationDefinitionDataMapping: IMapping<NotificationDefinitionPM, NotificationDefinition>
   {

        public void CustomPMToPOCO(NotificationDefinitionPM entityPM, NotificationDefinition entityPOCO)
        {
          BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        

            
        }
        private static void BuildSearchFields(NotificationDefinitionPM entityPM, NotificationDefinition poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.Code + "," + entityPM.Code + "," + entityPM.LocalName + "," + entityPM.EnglishName + "," + entityPM.AssigneeNotificationTypeName + "," + entityPM.DefaultAssigneeName;

            if (isNewEntity)
            {

            }

            else
            {

            }

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(NotificationDefinitionPM entityPM, NotificationDefinition entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   