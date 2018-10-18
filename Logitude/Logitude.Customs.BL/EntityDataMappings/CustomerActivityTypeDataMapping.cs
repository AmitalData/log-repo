
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
   
   public partial class CustomerActivityTypeDataMapping: IMapping<CustomerActivityTypePM, CustomerActivityType>
   {

        public void CustomPMToPOCO(CustomerActivityTypePM entityPM, CustomerActivityType entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private static void BuildSearchFields(CustomerActivityTypePM entityPM, CustomerActivityType poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.Code + "," + entityPM.LocalName;

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(CustomerActivityTypePM entityPM, CustomerActivityType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   