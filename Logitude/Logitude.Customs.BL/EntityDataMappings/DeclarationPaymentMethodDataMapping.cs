
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
   
   public partial class DeclarationPaymentMethodDataMapping: IMapping<DeclarationPaymentMethodPM, DeclarationPaymentMethod>
   {

        public void CustomPMToPOCO(DeclarationPaymentMethodPM entityPM, DeclarationPaymentMethod entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
               
                entityPOCO.DeclarationId = entityPM.DeclarationId;             
                entityPOCO.Line = entityPM.Line;
            }
        }

        public void CustomPOCOToPM(DeclarationPaymentMethodPM entityPM, DeclarationPaymentMethod entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.MethodTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.PayerActivityTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.InternalBankName);

            if (entityPOCO.MethodTypeCode != null)
            {
                PaymentMethodTypeQueryService paymentMethodTypeQueryService = new PaymentMethodTypeQueryService(entityPOCO.Tenant);
                PaymentMethodTypePM paymentMethodType = paymentMethodTypeQueryService.GetSingle(entityPOCO.MethodTypeCode, false, true);
                entityPM.MethodTypeName = paymentMethodType.LocalName;
            }

            if (entityPOCO.PayerActivityTypeCode != null)
            {
                CustomerActivityTypeQueryService customerActivityTypeQueryService = new CustomerActivityTypeQueryService(entityPOCO.Tenant);
                CustomerActivityTypePM customerActivityTypePM = customerActivityTypeQueryService.GetSingle(entityPOCO.PayerActivityTypeCode, false, true);
                entityPM.PayerActivityTypeName = customerActivityTypePM.LocalName;
            }

            if (entityPOCO.InternalBankId != null)
            {
                CustomBankQueryService customBankQueryService = new CustomBankQueryService(entityPOCO.Tenant);
                CustomBankPM customBank = customBankQueryService.GetSingle(entityPOCO.InternalBankId, false, true);
                entityPM.InternalBankName = customBank.LocalName;
            }

        }
   }


}
   