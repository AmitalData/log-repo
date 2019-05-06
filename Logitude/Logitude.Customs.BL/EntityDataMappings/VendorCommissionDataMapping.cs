
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
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class VendorCommissionDataMapping: IMapping<VendorCommissionPM, VendorCommission>
   {

        public void CustomPMToPOCO(VendorCommissionPM entityPM, VendorCommission entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.VendorId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CustomerId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ModificationsTypeCode);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.VendorId = entityPM.VendorId;
                entityPOCO.CustomerId = entityPM.CustomerId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ModificationsTypeCode = entityPM.ModificationsTypeCode;

            }
        }

        public void CustomPOCOToPM(VendorCommissionPM entityPM, VendorCommission entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   