
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
   
   public partial class DeclarationPendingDataMapping: IMapping<DeclarationPendingPM, DeclarationPending>
   {

        public void CustomPMToPOCO(DeclarationPendingPM entityPM, DeclarationPending entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationID);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CourierPendingReasonCode);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationID = entityPM.DeclarationID;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CourierPendingReasonCode = entityPM.CourierPendingReasonCode;
                entityPOCO.Approval= (entityPM.Approval == true) ? true : false;
            }
        }

        public void CustomPOCOToPM(DeclarationPendingPM entityPM, DeclarationPending entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CourierPendingReasonName);
            
            if (entityPOCO.CourierPendingReasonCode != null)
            {
                CourierPendingReasonQueryService courierPendingReasonQueryService = new CourierPendingReasonQueryService(entityPOCO.Tenant);
                CourierPendingReasonPM constraintType = courierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(entityPOCO.CourierPendingReasonCode, entityPOCO.Tenant);
                entityPM.CourierPendingReasonName = constraintType.LocalName;
                entityPM.CourierPendingRequireApr = constraintType.RequiresApproval;
            }
            entityPM.WasApproved = (entityPM.Approval==true)?true:false;
            entityPM.Approval = (entityPM.Approval == true) ? true : false;

        }
    }


}
   