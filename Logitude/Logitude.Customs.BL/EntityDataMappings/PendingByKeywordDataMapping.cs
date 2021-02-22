
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class PendingByKeywordDataMapping: IMapping<PendingByKeywordPM, PendingByKeyword>
   {

        public void CustomPMToPOCO(PendingByKeywordPM entityPM, PendingByKeyword entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPM.SearchFields = entityPM.CourierPendingReasonCode + "," + entityPM.CourierPendingReasonName + "," + entityPM.KeywordsList;
        }

        public void CustomPOCOToPM(PendingByKeywordPM entityPM, PendingByKeyword entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CourierPendingReasonName);
            if (entityPOCO.CourierPendingReasonCode != null)
            {
                CourierPendingReasonQueryService courierPendingReasonQueryService = new CourierPendingReasonQueryService(entityPOCO.Tenant);
                CourierPendingReasonPM courierPendingReason = courierPendingReasonQueryService.GetSingle(entityPOCO.CourierPendingReasonCode, false, true);
                entityPM.CourierPendingReasonName = courierPendingReason.LocalName;
            }
        }
   }


}
   