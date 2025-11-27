
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
   
   public partial class SupplierInvoiceItemsReqListDataMapping: IMapping<SupplierInvoiceItemsReqListPM, SupplierInvoiceItemsReqList>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemsReqListPM entityPM, SupplierInvoiceItemsReqList entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceItemLineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SIIRequestID);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
                entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber;
                entityPOCO.LineNumber = entityPM.LineNumber;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.SIIRequestID = entityPM.SIIRequestID;
            }
        }

        public void CustomPOCOToPM(SupplierInvoiceItemsReqListPM entityPM, SupplierInvoiceItemsReqList entityPOCO)
        {
            CustomMappedPMProperties.AddRange(new[]
            {
                PMPropertyNames.ManufactureCountryName,
                PMPropertyNames.ItemNo,
                PMPropertyNames.ItemName,
                PMPropertyNames.InvoiceQuantity,
                PMPropertyNames.InvoiceQuantityType,
                PMPropertyNames.StatisticQuantity,
                PMPropertyNames.StatisticQuantityType,
                PMPropertyNames.OriginCountryCode,
                PMPropertyNames.IssueDate,
                PMPropertyNames.InvoiceNumber,
                PMPropertyNames.InvoiceQuantityTypeCode,
                PMPropertyNames.ClassificationCode,
                PMPropertyNames.StatisticQuantityTypeCode,
                PMPropertyNames.VendorName,

            });

            if (entityPOCO.ManufactureCountryCode != null)
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(entityPOCO.ManufactureCountryCode, false, true);
                entityPM.ManufactureCountryName = customsCountry.LocalName;
            }

            var ctx = CustomContext.GetContext(entityPOCO.Tenant);
            var item = ctx.SupplierInvoiceItems.FirstOrDefault(i =>
                       i.Tenant == entityPOCO.Tenant &&
                       i.DeclarationId == entityPOCO.DeclarationId &&
                       i.CounterKey == entityPOCO.InvoiceCounterKey &&
                       i.LineNumber == entityPOCO.InvoiceItemLineNumber);
            if (item == null) return;
            
            var invoice = ctx.SupplierInvoices.FirstOrDefault(si =>
            si.Tenant == entityPOCO.Tenant &&
            si.DeclarationId == entityPOCO.DeclarationId &&
            si.InvoiceCounterKey == entityPOCO.InvoiceCounterKey);

            if (invoice != null)
            {
                entityPM.IssueDate = invoice.IssueDate;
                entityPM.InvoiceNumber = invoice.InvoiceNumber;
                if (!string.IsNullOrEmpty(invoice.VendorId))
                {
                    CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(entityPOCO.Tenant);
                    CustomsVendorPM vendor = vendorQueryService.GetSingle(invoice.VendorId, false, true);
                    entityPM.VendorName = vendor?.VendorName;
                }
            }

            bool isNewEntity = string.IsNullOrWhiteSpace(entityPOCO.SIIRequestID);

            if (isNewEntity)
            {
                entityPM.ItemNo = item.ItemCode;
                entityPM.ItemName = item.ItemDescription;
            }
            else
            {
                entityPM.ItemNo = entityPOCO.ItemNo;
                entityPM.ItemName = entityPOCO.ItemName;
            }
            entityPM.InvoiceQuantity = item.InvoiceQuantity;
            entityPM.StatisticQuantity = item.StatisticQuantity;
            entityPM.OriginCountryCode = item.OriginCountryCode;
            entityPM.InvoiceQuantityTypeCode = item.InvoiceQuantityType;
            entityPM.ClassificationCode = item.ClassificationCode;
            entityPM.StatisticQuantityTypeCode = item.StatisticQuantityType;
            

            var muQS = new MeasurmentUnitQueryService(entityPOCO.Tenant);

            if (!string.IsNullOrWhiteSpace(item.InvoiceQuantityType))
            {
                entityPM.InvoiceQuantityType =
                    muQS.GetSingle(item.InvoiceQuantityType, false, true)?.LocalName;
            }

            if (!string.IsNullOrWhiteSpace(item.StatisticQuantityType))
            {
                entityPM.StatisticQuantityType =
                    muQS.GetSingle(item.StatisticQuantityType, false, true)?.LocalName;
            }
            if (!String.IsNullOrWhiteSpace(entityPOCO.StatusCode))
            {
                var sIIRequestLineStatusQueryService = new SIIRequestLineStatusQueryService(entityPOCO.Tenant);
                var pm = sIIRequestLineStatusQueryService.GetSingle(entityPOCO.StatusCode, false, true);
                entityPM.StatusName = pm?.LocalName;
            }
        }
    }


}
   