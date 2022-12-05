
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
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvoiceItemDataMapping: IMapping<SupplierInvoiceItemPM, SupplierInvoiceItem>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemPM entityPM, SupplierInvoiceItem entityPOCO)
        {

            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CounterKey);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.CounterKey = entityPM.CounterKey;               
                entityPOCO.LineNumber = entityPM.LineNumber;            
                entityPOCO.Tenant = entityPM.Tenant;

            }
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;

        }
        private static void BuildSearchFields(SupplierInvoiceItemPM entityPM, SupplierInvoiceItem poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.ClassificationCode))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ClassificationCode : result + "," + entityPM.ClassificationCode;
            }

            if (!string.IsNullOrEmpty(entityPM.ItemCode))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ItemCode : result + "," + entityPM.ItemCode;
            }
           
         

         
            

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(SupplierInvoiceItemPM entityPM, SupplierInvoiceItem entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.OriginCountryName);
            CustomMappedPMProperties.Add(PMPropertyNames.TradeAgreementName);
            CustomMappedPMProperties.Add(PMPropertyNames.InvoiceQuantityTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.StatisticQuantityTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.AdditionalQuantityTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.TaxExemptName);
          



            if (entityPOCO.OriginCountryCode != null)
            {
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPOCO.OriginCountryCode, false, true);
                if(country != null)
                   entityPM.OriginCountryName = country.LocalName;
            }

            if (entityPOCO.TradeAgreementCode != null)
            {
                TradeAgreementQueryService tradeAgreementQueryService = new TradeAgreementQueryService(entityPOCO.Tenant);
                TradeAgreementPM tradeAgreement = tradeAgreementQueryService.GetSingle(entityPOCO.TradeAgreementCode, false, true);
                entityPM.TradeAgreementName = tradeAgreement.LocalName;
            }

            if (entityPOCO.InvoiceQuantityType != null)
            {
                MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(entityPOCO.Tenant);
                MeasurmentUnitPM measurmentUnit = measurmentUnitQueryService.GetSingle(entityPOCO.InvoiceQuantityType, false, true);
                entityPM.InvoiceQuantityTypeName = measurmentUnit.LocalName;
            }

            if (entityPOCO.StatisticQuantityType != null)
            {
                MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(entityPOCO.Tenant);
                MeasurmentUnitPM measurmentUnit = measurmentUnitQueryService.GetSingle(entityPOCO.StatisticQuantityType, false, true);
                entityPM.StatisticQuantityTypeName = measurmentUnit.LocalName;
            }

            if (entityPOCO.AdditionalQuantityType != null)
            {
                MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(entityPOCO.Tenant);
                MeasurmentUnitPM measurmentUnit = measurmentUnitQueryService.GetSingle(entityPOCO.AdditionalQuantityType, false, true);
                entityPM.AdditionalQuantityTypeName = measurmentUnit.LocalName;
            }
            if (entityPOCO.ClassificationCode != null)
            {
                entityPM.ClassificationCodeSource = entityPOCO.ClassificationCode;
            }

            if (!string.IsNullOrWhiteSpace(entityPOCO.DutyRegimeProtocolCode))
                entityPM.DutyRegimeProtocolLocalName = new TradeAgreementProtocolQueryService(entityPM.Tenant).GetSingle(entityPOCO.DutyRegimeProtocolCode, false, true)?.LocalName;

            //if (entityPOCO.TaxExemptCode != null)
            //{
            //    ValidCustomsItemQueryService validCustomsItemQueryService = new ValidCustomsItemQueryService(entityPOCO.Tenant);
            //    ValidCustomsItemPM validCustomsItem = validCustomsItemQueryService.GetSingle(entityPOCO.TaxExemptCode, false, true);
            //    entityPM.TaxExemptName = validCustomsItem.LocalName;
            //}
        }

        public string CalcHash(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var SIItemPOCOFields = Enum.GetNames(typeof(SupplierInvoiceItemDataMapping.POCOPropertyNames)).ToList(); ;
            SIItemPOCOFields.Remove("SequenceNumeric");
            SIItemPOCOFields.Remove("LineNumber");
            SIItemPOCOFields.Remove("ItemCode");
            SIItemPOCOFields.Remove("ItemDescription");
            SIItemPOCOFields.Remove("ItemPrice");
            SIItemPOCOFields.Remove("StatisticQuantity");
            SIItemPOCOFields.Remove("InvoiceQuantity");
            SIItemPOCOFields.Remove("ItemHash");
            SIItemPOCOFields.Remove("ParentLineNumber");
            SIItemPOCOFields.Remove("NotForAccumaltion");
            SIItemPOCOFields.Remove("IsParent");
            SIItemPOCOFields.Remove("LastCopyFromOrderNo");
            SIItemPOCOFields.Remove("OrderByLineNo");
            SIItemPOCOFields.Remove("UnfInvoiceLine");

            var SIItemCerPOCOFields = Enum.GetNames(typeof(SupplierInvioceItemCertificatDataMapping.POCOPropertyNames)).ToList(); ;
            SIItemCerPOCOFields.Remove("LineNumber");
            SIItemCerPOCOFields.Remove("ItemCertificateCounterKey");
            SIItemCerPOCOFields.Remove("SequenceNumeric");

            var myHashCode = supplierInvoiceItemPM.GetHashCode4Accumulation(SIItemPOCOFields, SIItemCerPOCOFields);

            return myHashCode.ToString();

        }
    }


}
   