
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
   
   public partial class DeclarationReferantDataDataMapping: IMapping<DeclarationReferantDataPM, DeclarationReferantData>
   {

        public void CustomPMToPOCO(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
        {
            entityPOCO.DeclarationId = entityPM.DeclarationId;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private static void BuildSearchFields(DeclarationReferantDataPM entityPM, DeclarationReferantData poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.OrderNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.OrderNumber : result + "," + entityPM.OrderNumber;
            }

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
    }


}
   