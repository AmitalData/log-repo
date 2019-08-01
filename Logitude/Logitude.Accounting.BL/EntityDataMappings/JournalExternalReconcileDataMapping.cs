
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
   
   public partial class JournalExternalReconcileDataMapping: IMapping<JournalExternalReconcilePM, JournalExternalReconcile>
   {

        public void CustomPMToPOCO(JournalExternalReconcilePM entityPM, JournalExternalReconcile entityPOCO)
        {
            //throw new NotImplementedException();
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.JournalId);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.LedgerTransactionId);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.ReconcileExternalPageLineId);

                entityPOCO.Line = entityPM.Line;
                entityPOCO.JournalId = entityPM.JournalId;
                entityPOCO.LedgerTransactionId = entityPM.LedgerTransactionId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ReconcileExternalPageLineId = entityPM.ReconcileExternalPageLineId;
            }
        }

        public void CustomPOCOToPM(JournalExternalReconcilePM entityPM, JournalExternalReconcile entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   