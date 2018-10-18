
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
   
   public partial class DeclarationPaymentProtestDataMapping: IMapping<DeclarationPaymentProtestPM, DeclarationPaymentProtest>
   {

        public void CustomPMToPOCO(DeclarationPaymentProtestPM entityPM, DeclarationPaymentProtest entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
               
                entityPOCO.DeclarationId = entityPM.DeclarationId;              
                entityPOCO.Line = entityPM.Line;
            }
        }

        public void CustomPOCOToPM(DeclarationPaymentProtestPM entityPM, DeclarationPaymentProtest entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ProtestTypeName);

            if (entityPOCO.ProtestTypeCode != null)
            {
                PaymentProtestTypeQueryService paymentProtestTypeQueryService = new PaymentProtestTypeQueryService(entityPOCO.Tenant);
                PaymentProtestTypePM paymentProtestType = paymentProtestTypeQueryService.GetSingle(entityPOCO.ProtestTypeCode, false, true);
                entityPM.ProtestTypeName = paymentProtestType.LocalName;
            }

            SupplierInvoiceQueryService invoiceQuery = new SupplierInvoiceQueryService(entityPOCO.Tenant);
            SupplierInvoicePM invoice =    invoiceQuery.GetSupplierInvoiceByNumber(entityPOCO.InvoiceNumber, entityPOCO.Tenant);
            if (invoice != null)
            {
                SupplierInvoiceItemPM invoiceItem = invoice.SupplierInvoiceItems.Where(d => d.SequenceNumeric == entityPOCO.GoodsItemLineNumber).FirstOrDefault();
                if (invoiceItem != null)
                {
                    entityPM.InvoiceItemClassificationCode = invoiceItem.ClassificationCode;
                }
            }
        }
   }


}
   