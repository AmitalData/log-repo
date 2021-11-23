using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure.Helpers;
namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemQueryService : EntityQueryService<SupplierInvoiceItem, SupplierInvoiceItemKeys, SupplierInvoiceItemPM, SupplierInvoicePM, SupplierInvoiceKeys> 
    {
        public bool LoadComposition { get; set; }
        public override void GetComposition(EntityKeyFields entityKeys,SupplierInvoiceItemPM entityPM)
        {
            if (!LoadComposition)
            {
                return;
            }
            ICustomContext context = MainContext as CustomContext;
            SupplierInvoiceItemKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceItemKeys;
            //SupplierInvoiceItemsQuantityQueryService supplierInvoiceItemsQuantityQueryService = new SupplierInvoiceItemsQuantityQueryService(context);
            //entityPM.SupplierInvoiceItemsQuantities = supplierInvoiceItemsQuantityQueryService.GetMulti(supplierInvoiceItemKeys, false);
            SupplierInvoiceItemsConDeclarQueryService supplierInvoiceItemsConnectedDeclarationService = new SupplierInvoiceItemsConDeclarQueryService(context);
            entityPM.SupplierInvoiceItemsConDeclars = supplierInvoiceItemsConnectedDeclarationService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemsModQueryService supplierInvoiceItemsModificationQueryService = new SupplierInvoiceItemsModQueryService(context);
            entityPM.SupplierInvoiceItemsMods = supplierInvoiceItemsModificationQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemsTaxQueryService supplierInvoiceItemsTaxQueryService = new SupplierInvoiceItemsTaxQueryService(context);
            entityPM.SupplierInvoiceItemTaxes = supplierInvoiceItemsTaxQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvioceItemCertificatQueryService supplierInvioceItemsCertificateQueryService = new SupplierInvioceItemCertificatQueryService(context);
            entityPM.SupplierInvioceItemCertificats = supplierInvioceItemsCertificateQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemsSerialNumQueryService supplierInvoiceItemsSerialNumberQueryService = new SupplierInvoiceItemsSerialNumQueryService(context);
            entityPM.SupplierInvoiceItemsSerialNums = supplierInvoiceItemsSerialNumberQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemsProdIdentQueryService supplierInvoiceItemsProductIdentificationQueryService = new SupplierInvoiceItemsProdIdentQueryService(context);
            entityPM.SupplierInvoiceItemsProdIdents = supplierInvoiceItemsProductIdentificationQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemsDescriptQueryService supplierInvoiceItemsDescriptionQueryService = new SupplierInvoiceItemsDescriptQueryService(context);
            entityPM.SupplierInvoiceItemsDescripts = supplierInvoiceItemsDescriptionQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemProcesTypeQueryService supplierInvoiceItemsProcessTypeQueryService = new SupplierInvoiceItemProcesTypeQueryService(context);
            entityPM.SupplierInvoiceItemProcesTypes = supplierInvoiceItemsProcessTypeQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemsLevyQueryService supplierInvoiceItemsLevyQueryService = new SupplierInvoiceItemsLevyQueryService(context);
            entityPM.SupplierInvoiceItemLevies = supplierInvoiceItemsLevyQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemVehicleQueryService supplierInvoiceItemVehicleQueryService = new SupplierInvoiceItemVehicleQueryService(context);
            entityPM.SupplierInvoiceItemVehicles = supplierInvoiceItemVehicleQueryService.GetMulti(supplierInvoiceItemKeys, true);
            SupplierInvoiceItemModVehicleQueryService supplierInvoiceItemModVehicleQueryService = new SupplierInvoiceItemModVehicleQueryService(context);
            entityPM.SupplierInvoiceItemModVehicles = supplierInvoiceItemModVehicleQueryService.GetMulti(supplierInvoiceItemKeys, true);


            if (entityPM.SupplierInvoiceItemTaxes.Count > 0)
            {
                foreach (SupplierInvoiceItemsTaxPM item in entityPM.SupplierInvoiceItemTaxes)
                {
                    entityPM.SupplierInvoiceTaxesActiveIds = entityPM.SupplierInvoiceTaxesActiveIds + "," + item.TaxTypeCode;
                }
                entityPM.SupplierInvoiceTaxesActiveIds = entityPM.SupplierInvoiceTaxesActiveIds.TrimStart(',');
            }
            else
            {
                entityPM.SupplierInvoiceTaxesActiveIds = "";
            }

            //if (entityPM.SupplierInvoiceItemsQuantities != null)
            //{
            //    if (entityPM.SupplierInvoiceItemsQuantities.Count > 0)
            //    {
            //        entityPM.SupplierInvoiceItemsQuantityLastLineNumber = entityPM.SupplierInvoiceItemsQuantities.Max(m => m.LineNumber);
            //    }
            //}

            if (entityPM.SupplierInvoiceItemsConDeclars != null)
            {
                if (entityPM.SupplierInvoiceItemsConDeclars.Count > 0)
                {
                    entityPM.SupplierInvoiceItemsConnectedDeclarationLastLineNumber = entityPM.SupplierInvoiceItemsConDeclars.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemsMods != null)
            {
                if (entityPM.SupplierInvoiceItemsMods.Count > 0)
                {
                    entityPM.SupplierInvoiceItemsModificationLastLineNumber = entityPM.SupplierInvoiceItemsMods.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemTaxes != null)
            {
                if (entityPM.SupplierInvoiceItemTaxes.Count > 0)
                {
                    entityPM.SupplierInvoiceItemTaxLastLineNumber = entityPM.SupplierInvoiceItemTaxes.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvioceItemCertificats != null)
            {
                if (entityPM.SupplierInvioceItemCertificats.Count > 0)
                {
                    entityPM.SupplierInvioceItemsCertificatLastLineNumber = entityPM.SupplierInvioceItemCertificats.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemsSerialNums != null)
            {
                if (entityPM.SupplierInvoiceItemsSerialNums.Count > 0)
                {
                    entityPM.SupplierInvoiceItemsSerialNumberLastLineNumber = entityPM.SupplierInvoiceItemsSerialNums.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemsProdIdents != null)
            {
                if (entityPM.SupplierInvoiceItemsProdIdents.Count > 0)
                {
                    entityPM.SupplierInvoiceItemsProductIdentificationLastLineNumber = entityPM.SupplierInvoiceItemsProdIdents.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemsDescripts != null)
            {
                if (entityPM.SupplierInvoiceItemsDescripts.Count > 0)
                {
                    entityPM.SupplierInvoiceItemsDescriptionLastLineNumber = entityPM.SupplierInvoiceItemsDescripts.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemProcesTypes != null)
            {
                if (entityPM.SupplierInvoiceItemProcesTypes.Count > 0)
                {
                    entityPM.SupplierInvoiceItemsProcessTypeLastLineNumber = entityPM.SupplierInvoiceItemProcesTypes.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemLevies != null)
            {
                if (entityPM.SupplierInvoiceItemLevies.Count > 0)
                {
                    entityPM.SupplierInvoiceItemsLevyLastLineNumber = entityPM.SupplierInvoiceItemLevies.Max(m => m.LineNumber);
                }
            }

            if (entityPM.SupplierInvoiceItemVehicles != null)
            {
                if (entityPM.SupplierInvoiceItemVehicles.Count > 0)
                {
                    entityPM.SupplierInvoiceItemVehicleLastLineNumber = entityPM.SupplierInvoiceItemVehicles.Max(m => m.LineNumber);
                }
            }


            base.GetComposition(entityKeys,entityPM);
        }

        internal List<SupplierInvoiceItemPM> GetMultiOnlyParentItem(SupplierInvoiceKeys supplierInvoiceKeys)
        {
            var pocos= repository.GetMulti(supplierInvoiceKeys,true);
            var pms = pocos.Select(p => this.GetEntityPM(p)).ToList();
            return pms;
        }

        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemsByCounterKeys(string declarationId, List<int> counterKeys,List<int> lineNumbers, int tenant)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemsByCounterKeys(declarationId, counterKeys,lineNumbers, tenant);
            return (from a in supplierInvoiceItems
                    select new SupplierInvoiceItemPM()
                    {
                        DeclarationId = a.DeclarationId,
                        CounterKey = a.CounterKey,
                        LineNumber=a.LineNumber,
                        SequenceNumeric = a.SequenceNumeric,
                    }).ToList();
        }


        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemsByParent(string declarationId, int counterKey, int parentLineNumber, int tenant)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemsByParent(declarationId, counterKey, parentLineNumber, tenant);
            

            return (from a in supplierInvoiceItems
                    select new SupplierInvoiceItemPM()
                    {
                        DeclarationId = a.DeclarationId,
                        CounterKey = a.CounterKey,
                        LineNumber = a.LineNumber,
                        SequenceNumeric = a.SequenceNumeric,
                        ItemCode = a.ItemCode,
                        ClassificationCode = a.ClassificationCode,
                        OriginCountryName = a.OriginCountry != null ? a.OriginCountry.LocalName : null,
                        OriginCountryCode = a.OriginCountryCode,
                        TradeAgreementCode = a.TradeAgreementCode,
                        TradeAgreementName = a.TradeAgreement != null ? a.TradeAgreement.LocalName : null,
                        ItemPrice = a.ItemPrice,
                        InvoiceQuantity = a.InvoiceQuantity,
                        AdditionalQuantity = a.AdditionalQuantity,
                        AdditionalQuantityType = a.AdditionalQuantityType,
                        CertificatesStatusCode = a.CertificatesStatusCode,
                        CustomsBookTypeCode = a.CustomsBookTypeCode,
                        DeferredCustomsTax = a.DeferredCustomsTax,
                        DangerousClassificationCode = a.DangerousClassificationCode,
                        DeferredPurchaseTax = a.DeferredPurchaseTax,
                        InvoiceQuantityType = a.InvoiceQuantityType,
                        ItemDescription = a.ItemDescription,
                        ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                        ManufactureIdentifier = a.ManufactureIdentifier,
                        DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                        OptionalTamaPercentage = a.OptionalTamaPercentage,
                        IsUsed = a.IsUsed,
                        NonCustomsItemPrice = a.NonCustomsItemPrice,
                        NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                        PreferenceDocumentNumber = a.PreferenceDocumentNumber,
                        SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                        StatisticQuantity = a.StatisticQuantity,
                        StatisticQuantityType = a.StatisticQuantityType,
                        TaxExemptCode = a.TaxExemptCode,
                        WholeSaleItemPrice = a.WholeSaleItemPrice,
                        WholeSaleItemPriceCurrencyCode = a.WholeSaleItemPriceCurrencyCode,
                        AdditionalQuantityTypeName = a.AdditionalMeasurmentUnit != null ? a.AdditionalMeasurmentUnit.LocalName : null,
                        InvoiceQuantityTypeName = a.InvoiceMeasurmentUnit != null ? a.InvoiceMeasurmentUnit.LocalName : null,
                        StatisticQuantityTypeName = a.StatisticMeasurmentUnit != null ? a.StatisticMeasurmentUnit.LocalName : null,
                        ParentLineNumber = a.ParentLineNumber,
                        IsParent = a.IsParent
                    }).ToList();
        }

        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemsByParentFullPM(string declarationId, int counterKey, int parentLineNumber, int tenant)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemsByParent(declarationId, counterKey, parentLineNumber, tenant).ToList();
            return supplierInvoiceItems.Select(rec => this.GetEntityPM(rec)).ToList();
        }

        public int GetSupplierInvoiceItemsCount(string declarationId, int counterKey, int tenant)
        {
           int itemscount = repository.GetSupplierInvoiceItemsCountByInvoice(declarationId, counterKey);
            return itemscount;
        }



        public int GetDeclarationCountOfSupplierInvoiceItems(int tenant ,string declarationId,bool? SuppressIsParent=null)
        {
            return repository.GetDeclarationCountOfSupplierInvoiceItems(tenant, declarationId, SuppressIsParent);
            
        }

        public int GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(int tenant, string declarationId, List<int> invoicesCounterKeys)
        {

            return repository.GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(tenant, declarationId, invoicesCounterKeys);

        }

        public int ExistSupplierInvoiceItemsWithParent(int tenant, string declarationId)
        {
            return repository.ExistSupplierInvoiceItemsWithParent(tenant, declarationId);

        }

        public int ExistSupplierInvoiceItemsWithoutHash(int tenant, string declarationId)
        {
            return repository.ExistSupplierInvoiceItemsWithoutHash(tenant, declarationId);

        }

        public int ExistSupplierInvoiceItemsWithoutHashForAccumulation(int tenant, string declarationId, List<int> invoicesCounterKeys)
        {
            return repository.ExistSupplierInvoiceItemsWithoutHashForAccumulation(tenant, declarationId, invoicesCounterKeys);

        }

        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemsByInvoice(string declarationId, int invoiceCounterKey)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemsByInvoice(declarationId, invoiceCounterKey);
            return (from a in supplierInvoiceItems
                    select new SupplierInvoiceItemPM()
                    {
                        DeclarationId = a.DeclarationId,
                        CounterKey = a.CounterKey,
                        LineNumber = a.LineNumber,
                        SequenceNumeric = a.SequenceNumeric,
                        Tenant = a.Tenant,
                    }).ToList();

        }

        public List<SupplierInvoiceItemPM> GetSomeSupplierInvoiceItemsForInvoice(bool IsAccumulated, string declarationId, int invoiceCounterKey, int skip, int take,string type, ref int fullCount, ref int childrenCount)
        {
            fullCount = repository.GetSupplierInvoiceItemsCountForInvoice(declarationId, invoiceCounterKey);
            childrenCount = repository.GetChildrenCountForInvoice(declarationId, invoiceCounterKey);


            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSomeSupplierInvoiceItemsForInvoice(IsAccumulated,declarationId, invoiceCounterKey,skip,take, type);
            List<SupplierInvoiceItemPM> supplierInvoiceItemPMs = new List<SupplierInvoiceItemPM>();
            SupplierInvoiceItemDataMapping dataMapping = new SupplierInvoiceItemDataMapping();
            foreach (SupplierInvoiceItem item in supplierInvoiceItems)
            {
                SupplierInvoiceItemPM itemPM = new SupplierInvoiceItemPM();
                dataMapping.CustomPOCOToPM(itemPM, item);
                dataMapping.POCOToPM(itemPM, item);
                supplierInvoiceItemPMs.Add(itemPM);
            }
            return supplierInvoiceItemPMs;
        }

        public int GetMaxLineNumber(string declarationId, int invoiceCounterKey)
        {
            return repository.GetMaxLineNumber(declarationId, invoiceCounterKey);

            
        }

        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemsForMultiUpdate(string declarationId, int tenant)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemsForDeclaration(declarationId, tenant);
            return supplierInvoiceItems.Select(rec => this.GetEntityPM(rec)).ToList();
        }

        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemsForDeclaration(string declarationId, int tenant)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemsForDeclaration(declarationId, tenant);
            return (from a in supplierInvoiceItems
                    select new SupplierInvoiceItemPM()
                    {
                        DeclarationId = a.DeclarationId,
                        CounterKey = a.CounterKey,
                        Tenant=a.Tenant,
                        LineNumber = a.LineNumber,
                        SequenceNumeric = a.SequenceNumeric,
                        ItemCode = a.ItemCode,
                        ClassificationCode = a.ClassificationCode,
                        OriginCountryName = a.OriginCountry != null ? a.OriginCountry.LocalName : null,
                        OriginCountryCode = a.OriginCountryCode,
                        TradeAgreementCode = a.TradeAgreementCode,
                        TradeAgreementName = a.TradeAgreement != null ? a.TradeAgreement.LocalName : null,
                        ItemPrice = a.ItemPrice,
                        InvoiceQuantity = a.InvoiceQuantity,
                        AdditionalQuantity = a.AdditionalQuantity,
                        AdditionalQuantityType = a.AdditionalQuantityType,
                        CertificatesStatusCode = a.CertificatesStatusCode,
                        CustomsBookTypeCode = a.CustomsBookTypeCode,
                        DeferredCustomsTax = a.DeferredCustomsTax,
                        DangerousClassificationCode = a.DangerousClassificationCode,
                        DeferredPurchaseTax = a.DeferredPurchaseTax,
                        InvoiceQuantityType = a.InvoiceQuantityType,
                        ItemDescription = a.ItemDescription,
                        ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                        ManufactureIdentifier = a.ManufactureIdentifier,
                        DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                        OptionalTamaPercentage = a.OptionalTamaPercentage,
                        IsUsed = a.IsUsed,
                        NonCustomsItemPrice = a.NonCustomsItemPrice,
                        NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                        PreferenceDocumentNumber = a.PreferenceDocumentNumber,
                        SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                        StatisticQuantity = a.StatisticQuantity,
                        StatisticQuantityType = a.StatisticQuantityType,
                        TaxExemptCode = a.TaxExemptCode,
                        WholeSaleItemPrice = a.WholeSaleItemPrice,
                        WholeSaleItemPriceCurrencyCode = a.WholeSaleItemPriceCurrencyCode,
                        TransactionNatureCode= a.TransactionNatureCode,
                        TransactionNatureName=a.TransactionNatureCode,
                        ClaimReasonName=a.ClaimReasonCode,
                        ClaimReasonCode=a.ClaimReasonCode,
                        AdditionalQuantityTypeName = a.AdditionalMeasurmentUnit != null ? a.AdditionalMeasurmentUnit.LocalName : null,
                        InvoiceQuantityTypeName = a.InvoiceMeasurmentUnit != null ? a.InvoiceMeasurmentUnit.LocalName : null,
                        StatisticQuantityTypeName = a.StatisticMeasurmentUnit != null ? a.StatisticMeasurmentUnit.LocalName : null,
                        ItemAdditionalStatus=a.ItemAdditionalStatus,
                    }).ToList();
        }

        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemByClassificationCode(string declarationId, int tenant,string classificationCode)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemByClassificationCode(declarationId, tenant, classificationCode);
            return supplierInvoiceItems.Select(rec => this.GetEntityPM(rec)).ToList();
        }
        public List<SupplierInvoiceItemPM> GetSupplierInvoiceItemByInvoiceNumber(int tenant, string declarationId, string ItemCode)
        {
            List<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemByInvoiceNumber(tenant,declarationId,ItemCode);
            return (from a in supplierInvoiceItems
                    select new SupplierInvoiceItemPM()
                    {
                        DeclarationId = a.DeclarationId,
                        CounterKey = a.CounterKey,
                        LineNumber = a.LineNumber,
                        SequenceNumeric = a.SequenceNumeric,
                        ItemCode = a.ItemCode,
                        ClassificationCode = a.ClassificationCode,
                        OriginCountryName = a.OriginCountry != null ? a.OriginCountry.LocalName : null,
                        OriginCountryCode = a.OriginCountryCode,
                        TradeAgreementCode = a.TradeAgreementCode,
                        TradeAgreementName = a.TradeAgreement != null ? a.TradeAgreement.LocalName : null,
                        ItemPrice = a.ItemPrice,
                        InvoiceQuantity = a.InvoiceQuantity,
                        AdditionalQuantity = a.AdditionalQuantity,
                        AdditionalQuantityType = a.AdditionalQuantityType,
                        CertificatesStatusCode = a.CertificatesStatusCode,
                        CustomsBookTypeCode = a.CustomsBookTypeCode,
                        DeferredCustomsTax = a.DeferredCustomsTax,
                        DangerousClassificationCode = a.DangerousClassificationCode,
                        DeferredPurchaseTax = a.DeferredPurchaseTax,
                        InvoiceQuantityType = a.InvoiceQuantityType,
                        ItemDescription = a.ItemDescription,
                        ItemPriceCurrencyCode = a.ItemPriceCurrencyCode,
                        ManufactureIdentifier = a.ManufactureIdentifier,
                        DangerousPackingGroupTypeCode = a.DangerousPackingGroupTypeCode,
                        OptionalTamaPercentage = a.OptionalTamaPercentage,
                        IsUsed = a.IsUsed,
                        NonCustomsItemPrice = a.NonCustomsItemPrice,
                        NonCustomsItemPriceCurCode = a.NonCustomsItemPriceCurCode,
                        PreferenceDocumentNumber = a.PreferenceDocumentNumber,
                        SalesTaxExemptionTypeCode = a.SalesTaxExemptionTypeCode,
                        StatisticQuantity = a.StatisticQuantity,
                        StatisticQuantityType = a.StatisticQuantityType,
                        TaxExemptCode = a.TaxExemptCode,
                        WholeSaleItemPrice = a.WholeSaleItemPrice,
                        WholeSaleItemPriceCurrencyCode = a.WholeSaleItemPriceCurrencyCode,
                        AdditionalQuantityTypeName = a.AdditionalMeasurmentUnit != null ? a.AdditionalMeasurmentUnit.LocalName : null,
                        InvoiceQuantityTypeName = a.InvoiceMeasurmentUnit != null ? a.InvoiceMeasurmentUnit.LocalName : null,
                        StatisticQuantityTypeName = a.StatisticMeasurmentUnit != null ? a.StatisticMeasurmentUnit.LocalName : null,
                        Tenant=a.Tenant,
                        IsParent=a.IsParent,
                        ParentLineNumber=a.ParentLineNumber,
                    }).ToList();
        }

        public int GetSupplierInvoiceItemsCountForDeclaration(string declarationId, int tenant)
        {
           int itemsCount = repository.GetSupplierInvoiceItemsCountForDeclaration(declarationId, tenant);
            return itemsCount;
        }

        public IQueryable<SupplierInvoiceItemPM> GetSupplierInvoiceItemsQueryForDeclaration(string declarationId, int tenant)
        {
            SupplierInvoiceQueryService invoiceQueryService = new SupplierInvoiceQueryService(context);
            IQueryable<SupplierInvoiceItem> supplierInvoiceItems = repository.GetSupplierInvoiceItemsQueryForDeclaration(declarationId, tenant);
            IQueryable<SupplierInvoicePM> invoices = invoiceQueryService.GetSupplierInvoicesQueryForDeclaration(declarationId, tenant, false);

            return (from a in supplierInvoiceItems
                    join s in invoices on
                    a.CounterKey equals s.InvoiceCounterKey into invoiceItems
                    from sa in invoiceItems
                    select new SupplierInvoiceItemPM()
                    {
                        InvoiceNumber = sa.InvoiceNumber,
                        DeclarationId = a.DeclarationId,
                        CounterKey = a.CounterKey,
                        LineNumber = a.LineNumber,
                        SequenceNumeric = a.SequenceNumeric,
                        ItemCode = a.ItemCode,
                        ClassificationCode = a.ClassificationCode,
                        OriginCountryName = a.OriginCountry != null ? a.OriginCountry.LocalName : null,
                        OriginCountryCode = a.OriginCountryCode,
                        TradeAgreementCode = a.TradeAgreementCode,
                        TradeAgreementName = a.TradeAgreement != null ? a.TradeAgreement.LocalName : null,
                  
                    });


          

        }

        public decimal? GetTotalForeignCurrencyForInvoice(string declarationId, int counterKey, int tenant)
        {
            return  repository.GetTotalForeignCurrencyForInvoice(declarationId, counterKey, tenant);
        }

        public SupplierInvoiceItemPM GetSingleSupplierInvoicePMBySequence(string declarationId,int counterKey, int sequenceNumeric)
        {
            SupplierInvoiceItem supplierInvoiceItem = repository.GetSupplierInvoiceItemBySequenceNumeric(declarationId,  counterKey, sequenceNumeric);
            SupplierInvoiceItemPM itemPM = new SupplierInvoiceItemPM();
            SupplierInvoiceItemDataMapping mapping = new SupplierInvoiceItemDataMapping();
            mapping.CustomPOCOToPM(itemPM, supplierInvoiceItem);
            mapping.POCOToPM(itemPM, supplierInvoiceItem);
            GetComposition(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = supplierInvoiceItem.CounterKey, LineNumber = supplierInvoiceItem.LineNumber }, itemPM);
            return itemPM;
            
        }

        public SupplierInvoiceItemPM GetParentSingleSupplierInvoicePMBySequence(string declarationId, int counterKey, int sequenceNumeric)
        {
            SupplierInvoiceItem supplierInvoiceItem = repository.GetParentSupplierInvoiceItemBySequenceNumeric(declarationId, counterKey, sequenceNumeric);
            SupplierInvoiceItemPM itemPM = new SupplierInvoiceItemPM();
            SupplierInvoiceItemDataMapping mapping = new SupplierInvoiceItemDataMapping();
            if (supplierInvoiceItem != null)
            {
                mapping.CustomPOCOToPM(itemPM, supplierInvoiceItem);
                mapping.POCOToPM(itemPM, supplierInvoiceItem);
                GetComposition(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = supplierInvoiceItem.CounterKey, LineNumber = supplierInvoiceItem.LineNumber }, itemPM);
            }
            else itemPM = null;
            return itemPM;
        }


    }
}
