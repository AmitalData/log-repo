
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
   
   public partial class SupplierInvoiceItemsPriceDataMapping: IMapping<SupplierInvoiceItemsPricePM, SupplierInvoiceItemsPrice>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemsPricePM entityPM, SupplierInvoiceItemsPrice entityPOCO)
        {
            entityPOCO.DeclarationId = entityPM.DeclarationId;
            entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
            entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber;
            entityPOCO.LineNumber = entityPM.LineNumber;
         }

        public void CustomPOCOToPM(SupplierInvoiceItemsPricePM entityPM, SupplierInvoiceItemsPrice entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.AdditionalPriceTypeName);
            if (entityPOCO.AdditionalPriceTypeCode != null)
            {
                AmountTypeQueryService amountTypeQueryService = new AmountTypeQueryService(entityPOCO.Tenant);
                AmountTypePM amountTypePM = amountTypeQueryService.GetSingle(entityPOCO.AdditionalPriceTypeCode, false, true);
                entityPM.AdditionalPriceTypeName = amountTypePM.LocalName;
            }


        }
    }


}
   