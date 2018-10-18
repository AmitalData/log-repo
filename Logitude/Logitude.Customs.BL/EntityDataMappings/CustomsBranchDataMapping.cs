
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
   
   public partial class CustomsBranchDataMapping: IMapping<CustomsBranchPM, CustomsBranch>
   {

        public void CustomPMToPOCO(CustomsBranchPM entityPM, CustomsBranch entityPOCO)
        {
            //throw new NotImplementedException();
            AddPOCOPropertyName(POCOPropertyNames.Id);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private static void BuildSearchFields(CustomsBranchPM entityPM, CustomsBranch poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.Code + "," + entityPM.BankCode + "," + entityPM.LocalName;

            if (isNewEntity)
            {

            }

            else
            {

            }

            entityPM.SearchFields = result.ToLower(); ;
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(CustomsBranchPM entityPM, CustomsBranch entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   