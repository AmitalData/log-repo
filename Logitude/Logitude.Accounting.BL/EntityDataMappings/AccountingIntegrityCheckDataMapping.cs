
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

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class AccountingIntegrityCheckDataMapping: IMapping<AccountingIntegrityCheckPM, AccountingIntegrityCheck>
   {

        public void CustomPMToPOCO(AccountingIntegrityCheckPM entityPM, AccountingIntegrityCheck entityPOCO)
        {

        }

        public void CustomPOCOToPM(AccountingIntegrityCheckPM entityPM, AccountingIntegrityCheck entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.StatusName);

            if (entityPOCO.IntegrityCheckStatus != null)
            {
                IntegrityCheckStatusQueryService query = new IntegrityCheckStatusQueryService(entityPOCO.Tenant);

                var checkStatus = query.GetSingle(entityPOCO.StatusCode, false, false);
                if (checkStatus != null)
                {
                    entityPM.StatusName = checkStatus.Name;
                }
            }
        }
   }


}
   