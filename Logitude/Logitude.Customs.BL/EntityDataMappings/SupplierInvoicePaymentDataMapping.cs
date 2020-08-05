
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
   
   public partial class SupplierInvoicePaymentDataMapping: IMapping<SupplierInvoicePaymentPM, SupplierInvoicePayment>
   {

        public void CustomPMToPOCO(SupplierInvoicePaymentPM entityPM, SupplierInvoicePayment entityPOCO)
        {
            entityPOCO.DeclarationId = entityPM.DeclarationId;
            entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
            entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
         }

        public void CustomPOCOToPM(SupplierInvoicePaymentPM entityPM, SupplierInvoicePayment entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.PaymentTypeName);

            if (!string.IsNullOrWhiteSpace(entityPOCO.PaymentTypeCode))
            {
                PaymentTypeQueryService paymentTypeQueryService = new PaymentTypeQueryService(entityPOCO.Tenant);
                PaymentTypePM paymentTypePM = paymentTypeQueryService.GetSingle(entityPOCO.PaymentTypeCode, false, true);
                entityPM.PaymentTypeName = paymentTypePM.LocalName;
            }
            //throw new NotImplementedException();
        }
   }


}
   