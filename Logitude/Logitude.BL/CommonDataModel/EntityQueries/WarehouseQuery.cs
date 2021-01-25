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
                                         GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                         StorageFreeDays = a.Card.StorageFreeDays,

                                         Card = new CardPM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             EnglishName = a.Card.EnglishName,
                                             PrimaryContactId = a.Card.PrimaryContactId,
                                             PartnerTypeId = a.Card.PartnerTypeId,
                                             Code = a.Card.Code,
                                             GLAccountDisplayNumber = a.Card.GLAccountDisplayNumber,
                                         },
                                     }).FirstOrDefault();

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

            return securedPm;
        }

        public WarehousePM GetSingleWarehousePM(string id, int tenant)
        {
            WarehousePM warehouse = (from a in repository.context.Warehouses.Include("Card")
                                     where a.Id == id && a.Tenant == tenant
                                     select new WarehousePM()
                                     {
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

                                         Card = new CardPM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             EnglishName = a.Card.EnglishName,
                                             PrimaryContactId = a.Card.PrimaryContactId,
                                         },
                                     }).FirstOrDefault();

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

            return securedPm;
        }

        public IQueryable<WarehousePM> GetWarehousePMsByTenant(int tenant)
        {
            return from a in repository.context.Warehouses.Include("Card")
                   where a.Tenant == tenant
                   select new WarehousePM()
                   {
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
            IQueryable<WarehouseList> result = (from a in iQueryable.Include("Card")
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
                                                });


            return result;
        }
    
        public WarehousePM GetSinglePMByCode(string code, int tenant)
        {
            var warehouse = (from a in repository.context.Warehouses.Include("Card")
                           where a.Card.Code == code && a.Tenant == tenant
                           select new WarehousePM()
                           {
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

                               Card = new CardPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   EnglishName = a.Card.EnglishName,
                                   PrimaryContactId = a.Card.PrimaryContactId,
                               },
                           }).FirstOrDefault();


            return warehouse;
        }
    
        public string GetWarehouseTypeById(string warehouseId, int tenant)
        {
            return repository.GetWarehouseTypeById(warehouseId, tenant);
        }
    }
}
