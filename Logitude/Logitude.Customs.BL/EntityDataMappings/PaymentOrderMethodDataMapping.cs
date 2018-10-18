
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
   
   public partial class PaymentOrderMethodDataMapping: IMapping<PaymentOrderMethodPM, PaymentOrderMethod>
   {

        public void CustomPMToPOCO(PaymentOrderMethodPM entityPM, PaymentOrderMethod entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.PaymentOrderId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
               
                entityPOCO.PaymentOrderId = entityPM.PaymentOrderId;              
                entityPOCO.Line = entityPM.Line;
            }
        }

        public void CustomPOCOToPM(PaymentOrderMethodPM entityPM, PaymentOrderMethod entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.BankName);
            CustomMappedPMProperties.Add(PMPropertyNames.PaymentMethodStatusName);
            CustomMappedPMProperties.Add(PMPropertyNames.TypeName);

            if (entityPOCO.BankCode != null)
            {
                BankQueryService bankQueryService = new BankQueryService(entityPOCO.Tenant);
                BankPM bank = bankQueryService.GetSingle(entityPOCO.BankCode, false, false);
                entityPM.BankName = bank.LocalName;
            }

            if (entityPOCO.TypeCode != null)
            {
                PaymentMethodTypeQueryService paymentMethodTypeQueryService = new PaymentMethodTypeQueryService(entityPOCO.Tenant);
                PaymentMethodTypePM paymentMethodType = paymentMethodTypeQueryService.GetSingle(entityPOCO.TypeCode, false, true);
                entityPM.TypeName = paymentMethodType.LocalName;
            }

            if (entityPOCO.PaymentMethodStatusCode != null)
            {
                PaymentMethodStatusQueryService paymentMethodStatusQueryService = new PaymentMethodStatusQueryService(entityPOCO.Tenant);
                PaymentMethodStatusPM paymentMethodStatus = paymentMethodStatusQueryService.GetSingle(entityPOCO.PaymentMethodStatusCode, false, true);
                entityPM.PaymentMethodStatusName = paymentMethodStatus.LocalName;
            }

            if (entityPOCO.CustomerActivityTypeCode != null)
            {
                CustomerActivityTypeQueryService customerActivityTypeQueryService = new CustomerActivityTypeQueryService(entityPOCO.Tenant);
                CustomerActivityTypePM customerActivityTypePM = customerActivityTypeQueryService.GetSingle(entityPOCO.CustomerActivityTypeCode, false, true);
                entityPM.CustomerActivityTypeName = customerActivityTypePM.LocalName;
            }
        }
   }


}
   