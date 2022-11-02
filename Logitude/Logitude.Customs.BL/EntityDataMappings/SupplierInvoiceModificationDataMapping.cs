
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
   
   public partial class SupplierInvoiceModificationDataMapping: IMapping<SupplierInvoiceModificationPM, SupplierInvoiceModification>
   {

        public void CustomPMToPOCO(SupplierInvoiceModificationPM entityPM, SupplierInvoiceModification entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.TypeCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ModificationCounterKey);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
              
                entityPOCO.DeclarationId = entityPM.DeclarationId;                
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;              
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ModificationCounterKey = entityPM.ModificationCounterKey;
            }

            //entityPOCO.TypeCode = entityPM.TypeCode;             

        }

        public void CustomPOCOToPM(SupplierInvoiceModificationPM entityPM, SupplierInvoiceModification entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.TypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.CurrencyTypeName);
            if (entityPOCO.TypeCode != null)
            {
                ModificationAndDiscountTypeQueryService modificationAndDiscountTypeQueryService = new ModificationAndDiscountTypeQueryService(entityPOCO.Tenant);
                ModificationAndDiscountTypePM modificationAndDiscountType = modificationAndDiscountTypeQueryService.GetSingle(entityPOCO.TypeCode, false, true);
                entityPM.TypeName = modificationAndDiscountType.LocalName;
                entityPM.ModificationAffectTypeID = modificationAndDiscountType.NetoValuesModificationAffectID;
            }
            if (entityPOCO.CurrencyTypeCode != null)
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(entityPOCO.Tenant);
                CurrencyTypePM currencyType = currencyTypeQueryService.GetSingle(entityPOCO.CurrencyTypeCode, false, true);
                entityPM.CurrencyTypeName = currencyType.LocalName;
            }
        }
   }


}
   