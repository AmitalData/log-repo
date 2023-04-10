using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.ExternalService;
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class WarehouseQuery
    {
        WarehouseRepository repository;

        public WarehouseQuery()
        {
            repository = new WarehouseRepository(); 
        }

        public WarehouseQuery(int tenant)
        {
            repository = new WarehouseRepository(tenant);
        }

        public WarehouseQuery(WarehouseRepository repository)
        {
            this.repository = repository;
        }

        public WarehousePM GetSinglePM(string id, int tenant)
        {
            WarehousePM warehouse = (from a in repository.context.Warehouses.Include("Card")
                                     where a.Id == id && a.Tenant == tenant
                                     select new WarehousePM()
                                     {
                                         ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                         ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                                         ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                         PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                         AccountingVATSplit = a.Card.AccountingVATSplit,
                                         BillToId = a.Card.BillToId,
                                         AddedManually = a.AddedManually,
                                         Id = a.Id,
                                         Tenant = a.Tenant,
                                         VatNumber = a.Card.VatNumber,
                                         Code = a.Card.Code,
                                         EnglishName = a.Card.EnglishName,
                                         LocalName = a.Card.LocalName,
                                         PartnerTypeId = a.Card.PartnerTypeId,
                                         InActive = a.Card.InActive,
                                         PaymentTermId = a.Card.PaymentTermId,
                                         Website = a.Card.Website,
                                         Notes = a.Card.Notes,
                                         InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                         ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                         VatTypeId = a.Card.VatTypeId,
                                         AccountNumber = a.Card.AccountNumber,
                                         Swift = a.Card.Swift,
                                         IBANNumber = a.Card.IBANNumber,
                                         BankName = a.Card.BankName,
                                         BankAddress = a.Card.BankAddress,
                                         PrimaryContactId = a.Card.PrimaryContactId,
                                         IRSNumber = a.Card.IRSNumber,
                                         IRSPlace = a.Card.IRSPlace,
                                         ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                         PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                         ExternalId2 = a.Card.ExternalId2,
                                         MetodoPagoCode = a.Card.MetodoPagoCode,
                                         FirmCode = a.FirmCode,
                                         TypeCode = a.TypeCode,
                                         MyWarehouse = a.MyWarehouse,
                                         UsoCFDICode = a.Card.UsoCFDICode,
                                         SATForeignRFC = a.Card.SATForeignRFC,
                                         ChargeStorage = a.ChargeStorage,
                                         CurrencyId = a.CurrencyId,
                                         AirWeightMeasurementCode = a.AirWeightMeasurementCode,
                                         OceanWeightMeasurementCode = a.OceanWeightMeasurementCode,
                                         InlandWeightMeasurementCode = a.InlandWeightMeasurementCode,
                                         AirWeightRoundingCode = a.AirWeightRoundingCode,
                                         OceanWeightRoundingCode = a.OceanWeightRoundingCode,
                                         InlandWeightRoundingCode = a.InlandWeightRoundingCode,
                                         GLAccountId = a.Card.GLAccountId,
                                         GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                         StorageFreeDays = a.Card.StorageFreeDays,
                                         RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                         SATReceptorName = a.Card.SATCustomerName,

                                         Card = new CardPM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             EnglishName = a.Card.EnglishName,
                                             PrimaryContactId = a.Card.PrimaryContactId,
                                             PartnerTypeId = a.Card.PartnerTypeId,
                                             Code = a.Card.Code,
                                             GLAccountDisplayNumber = a.Card.GLAccountDisplayNumber,
                                             SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                             CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                             ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                             ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                                         },
                                     }).FirstOrDefault();

            if (warehouse != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(warehouse.Tenant);
                warehouse.Card = partnerARinvoiceDocumentTypeService.Set(warehouse.Card);
            }
            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            warehouse.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(warehouse.Id, warehouse.Tenant);

            WarehouseStoragePricingRepository warehouseStoragePricingRepository = new WarehouseStoragePricingRepository(repository.context);
            WarehouseStoragePricingQuery warehouseStoragePricingQuery = new WarehouseStoragePricingQuery(warehouseStoragePricingRepository);
            warehouse.WarehouseStoragePricings = warehouseStoragePricingQuery.GetWarehouseStoragePricingPMsByWarehouseId(warehouse.Id, warehouse.Tenant);

            if (warehouse != null)
            {
                warehouse.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        warehouse.IsExternal = true;
                    }
                }
            }

            WarehousePM securedPm = new WarehousePM();
            SecuredMapping.GetMappedPM(warehouse, securedPm, "Warehouse", tenant);

            if (securedPm != null && warehouse != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Warehouse", Tenant = tenant, Type = "PM", Entities = new List<WarehousePM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        } 

        public WarehousePM GetSingleWarehousePM(string id, int tenant)
        {
            WarehousePM warehouse = (from a in repository.context.Warehouses.Include("Card")
                                     where a.Id == id && a.Tenant == tenant
                                     select new WarehousePM()
                                     {
                                         ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                         ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                                         ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                         PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                         AccountingVATSplit = a.Card.AccountingVATSplit,
                                         BillToId = a.Card.BillToId,
                                         AddedManually = a.AddedManually,
                                         Id = a.Id,
                                         Tenant = a.Tenant,
                                         VatNumber = a.Card.VatNumber,
                                         Code = a.Card.Code,
                                         EnglishName = a.Card.EnglishName,
                                         LocalName = a.Card.LocalName,
                                         PartnerTypeId = a.Card.PartnerTypeId,
                                         InActive = a.Card.InActive,
                                         PaymentTermId = a.Card.PaymentTermId,
                                         Website = a.Card.Website,
                                         Notes = a.Card.Notes,
                                         InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                         ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                         VatTypeId = a.Card.VatTypeId,
                                         AccountNumber = a.Card.AccountNumber,
                                         Swift = a.Card.Swift,
                                         IBANNumber = a.Card.IBANNumber,
                                         BankName = a.Card.BankName,
                                         BankAddress = a.Card.BankAddress,
                                         PrimaryContactId = a.Card.PrimaryContactId,
                                         EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                         IRSNumber = a.Card.IRSNumber,
                                         IRSPlace = a.Card.IRSPlace,
                                         ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                         PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                         ExternalId2 = a.Card.ExternalId2,
                                         MetodoPagoCode = a.Card.MetodoPagoCode,
                                         FirmCode = a.FirmCode,
                                         TypeCode = a.TypeCode,
                                         MyWarehouse = a.MyWarehouse,
                                         UsoCFDICode = a.Card.UsoCFDICode,
                                         SATForeignRFC = a.Card.SATForeignRFC,
                                         ChargeStorage = a.ChargeStorage,
                                         CurrencyId = a.CurrencyId,
                                         AirWeightMeasurementCode = a.AirWeightMeasurementCode,
                                         OceanWeightMeasurementCode = a.OceanWeightMeasurementCode,
                                         InlandWeightMeasurementCode = a.InlandWeightMeasurementCode,
                                         AirWeightRoundingCode = a.AirWeightRoundingCode,
                                         OceanWeightRoundingCode = a.OceanWeightRoundingCode,
                                         InlandWeightRoundingCode = a.InlandWeightRoundingCode,
                                         StorageFreeDays = a.Card.StorageFreeDays,
                                         RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                         SATReceptorName = a.Card.SATCustomerName,

                                         Card = new CardPM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             EnglishName = a.Card.EnglishName,
                                             PrimaryContactId = a.Card.PrimaryContactId,
                                             SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                             CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                             ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                             ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                                         },
                                     }).FirstOrDefault();

            if (warehouse != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(warehouse.Tenant);
                warehouse.Card = partnerARinvoiceDocumentTypeService.Set(warehouse.Card);
            }
            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            warehouse.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(warehouse.Id, warehouse.Tenant);
           

            if (warehouse != null)
            {
                warehouse.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        warehouse.IsExternal = true;
                    }
                }
            }

            WarehousePM securedPm = new WarehousePM();
            SecuredMapping.GetMappedPM(warehouse, securedPm, "Warehouse", tenant);

            if (securedPm != null && warehouse != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Warehouse", Tenant = tenant, Type = "PM", Entities = new List<WarehousePM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }
        public IQueryable<WarehousePM> GetWarehousePMsByTenant(int tenant)
        {
            return from a in repository.context.Warehouses.Include("Card")
                   where a.Tenant == tenant
                   select new WarehousePM()
                   {
                       ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                       ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                       ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                       PayablesAccountingCard = a.Card.PayablesAccountingCard,
                       AccountingVATSplit = a.Card.AccountingVATSplit,
                       BillToId = a.Card.BillToId,
                       AddedManually = a.AddedManually,
                       Id = a.Id,
                       Tenant = a.Tenant,
                       VatNumber = a.Card.VatNumber,
                       Code = a.Card.Code,
                       EnglishName = a.Card.EnglishName,
                       LocalName = a.Card.LocalName,
                       PartnerTypeId = a.Card.PartnerTypeId,
                       InActive = a.Card.InActive,
                       PaymentTermId = a.Card.PaymentTermId,
                       Website = a.Card.Website,
                       Notes = a.Card.Notes,
                       VatTypeId = a.Card.VatTypeId,
                       InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                       ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                       AccountNumber = a.Card.AccountNumber,
                       Swift = a.Card.Swift,
                       IBANNumber = a.Card.IBANNumber,
                       BankName = a.Card.BankName,
                       BankAddress = a.Card.BankAddress,
                       PrimaryContactId = a.Card.PrimaryContactId,
                       EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                       IRSNumber = a.Card.IRSNumber,
                       IRSPlace = a.Card.IRSPlace,
                       ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                       PaymentMethodCode = a.Card.SATPaymentMethodCode,
                       ExternalId2 = a.Card.ExternalId2,
                       MetodoPagoCode = a.Card.MetodoPagoCode,
                       FirmCode = a.FirmCode,
                       TypeCode = a.TypeCode,
                       MyWarehouse = a.MyWarehouse,
                       UsoCFDICode = a.Card.UsoCFDICode,
                       SATForeignRFC = a.Card.SATForeignRFC,
                       ChargeStorage = a.ChargeStorage,
                       CurrencyId = a.CurrencyId,
                       AirWeightMeasurementCode = a.AirWeightMeasurementCode,
                       OceanWeightMeasurementCode = a.OceanWeightMeasurementCode,
                       InlandWeightMeasurementCode = a.InlandWeightMeasurementCode,
                       AirWeightRoundingCode = a.AirWeightRoundingCode,
                       OceanWeightRoundingCode = a.OceanWeightRoundingCode,
                       InlandWeightRoundingCode = a.InlandWeightRoundingCode,
                       StorageFreeDays = a.Card.StorageFreeDays,
                       RegimenFiscalCode = a.Card.RegimenFiscalCode,
                       SATReceptorName = a.Card.SATCustomerName,

                       Card = new CardPM()
                       {
                           Id = a.Id,
                           Tenant = a.Tenant,
                           EnglishName = a.Card.EnglishName,
                           PrimaryContactId = a.Card.PrimaryContactId,
                       },
                   };
        }

        public IQueryable<WarehouseList> GetIQueryableEntityList(IQueryable<Warehouse> iQueryable)
        {
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<WarehouseList> result = (from a in iQueryable.Include("Card")
                                                join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
                                                select new WarehouseList()
                                                {
                                                    Code = a.Card.Code,
                                                    EnglishName = a.Card.EnglishName,
                                                    LocalName = a.Card.LocalName,
                                                    ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                    PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                    InActive = a.Card.InActive,
                                                    Remark = a.Card.Notes,
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    VatNumber = a.Card.VatNumber,
                                                    AddedManually = a.AddedManually,
                                                    PaymentTermId = a.Card.PaymentTermId,
                                                    SearchFields = a.Card.SearchFields,
                                                    Notes = a.Card.Notes,
                                                    Website = a.Card.Website,
                                                    InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                    VatTypeId = a.Card.VatTypeId,
                                                    EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                    CityName = a.Card.CityName,
                                                    Address1 = a.Card.Address1,
                                                    Address2 = a.Card.Address2,
                                                    CountryId = a.Card.CountryId,
                                                    CountryCode = a.Card.CountryCode,
                                                    CountryName = a.Card.CountryName,
                                                    ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                    PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                    ExternalId2 = a.Card.ExternalId2,
                                                    MetodoPagoCode = a.Card.MetodoPagoCode,
                                                    FirmCode = a.FirmCode,
                                                    TypeCode = a.TypeCode,
                                                    MyWarehouse = a.MyWarehouse,
                                                    UsoCFDICode = a.Card.UsoCFDICode,
                                                    SATForeignRFC = a.Card.SATForeignRFC,
                                                    PrimaryContactName = a.PrimaryContactName,
                                                    PrimaryContactEmail = a.PrimaryContactEmail,
                                                    PrimaryContactPhone = a.PrimaryContactPhone,
                                                    StateName = a.Card.StateName,
                                                    ChargeStorage = a.ChargeStorage,
                                                    CurrencyId = a.CurrencyId,
                                                    AirWeightMeasurementCode = a.AirWeightMeasurementCode,
                                                    OceanWeightMeasurementCode = a.OceanWeightMeasurementCode,
                                                    InlandWeightMeasurementCode = a.InlandWeightMeasurementCode,
                                                    AirWeightRoundingCode = a.AirWeightRoundingCode,
                                                    OceanWeightRoundingCode = a.OceanWeightRoundingCode,
                                                    InlandWeightRoundingCode = a.InlandWeightRoundingCode,
                                                    GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                                    StorageFreeDays = a.Card.StorageFreeDays,
                                                    RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                                    SATReceptorName = a.Card.SATCustomerName,
                                                    Field1 = customFieldsMainObject != null ? customFieldsMainObject.Field1 : null,
                                                    Field2 = customFieldsMainObject != null ? customFieldsMainObject.Field2 : null,
                                                    Field3 = customFieldsMainObject != null ? customFieldsMainObject.Field3 : null,
                                                    Field4 = customFieldsMainObject != null ? customFieldsMainObject.Field4 : null,
                                                    Field5 = customFieldsMainObject != null ? customFieldsMainObject.Field5 : null,
                                                    Field6 = customFieldsMainObject != null ? customFieldsMainObject.Field6 : null,
                                                    Field7 = customFieldsMainObject != null ? customFieldsMainObject.Field7 : null,
                                                    Field8 = customFieldsMainObject != null ? customFieldsMainObject.Field8 : null,
                                                    Field9 = customFieldsMainObject != null ? customFieldsMainObject.Field9 : null,
                                                    Field10 = customFieldsMainObject != null ? customFieldsMainObject.Field10 : null,
                                                    Field11 = customFieldsMainObject != null ? customFieldsMainObject.Field11 : null,
                                                    Field12 = customFieldsMainObject != null ? customFieldsMainObject.Field12 : null,
                                                    Field13 = customFieldsMainObject != null ? customFieldsMainObject.Field13 : null,
                                                    Field14 = customFieldsMainObject != null ? customFieldsMainObject.Field14 : null,
                                                    Field15 = customFieldsMainObject != null ? customFieldsMainObject.Field15 : null,
                                                    Field16 = customFieldsMainObject != null ? customFieldsMainObject.Field16 : null,
                                                    Field17 = customFieldsMainObject != null ? customFieldsMainObject.Field17 : null,
                                                    Field18 = customFieldsMainObject != null ? customFieldsMainObject.Field18 : null,
                                                    Field19 = customFieldsMainObject != null ? customFieldsMainObject.Field19 : null,
                                                    Field20 = customFieldsMainObject != null ? customFieldsMainObject.Field20 : null,
                                                    Field21 = customFieldsMainObject != null ? customFieldsMainObject.Field21 : null,
                                                    Field22 = customFieldsMainObject != null ? customFieldsMainObject.Field22 : null,
                                                    Field23 = customFieldsMainObject != null ? customFieldsMainObject.Field23 : null,
                                                    Field24 = customFieldsMainObject != null ? customFieldsMainObject.Field24 : null,
                                                    Field25 = customFieldsMainObject != null ? customFieldsMainObject.Field25 : null,
                                                    Field26 = customFieldsMainObject != null ? customFieldsMainObject.Field26 : null,
                                                    Field27 = customFieldsMainObject != null ? customFieldsMainObject.Field27 : null,
                                                    Field28 = customFieldsMainObject != null ? customFieldsMainObject.Field28 : null,
                                                    Field29 = customFieldsMainObject != null ? customFieldsMainObject.Field29 : null,
                                                    Field30 = customFieldsMainObject != null ? customFieldsMainObject.Field30 : null,
                                                    Field31 = customFieldsMainObject != null ? customFieldsMainObject.Field31 : null,
                                                    Field32 = customFieldsMainObject != null ? customFieldsMainObject.Field32 : null,
                                                    Field33 = customFieldsMainObject != null ? customFieldsMainObject.Field33 : null,
                                                    Field34 = customFieldsMainObject != null ? customFieldsMainObject.Field34 : null,
                                                    Field35 = customFieldsMainObject != null ? customFieldsMainObject.Field35 : null,
                                                    Field36 = customFieldsMainObject != null ? customFieldsMainObject.Field36 : null,
                                                    Field37 = customFieldsMainObject != null ? customFieldsMainObject.Field37 : null,
                                                    Field38 = customFieldsMainObject != null ? customFieldsMainObject.Field38 : null,
                                                    Field39 = customFieldsMainObject != null ? customFieldsMainObject.Field39 : null,
                                                    Field40 = customFieldsMainObject != null ? customFieldsMainObject.Field40 : null,
                                                    Field41 = customFieldsMainObject != null ? customFieldsMainObject.Field41 : null,
                                                    Field42 = customFieldsMainObject != null ? customFieldsMainObject.Field42 : null,
                                                    Field43 = customFieldsMainObject != null ? customFieldsMainObject.Field43 : null,
                                                    Field44 = customFieldsMainObject != null ? customFieldsMainObject.Field44 : null,
                                                    Field45 = customFieldsMainObject != null ? customFieldsMainObject.Field45 : null,
                                                    Field46 = customFieldsMainObject != null ? customFieldsMainObject.Field46 : null,
                                                    Field47 = customFieldsMainObject != null ? customFieldsMainObject.Field47 : null,
                                                    Field48 = customFieldsMainObject != null ? customFieldsMainObject.Field48 : null,
                                                    Field49 = customFieldsMainObject != null ? customFieldsMainObject.Field49 : null,
                                                    Field50 = customFieldsMainObject != null ? customFieldsMainObject.Field50 : null,
                                                    PaymentTermEnglishName = a.Card.PaymentTerm != null ? a.Card.PaymentTerm.EnglishName : "",
                                                });


            return result;
        }
    
        public WarehousePM GetSinglePMByCode(string code, int tenant)
        {
            var warehouse = (from a in repository.context.Warehouses.Include("Card")
                           where a.Card.Code == code && a.Tenant == tenant
                           select new WarehousePM()
                           {
                               ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                               ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
                               AddedManually = a.AddedManually,
                               Id = a.Id,
                               Tenant = a.Tenant,
                               VatNumber = a.Card.VatNumber,
                               Code = a.Card.Code,
                               EnglishName = a.Card.EnglishName,
                               LocalName = a.Card.LocalName,
                               PartnerTypeId = a.Card.PartnerTypeId,
                               InActive = a.Card.InActive,
                               PaymentTermId = a.Card.PaymentTermId,
                               Website = a.Card.Website,
                               Notes = a.Card.Notes,
                               InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                               ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                               VatTypeId = a.Card.VatTypeId,
                               AccountNumber = a.Card.AccountNumber,
                               Swift = a.Card.Swift,
                               IBANNumber = a.Card.IBANNumber,
                               BankName = a.Card.BankName,
                               BankAddress = a.Card.BankAddress,
                               PrimaryContactId = a.Card.PrimaryContactId,
                               EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                               IRSNumber = a.Card.IRSNumber,
                               IRSPlace = a.Card.IRSPlace,
                               ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                               PaymentMethodCode = a.Card.SATPaymentMethodCode,
                               ExternalId2 = a.Card.ExternalId2,
                               MetodoPagoCode = a.Card.MetodoPagoCode,
                               FirmCode = a.FirmCode,
                               TypeCode = a.TypeCode,
                               MyWarehouse = a.MyWarehouse,
                               UsoCFDICode = a.Card.UsoCFDICode,
                               SATForeignRFC = a.Card.SATForeignRFC,
                               ChargeStorage = a.ChargeStorage,
                               CurrencyId = a.CurrencyId,
                               AirWeightMeasurementCode = a.AirWeightMeasurementCode,
                               OceanWeightMeasurementCode = a.OceanWeightMeasurementCode,
                               InlandWeightMeasurementCode = a.InlandWeightMeasurementCode,
                               AirWeightRoundingCode = a.AirWeightRoundingCode,
                               OceanWeightRoundingCode = a.OceanWeightRoundingCode,
                               InlandWeightRoundingCode = a.InlandWeightRoundingCode,
                               StorageFreeDays = a.Card.StorageFreeDays,
                               RegimenFiscalCode = a.Card.RegimenFiscalCode,
                               SATReceptorName = a.Card.SATCustomerName,

                               Card = new CardPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   EnglishName = a.Card.EnglishName,
                                   PrimaryContactId = a.Card.PrimaryContactId,
                                   SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                   CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                   ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                   ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                               },
                           }).FirstOrDefault();
            if(warehouse != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(warehouse.Tenant);
                warehouse.Card = partnerARinvoiceDocumentTypeService.Set(warehouse.Card);
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Warehouse", Tenant = tenant, Type = "PM", Entities = new List<WarehousePM> { warehouse }.Cast<object>().ToList() }).Set();
            }
            return warehouse;
        }
    
        public string GetWarehouseTypeById(string warehouseId, int tenant)
        {
            return repository.GetWarehouseTypeById(warehouseId, tenant);
        }
    }
}
