
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
   
   public partial class CourierPendingReasonDataMapping: IMapping<CourierPendingReasonPM, CourierPendingReason>
   {

        public void CustomPMToPOCO(CourierPendingReasonPM entityPM, CourierPendingReason entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
            entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.Code;
        }

        public void CustomPOCOToPM(CourierPendingReasonPM entityPM, CourierPendingReason entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ErrorPlaceName);

            if (entityPOCO.ErrorPlace != null)
            {
                PendingErrorPlaceQueryService pendingErrorPlaceQueryService = new PendingErrorPlaceQueryService(entityPOCO.Tenant);
                PendingErrorPlacePM pendingErrorPlacePM = pendingErrorPlaceQueryService.GetSingle(entityPOCO.ErrorPlace, false, true);
                entityPM.ErrorPlaceName = pendingErrorPlacePM.LocalName;
            }
        }
   }


}
   