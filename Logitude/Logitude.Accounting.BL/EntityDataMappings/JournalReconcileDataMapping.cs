
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
   
   public partial class JournalReconcileDataMapping: IMapping<JournalReconcilePM, JournalReconcile>
   {

        public void CustomPMToPOCO(JournalReconcilePM entityPM, JournalReconcile entityPOCO)
        {
            //throw new NotImplementedException();

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.JournalId);
                CustomMappedPOCOProperties.Add(POCOPropertyNames.LedgerTransactionId);

                entityPOCO.Line = entityPM.Line;
                entityPOCO.JournalId = entityPM.JournalId;
                entityPOCO.LedgerTransactionId = entityPM.LedgerTransactionId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(JournalReconcilePM entityPM, JournalReconcile entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   