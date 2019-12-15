
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestTransactionDataMapping: IMapping<InterestTransactionPM, InterestTransaction>
   {

        public void CustomPMToPOCO(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
           
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.Id);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
               
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;

            }
        }

        public void CustomPOCOToPM(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   