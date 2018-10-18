
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DeclarationPaymentDataMapping: IMapping<DeclarationPaymentPM, DeclarationPayment>
   {

        public void CustomPMToPOCO(DeclarationPaymentPM entityPM, DeclarationPayment entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
           
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
              
                entityPOCO.DeclarationId = entityPM.DeclarationId;            
                entityPOCO.Tenant = entityPM.Tenant;
            }
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;//Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;   
        }

        public void CustomPOCOToPM(DeclarationPaymentPM entityPM, DeclarationPayment entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.NewConcurrencyGUID);
            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();

           
          

        }
   }


}
   