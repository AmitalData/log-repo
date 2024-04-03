
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
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestTransactionDataMapping: IMapping<InterestTransactionPM, InterestTransaction>
   {

        public void CustomPMToPOCO(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
           
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.InterestValueDate);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
               
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.InterestValueDate = entityPM.InterestValueDate.Date;
            }

            entityPOCO.AccountingEntityCode = entityPM.AccountingEntityCode;
        }

        public void CustomPOCOToPM(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
            entityPM.AccountingEntityCode = entityPOCO.AccountingEntityCode;
        }

    }


}
   