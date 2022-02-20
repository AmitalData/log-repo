
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
   
   public partial class SupplierInvoiceFreightAmountDataMapping: IMapping<SupplierInvoiceFreightAmountPM, SupplierInvoiceFreightAmount>
   {

        public void CustomPMToPOCO(SupplierInvoiceFreightAmountPM entityPM, SupplierInvoiceFreightAmount entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CurrencyTypeCode);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {              
                entityPOCO.Id = entityPM.Id;             
                entityPOCO.DeclarationId = entityPM.DeclarationId;             
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;              
                entityPOCO.CurrencyTypeCode = entityPM.CurrencyTypeCode;               
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(SupplierInvoiceFreightAmountPM entityPM, SupplierInvoiceFreightAmount entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CurrencyTypeName);

            if (entityPOCO.CurrencyTypeCode != null)
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(entityPOCO.Tenant);
                CurrencyTypePM currency = currencyTypeQueryService.GetSingle(entityPOCO.CurrencyTypeCode, false, true);
                entityPM.CurrencyTypeName = currency.LocalName;
            }
        }
   }
}
   