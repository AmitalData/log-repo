
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
   
   public partial class ExternalReconciliationLineDataMapping: IMapping<ExternalReconciliationLinePM, ExternalReconciliationLine>
   {

        public void CustomPMToPOCO(ExternalReconciliationLinePM entityPM, ExternalReconciliationLine entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                CustomMappedPOCOProperties.Add(POCOPropertyNames.GroupNumber);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.ExternalPageLineId);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.LedgerTransactionId);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.ReconciliationId);

                entityPOCO.Line = entityPM.Line;
                entityPOCO.GroupNumber = entityPM.GroupNumber;
                entityPOCO.ExternalPageLineId = entityPM.ExternalPageLineId;
                entityPOCO.LedgerTransactionId = entityPM.LedgerTransactionId;
                entityPOCO.ReconciliationId = entityPM.ReconciliationId;
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(ExternalReconciliationLinePM entityPM, ExternalReconciliationLine entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   