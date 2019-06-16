
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
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ReconcileExternalPageLineDataMapping: IMapping<ReconcileExternalPageLinePM, ReconcileExternalPageLine>
   {

        public void CustomPMToPOCO(ReconcileExternalPageLinePM entityPM, ReconcileExternalPageLine entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.SearchFields = entityPM.Reference + "," + entityPM.Notes + "," + entityPM.Notes;

        }

        public void CustomPOCOToPM(ReconcileExternalPageLinePM entityPM, ReconcileExternalPageLine entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ReconciliationNumber);
            CustomMappedPMProperties.Add(PMPropertyNames.Amount);

            if (entityPOCO.IsReconciled)
            {
                ExternalReconciliationLineQueryService recoLineQS = new ExternalReconciliationLineQueryService(entityPOCO.Tenant);
                ExternalReconciliationQueryService recoQS = new ExternalReconciliationQueryService(entityPOCO.Tenant);
                ExternalReconciliationLine reconciliationLine = recoLineQS.GetByBankPageLineId(entityPOCO.Id, entityPOCO.Tenant);
                ExternalReconciliationPM reco = recoQS.GetSingle(reconciliationLine.ReconciliationId, false,false);
                entityPM.ReconciliationNumber = reco.ReconciliationNumber.ToString();
            }

            entityPM.Amount = entityPOCO.CreditAmount > 0 ? entityPOCO.CreditAmount : entityPOCO.DebitAmount;

        }
    }


}
   