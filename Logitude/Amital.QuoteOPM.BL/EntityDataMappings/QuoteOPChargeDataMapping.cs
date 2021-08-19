
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;
using Simplog.Data.Helpers;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPChargeDataMapping: IMapping<QuoteOPChargePM, QuoteOPCharge>
   {

        public void CustomPMToPOCO(QuoteOPChargePM entityPM, QuoteOPCharge entityPOCO)
        {
            //throw new NotImplementedException();

            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuoteOPId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.QuoteOPId = entityPM.QuoteOPId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.ValueDate = TenantServerConfigration.GetCurrentDateTime(entityPOCO.Tenant);
            entityPOCO.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPOCO.Tenant);

        }

        public void CustomPOCOToPM(QuoteOPChargePM entityPM, QuoteOPCharge entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   