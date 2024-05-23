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
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ChargesTypeQuery
    {
        ChargesTypeRepository repository;

        public ChargesTypeQuery()
        {
            repository = new ChargesTypeRepository();
        }
        public ChargesTypeQuery(int tenant)
        {
            repository = new ChargesTypeRepository(tenant);
        }
        public ChargesTypeQuery(ChargesTypeRepository chargesTypeRepository)
        {
            repository = chargesTypeRepository;
        }

        public ChargesTypePM GetSingle(string id, int tenant)
        {
            ChargesTypePM entity = null;

            entity = (from a in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement")
                      where a.Id == id && a.Tenant == tenant
                      select new ChargesTypePM()
                      {
                          AddedManually = a.AddedManually,
                          Code = a.Code,
                          MeasurementId = a.MeasurementId,
                          MeasurementCode = a.Measurement.Code,
                          MeasurementShortName = a.Measurement.ShortName,
                          Id = a.Id,
                          InActive = a.InActive,
                          LocalName = a.LocalName,
                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                          EnglishName = a.EnglishName,
                          Tenant = a.Tenant,
                          AWBPrintDescription = a.AWBPrintDescription,
                          ChargesGroupCode = a.ChargesGroupCode,
                          ChargesGroupId = a.ChargesGroupId,
                          QuoteChargesGroupCode = a.QuoteChargesGroupCode,
                          QuoteChargesGroupId = a.QuoteChargesGroupId,
                          IATACodeId = a.IATACodeId,
                          Description = a.Description,
                          IsAir = a.IsAir,
                          IsOcean = a.IsOcean,
                          IsInland = a.IsInland,
                          IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                          IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                          IsPayable = a.IsPayable,
                          IsReceivable = a.IsReceivable,
                          VatTypeId = a.VatTypeId,
                          DueTypeCode = a.DueTypeCode,
                          IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                          ContainerMeasurementId = a.ContainerMeasurementId,
                          ContainerMeasurementCode = a.ContainerMeasurement != null ? a.ContainerMeasurement.Code : null,
                          ViewOrder = a.ViewOrder,
                          SearchFields = a.SearchFields,
                          ReceivableAccountId = a.ReceivableAccountId,
                          PayableAccountId = a.PayableAccountId,
                          AccountingVATSplit = a.AccountingVATSplit,
                          ReceivableCreditAccount = a.ReceivableCreditAccount,
                          PayableDebitAccount = a.PayableDebitAccount,
                          ReceivablesChargesTypeExtCode = a.ReceivablesChargesTypeExtCode,
                          PayablesChargesTypeExtCode = a.PayablesChargesTypeExtCode,
                          PayableDebitGLAcountId = a.PayableDebitGLAcountId,
                          ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
                          RecCreditGLAcountLocalName = a.RecCreditGLAcountLocalName,
                          PayDebitGLAcountLocalName = a.PayDebitGLAcountLocalName,
                          IsAutoDisplayInCustoms = a.IsAutoDisplayInCustoms,
                          IsCustoms = a.IsCustoms,
                          IsBackToBack = a.IsBackToBack,
                          SATExternalId = a.SATExternalId,
                          IsExpense = a.IsExpense,
                          IsDomestic = a.IsDomestic, 
                          IsImport = a.IsImport, 
                          IsDrop = a.IsDrop, 
                          IsExport = a.IsExport, 
                          ReceivablesDefaultCurrencyId = a.ReceivablesDefaultCurrencyId, 
                          PayablesDefaultCurrencyId = a.PayablesDefaultCurrencyId,
                          ApplyRegionalTax = a.ApplyRegionalTax,
                          HasPickup = a.HasPickup,
                          HasDelivery = a.HasDelivery,
                          IsDirectionRestricted = a.IsDirectionRestricted,
                          IsActiveInExport = a.IsActiveInExport,
                          IsActiveInImport = a.IsActiveInImport,
                          IsActiveInDrop= a.IsActiveInDrop,
                          IsActiveInDomestic = a.IsActiveInDomestic,
                          
                      }).FirstOrDefault();

            ChargeTypeAccountingQuery chargeTypeAccountingQuery = new ChargeTypeAccountingQuery(tenant);
            entity.ChargeTypeAccountings = chargeTypeAccountingQuery.GetChargeTypeAccountingsForChargeType(entity.Id, tenant).ToList();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", Tenant = tenant, Type = "PM", Entities = new List<ChargesTypePM> { entity }.Cast<object>().ToList() }).Set();

            ChargesTypePM securedPm = new ChargesTypePM();
            SecuredMapping.GetMappedPM(entity, securedPm, "ChargesType", tenant);

            return securedPm;
        }

        public ChargesTypePM GetSinglePM(string id, int tenant)
        {
            ChargesTypePM entity = (from a in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement")
                                    where a.Id == id && a.Tenant == tenant
                                    select new ChargesTypePM()
                                    {
                                        AddedManually = a.AddedManually,
                                        Code = a.Code,
                                        MeasurementId = a.MeasurementId,
                                        MeasurementCode = a.Measurement.Code,
                                        MeasurementShortName = a.Measurement.ShortName,
                                        Id = a.Id,
                                        InActive = a.InActive,
                                        LocalName = a.LocalName,
                                        ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                        EnglishName = a.EnglishName,
                                        Tenant = a.Tenant,
                                        AWBPrintDescription = a.AWBPrintDescription,
                                        ChargesGroupCode = a.ChargesGroupCode,
                                        ChargesGroupId = a.ChargesGroupId,
                                        QuoteChargesGroupCode = a.QuoteChargesGroupCode,
                                        QuoteChargesGroupId = a.QuoteChargesGroupId,
                                        IATACodeId = a.IATACodeId,
                                        Description = a.Description,
                                        IsAir = a.IsAir,
                                        IsOcean = a.IsOcean,
                                        IsInland = a.IsInland,
                                        IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                                        IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                                        IsPayable = a.IsPayable,
                                        IsReceivable = a.IsReceivable,
                                        VatTypeId = a.VatTypeId,
                                        DueTypeCode = a.DueTypeCode,
                                        IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                                        ContainerMeasurementId = a.ContainerMeasurementId,
                                        ContainerMeasurementCode = a.ContainerMeasurement != null ? a.ContainerMeasurement.Code : null,
                                        ViewOrder = a.ViewOrder,
                                        SearchFields = a.SearchFields,
                                        ReceivableAccountId = a.ReceivableAccountId,
                                        PayableAccountId = a.PayableAccountId,
                                        AccountingVATSplit = a.AccountingVATSplit,
                                        ReceivableCreditAccount = a.ReceivableCreditAccount,
                                        PayableDebitAccount = a.PayableDebitAccount,
                                        ReceivablesChargesTypeExtCode = a.ReceivablesChargesTypeExtCode,
                                        PayablesChargesTypeExtCode = a.PayablesChargesTypeExtCode,
                                        PayableDebitGLAcountId = a.PayableDebitGLAcountId,
                                        ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
                                        RecCreditGLAcountLocalName = a.RecCreditGLAcountLocalName,
                                        PayDebitGLAcountLocalName = a.PayDebitGLAcountLocalName,
                                        IsAutoDisplayInCustoms = a.IsAutoDisplayInCustoms,
                                        IsCustoms = a.IsCustoms,
                                        IsBackToBack = a.IsBackToBack,
                                        SATExternalId = a.SATExternalId,
                                        IsExpense = a.IsExpense,
                                        IsDomestic = a.IsDomestic,
                                        IsImport = a.IsImport,
                                        IsDrop = a.IsDrop,
                                        IsExport = a.IsExport,
                                        ReceivablesDefaultCurrencyId = a.ReceivablesDefaultCurrencyId,
                                        PayablesDefaultCurrencyId = a.PayablesDefaultCurrencyId,
                                        ApplyRegionalTax = a.ApplyRegionalTax,
                                        HasPickup = a.HasPickup,
                                        HasDelivery = a.HasDelivery,
                                        IsDirectionRestricted = a.IsDirectionRestricted,
                                        IsActiveInExport = a.IsActiveInExport,
                                        IsActiveInImport = a.IsActiveInImport,
                                        IsActiveInDrop = a.IsActiveInDrop,
                                        IsActiveInDomestic = a.IsActiveInDomestic,
                                        QuoteGroupSectionID=a.QuoteGroupSectionID,
                                    }).FirstOrDefault();

            ChargeTypeAccountingQuery chargeTypeAccountingQuery = new ChargeTypeAccountingQuery(tenant);
            entity.ChargeTypeAccountings = chargeTypeAccountingQuery.GetChargeTypeAccountingsForChargeType(entity.Id, tenant).ToList();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", Tenant = tenant, Type = "PM", Entities = new List<ChargesTypePM> { entity }.Cast<object>().ToList() }).Set();

            ChargesTypePM securedPm = new ChargesTypePM();
            SecuredMapping.GetMappedPM(entity, securedPm, "ChargesType", tenant);

            return securedPm;
        }

        public ChargesTypePM GetSinglePMByCode(string code, int tenant)
        {
            ChargesTypePM entity = null;
            string entityName = "ChargesTypePM" + code + tenant;

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = (from a in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement")
                              where a.Code == code && a.Tenant == tenant
                              select new ChargesTypePM()
                              {
                                  AddedManually = a.AddedManually,
                                  Code = a.Code,
                                  MeasurementId = a.MeasurementId,
                                  MeasurementCode = a.Measurement.Code,
                                  MeasurementShortName = a.Measurement.ShortName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  EnglishName = a.EnglishName,
                                  Tenant = a.Tenant,
                                  AWBPrintDescription = a.AWBPrintDescription,
                                  ChargesGroupCode = a.ChargesGroupCode,
                                  ChargesGroupId = a.ChargesGroupId,
                                  IATACodeId = a.IATACodeId,
                                  Description = a.Description,
                                  IsAir = a.IsAir,
                                  IsOcean = a.IsOcean,
                                  IsInland = a.IsInland,
                                  IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                                  IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                                  IsPayable = a.IsPayable,
                                  IsReceivable = a.IsReceivable,
                                  VatTypeId = a.VatTypeId,
                                  DueTypeCode = a.DueTypeCode,
                                  IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                                  ContainerMeasurementId = a.ContainerMeasurementId,
                                  ContainerMeasurementCode = a.ContainerMeasurement != null ? a.ContainerMeasurement.Code : null,
                                  ViewOrder = a.ViewOrder,
                                  SearchFields = a.SearchFields,
                                  ReceivableAccountId = a.ReceivableAccountId,
                                  PayableAccountId = a.PayableAccountId,
                                  AccountingVATSplit = a.AccountingVATSplit,
                                  ReceivableCreditAccount = a.ReceivableCreditAccount,
                                  PayableDebitAccount = a.PayableDebitAccount,
                                  ReceivablesChargesTypeExtCode = a.ReceivablesChargesTypeExtCode,
                                  PayablesChargesTypeExtCode = a.PayablesChargesTypeExtCode,
                                  PayableDebitGLAcountId = a.PayableDebitGLAcountId,
                                  ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
                                  RecCreditGLAcountLocalName = a.RecCreditGLAcountLocalName,
                                  PayDebitGLAcountLocalName = a.PayDebitGLAcountLocalName,
                                  IsAutoDisplayInCustoms = a.IsAutoDisplayInCustoms,
                                  IsCustoms = a.IsCustoms,
                                  IsBackToBack = a.IsBackToBack,
                                  SATExternalId = a.SATExternalId,
                                  IsExpense = a.IsExpense,
                                  IsDomestic = a.IsDomestic,
                                  IsImport = a.IsImport,
                                  IsDrop = a.IsDrop,
                                  IsExport = a.IsExport,
                                  ReceivablesDefaultCurrencyId = a.ReceivablesDefaultCurrencyId,
                                  PayablesDefaultCurrencyId = a.PayablesDefaultCurrencyId,
                                  ApplyRegionalTax = a.ApplyRegionalTax,
                                  HasPickup = a.HasPickup,
                                  HasDelivery = a.HasDelivery,
                                  IsDirectionRestricted = a.IsDirectionRestricted,
                                  IsActiveInExport = a.IsActiveInExport,
                                  IsActiveInImport = a.IsActiveInImport,
                                  IsActiveInDrop = a.IsActiveInDrop,
                                  IsActiveInDomestic = a.IsActiveInDomestic,
                              }).FirstOrDefault();

                    ChargeTypeAccountingQuery chargeTypeAccountingQuery = new ChargeTypeAccountingQuery(tenant);
                    if (entity != null)
                    {
                        entity.ChargeTypeAccountings = chargeTypeAccountingQuery.GetChargeTypeAccountingsForChargeType(entity.Id, tenant).ToList();
                        new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", Tenant = tenant, Type = "PM", Entities = new List<ChargesTypePM> { entity }.Cast<object>().ToList() }).Set();

                        string cname = "ChargesTypePM" + entity.Id + entity.Tenant;

                        if (CacheManager.CacheWrapper.Get(cname) == null)
                        {
                            CacheManager.CacheWrapper.Insert(cname, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else {
                        return null;
                    }


                }
                else
                {
                    entity = (ChargesTypePM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                entity = (from a in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement")
                          where a.Code == code && a.Tenant == tenant
                          select new ChargesTypePM()
                          {
                              AddedManually = a.AddedManually,
                              Code = a.Code,
                              MeasurementId = a.MeasurementId,
                              MeasurementCode = a.Measurement.Code,
                              MeasurementShortName = a.Measurement.ShortName,
                              Id = a.Id,
                              InActive = a.InActive,
                              LocalName = a.LocalName,
                              ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                              EnglishName = a.EnglishName,
                              Tenant = a.Tenant,
                              AWBPrintDescription = a.AWBPrintDescription,
                              ChargesGroupCode = a.ChargesGroupCode,
                              ChargesGroupId = a.ChargesGroupId,
                              QuoteChargesGroupCode = a.QuoteChargesGroupCode,
                              QuoteChargesGroupId = a.QuoteChargesGroupId,
                              IATACodeId = a.IATACodeId,
                              Description = a.Description,
                              IsAir = a.IsAir,
                              IsOcean = a.IsOcean,
                              IsInland = a.IsInland,
                              IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                              IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                              IsPayable = a.IsPayable,
                              IsReceivable = a.IsReceivable,
                              VatTypeId = a.VatTypeId,
                              DueTypeCode = a.DueTypeCode,
                              IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                              ContainerMeasurementId = a.ContainerMeasurementId,
                              ContainerMeasurementCode = a.ContainerMeasurement != null ? a.ContainerMeasurement.Code : null,
                              ViewOrder = a.ViewOrder,
                              SearchFields = a.SearchFields,
                              ReceivableAccountId = a.ReceivableAccountId,
                              PayableAccountId = a.PayableAccountId,
                              AccountingVATSplit = a.AccountingVATSplit,
                              ReceivableCreditAccount = a.ReceivableCreditAccount,
                              PayableDebitAccount = a.PayableDebitAccount,
                              ReceivablesChargesTypeExtCode = a.ReceivablesChargesTypeExtCode,
                              PayablesChargesTypeExtCode = a.PayablesChargesTypeExtCode,
                              PayableDebitGLAcountId = a.PayableDebitGLAcountId,
                              ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
                              RecCreditGLAcountLocalName = a.RecCreditGLAcountLocalName,
                              PayDebitGLAcountLocalName = a.PayDebitGLAcountLocalName,
                              IsAutoDisplayInCustoms = a.IsAutoDisplayInCustoms,
                              IsCustoms = a.IsCustoms,
                              IsBackToBack = a.IsBackToBack,
                              SATExternalId = a.SATExternalId,
                              IsExpense = a.IsExpense,
                              IsDomestic = a.IsDomestic,
                              IsImport = a.IsImport,
                              IsDrop = a.IsDrop,
                              IsExport = a.IsExport,
                              ReceivablesDefaultCurrencyId = a.ReceivablesDefaultCurrencyId,
                              PayablesDefaultCurrencyId = a.PayablesDefaultCurrencyId,
                              ApplyRegionalTax = a.ApplyRegionalTax,
                              HasPickup = a.HasPickup,
                              HasDelivery = a.HasDelivery,
                              IsDirectionRestricted = a.IsDirectionRestricted,
                              IsActiveInExport = a.IsActiveInExport,
                              IsActiveInImport = a.IsActiveInImport,
                              IsActiveInDrop = a.IsActiveInDrop,
                              IsActiveInDomestic = a.IsActiveInDomestic,
                          }).FirstOrDefault();


                ChargeTypeAccountingQuery chargeTypeAccountingQuery = new ChargeTypeAccountingQuery(tenant);
                entity.ChargeTypeAccountings = chargeTypeAccountingQuery.GetChargeTypeAccountingsForChargeType(entity.Id, tenant).ToList();
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", Tenant = tenant, Type = "PM", Entities = new List<ChargesTypePM> { entity }.Cast<object>().ToList() }).Set();

            }

            ChargesTypePM securedPm = new ChargesTypePM();
            SecuredMapping.GetMappedPM(entity, securedPm, "ChargesType", tenant);

            return securedPm;
        }



        public List<ChargesTypePM> GetChargesTypesByCode(string code, int tenant)
        {


            List<ChargesTypePM> Entities = (from a in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement")
                              where a.Code == code && a.Tenant == tenant  
                              select new ChargesTypePM()
                              {

                                  Code = a.Code,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  VatTypeId = a.VatTypeId,
                                  ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
                                  Tenant = a.Tenant,

                              }).ToList();

            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", Tenant = tenant, Type = "PM", Entities = Entities.Cast<object>().ToList() }).Set();

            return Entities;



        }

        public IQueryable<ChargesTypePM> GetChargesTypePMsByTenant(int tenant)
        {
            IQueryable<ChargesTypePM> charges = from a in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("ReceivableAccount").Include("PayableAccount")
                                                where a.Tenant == tenant
                                                select new ChargesTypePM()
                                                {
                                                    AddedManually = a.AddedManually,
                                                    Code = a.Code,
                                                    MeasurementId = a.MeasurementId,
                                                    MeasurementCode = a.Measurement.Code,
                                                    MeasurementShortName = a.Measurement.ShortName,
                                                    Id = a.Id,
                                                    InActive = a.InActive,
                                                    LocalName = a.LocalName,
                                                    EnglishName = a.EnglishName,
                                                    Tenant = a.Tenant,
                                                    AWBPrintDescription = a.AWBPrintDescription,
                                                    ChargesGroupCode = a.ChargesGroupCode,
                                                    ChargesGroupId = a.ChargesGroupId,
                                                    QuoteChargesGroupCode = a.QuoteChargesGroupCode,
                                                    QuoteChargesGroupId = a.QuoteChargesGroupId,
                                                    IATACodeId = a.IATACodeId,
                                                    Description = a.Description,
                                                    IsAir = a.IsAir,
                                                    IsOcean = a.IsOcean,
                                                    IsInland = a.IsInland,
                                                    IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                                                    IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                                                    IsPayable = a.IsPayable,
                                                    IsReceivable = a.IsReceivable,
                                                    VatTypeId = a.VatTypeId,
                                                    DueTypeCode = a.DueTypeCode,
                                                    IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                                                    ContainerMeasurementId = a.ContainerMeasurementId,
                                                    ContainerMeasurementCode = a.ContainerMeasurement != null ? a.ContainerMeasurement.Code : null,
                                                    ViewOrder = a.ViewOrder,
                                                    SearchFields = a.SearchFields,
                                                    ReceivableAccountId = a.ReceivableAccount != null ? a.ReceivableAccount.Id : null,
                                                    PayableAccountId = a.PayableAccount != null ? a.PayableAccount.Id : null,
                                                    AccountingVATSplit = a.AccountingVATSplit,
                                                    ReceivableCreditAccount = a.ReceivableCreditAccount,
                                                    PayableDebitAccount = a.PayableDebitAccount,
                                                    ReceivablesChargesTypeExtCode = a.ReceivablesChargesTypeExtCode,
                                                    PayablesChargesTypeExtCode=a.PayablesChargesTypeExtCode,
                                                    PayableDebitGLAcountId = a.PayableDebitGLAcountId,
                                                    ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
                                                    RecCreditGLAcountLocalName = a.RecCreditGLAcountLocalName,
                                                    PayDebitGLAcountLocalName = a.PayDebitGLAcountLocalName,
                                                    IsAutoDisplayInCustoms = a.IsAutoDisplayInCustoms,
                                                    IsCustoms = a.IsCustoms,
                                                    IsBackToBack = a.IsBackToBack,
                                                    SATExternalId = a.SATExternalId,
                                                    IsExpense = a.IsExpense,
                                                    IsDomestic = a.IsDomestic,
                                                    IsImport = a.IsImport,
                                                    IsDrop = a.IsDrop,
                                                    IsExport = a.IsExport,
                                                    ReceivablesDefaultCurrencyId = a.ReceivablesDefaultCurrencyId,
                                                    PayablesDefaultCurrencyId = a.PayablesDefaultCurrencyId,
                                                    ApplyRegionalTax = a.ApplyRegionalTax,
                                                    HasPickup = a.HasPickup,
                                                    HasDelivery = a.HasDelivery,
                                                    IsDirectionRestricted = a.IsDirectionRestricted,
                                                    IsActiveInExport = a.IsActiveInExport,
                                                    IsActiveInImport = a.IsActiveInImport,
                                                    IsActiveInDrop = a.IsActiveInDrop,
                                                    IsActiveInDomestic = a.IsActiveInDomestic,
                                                };
            return charges;
        }

        public IQueryable<ChargesTypePM> GetChargesTypeByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement")
                         where a.Tenant == tenant
                         select new ChargesTypePM()
                         {
                             AddedManually = a.AddedManually,
                             Code = a.Code,
                             MeasurementId = a.MeasurementId,
                             MeasurementCode = a.Measurement.Code,
                             MeasurementShortName = a.Measurement.ShortName,
                             Id = a.Id,
                             InActive = a.InActive,
                             LocalName = a.LocalName,
                             EnglishName = a.EnglishName,
                             Tenant = a.Tenant,
                             AWBPrintDescription = a.AWBPrintDescription,
                             ChargesGroupCode = a.ChargesGroupCode,
                             ChargesGroupId = a.ChargesGroupId,
                             QuoteChargesGroupCode = a.QuoteChargesGroupCode,
                             QuoteChargesGroupId = a.QuoteChargesGroupId,
                             IATACodeId = a.IATACodeId,
                             Description = a.Description,
                             IsAir = a.IsAir,
                             IsOcean = a.IsOcean,
                             IsInland = a.IsInland,
                             IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                             IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                             IsPayable = a.IsPayable,
                             IsReceivable = a.IsReceivable,
                             VatTypeId = a.VatTypeId,
                             DueTypeCode = a.DueTypeCode,
                             IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                             ContainerMeasurementId = a.ContainerMeasurementId,
                             ContainerMeasurementCode = a.ContainerMeasurement != null ? a.ContainerMeasurement.Code : null,
                             ViewOrder = a.ViewOrder,
                             SearchFields = a.SearchFields,
                             ReceivableAccountId = a.ReceivableAccountId,
                             PayableAccountId = a.PayableAccountId,
                             AccountingVATSplit = a.AccountingVATSplit,
                             ReceivableCreditAccount = a.ReceivableCreditAccount,
                             PayableDebitAccount = a.PayableDebitAccount,
                             ReceivablesChargesTypeExtCode = a.ReceivablesChargesTypeExtCode,
                             PayablesChargesTypeExtCode=a.PayablesChargesTypeExtCode,
                             PayableDebitGLAcountId = a.PayableDebitGLAcountId,
                             ReceivableCreditGLAccountId = a.ReceivableCreditGLAccountId,
                             RecCreditGLAcountLocalName = a.RecCreditGLAcountLocalName,
                             PayDebitGLAcountLocalName = a.PayDebitGLAcountLocalName,
                             IsAutoDisplayInCustoms = a.IsAutoDisplayInCustoms,
                             IsCustoms = a.IsCustoms,
                             IsBackToBack = a.IsBackToBack,
                             SATExternalId = a.SATExternalId,
                             IsExpense = a.IsExpense,
                             IsDomestic = a.IsDomestic,
                             IsImport = a.IsImport,
                             IsDrop = a.IsDrop,
                             IsExport = a.IsExport,
                             ReceivablesDefaultCurrencyId = a.ReceivablesDefaultCurrencyId,
                             PayablesDefaultCurrencyId = a.PayablesDefaultCurrencyId,
                             ApplyRegionalTax = a.ApplyRegionalTax,
                             HasPickup = a.HasPickup,
                             HasDelivery = a.HasDelivery,
                             IsDirectionRestricted = a.IsDirectionRestricted,
                             IsActiveInExport = a.IsActiveInExport,
                             IsActiveInImport = a.IsActiveInImport,
                             IsActiveInDrop = a.IsActiveInDrop,
                             IsActiveInDomestic = a.IsActiveInDomestic,
                         }).AsQueryable();

            IQueryable<ChargesTypePM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }



        public IQueryable<ChargesTypeList> GetIQueryableEntityList(IQueryable<ChargesType> iQueryable)
        {
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("ChargesType");
            IQueryable<ChargesTypeList> result = from f in iQueryable.Include("Measurement").Include("ContainerMeasurement").Include("VatType").Include("ChargesGroup").Include("QuoteChargesGroup")
                                                 join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on f.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                 from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
                                                 select new ChargesTypeList()
                                                 {
                                                     AddedManually = f.AddedManually,
                                                     Id = f.Id,
                                                     InActive = f.InActive,
                                                     LocalName = f.LocalName,
                                                     EnglishName = f.EnglishName,
                                                     Code = f.Code,
                                                     Tenant = f.Tenant,
                                                     MeasurementId = f.MeasurementId,
                                                     MeasurementCode = f.Measurement != null ? f.Measurement.Code : null,
                                                     MeasurementShortName = f.Measurement != null ? f.Measurement.ShortName : null,
                                                     AWBPrintDescription = f.AWBPrintDescription,
                                                     ChargesGroupCode = f.ChargesGroupCode,
                                                     ChargesGroupId = f.ChargesGroupId,
                                                     QuoteChargesGroupCode = f.QuoteChargesGroupCode,
                                                     QuoteChargesGroupId = f.QuoteChargesGroupId,
                                                     IATACodeId = f.IATACodeId,
                                                     Description = f.Description,
                                                     IsAir = f.IsAir,
                                                     IsOcean = f.IsOcean,
                                                     IsInland = f.IsInland,
                                                     IsAutoDisplayInConsolidation = f.IsAutoDisplayInConsolidation,
                                                     IsAutoDisplayInShipment = f.IsAutoDisplayInShipment,
                                                     IsPayable = f.IsPayable,
                                                     IsReceivable = f.IsReceivable,
                                                     VatTypeId = f.VatTypeId,
                                                     VatTypeName = f.VatType == null ? "" : f.VatType.EnglishName,
                                                     VatIsMultiPercentage = f.VatType == null ? false : f.VatType.IsMultiPercentage,
                                                     DueTypeCode = f.DueTypeCode,
                                                     DueTypeName = f.DueType != null ? f.DueType.Name : null,
                                                     IsAutoDisplayInQuote = f.IsAutoDisplayInQuote,
                                                     ContainerMeasurementId = f.ContainerMeasurementId,
                                                     ContainerMeasurementCode = f.ContainerMeasurement != null ? f.ContainerMeasurement.Code : null,
                                                     ViewOrder = f.ViewOrder,
                                                     ChargesGroupName = f.ChargesGroup == null ? null : f.ChargesGroup.Name,
                                                     QuoteChargesGroupName = f.QuoteChargesGroup == null ? null : f.QuoteChargesGroup.Name,
                                                     SearchFields = f.SearchFields,
                                                     AccountingVATSplit = f.AccountingVATSplit,
                                                     ReceivableCreditAccount = f.ReceivableCreditAccount,
                                                     PayableDebitAccount = f.PayableDebitAccount,
                                                     ReceivablesChargesTypeExtCode = f.ReceivablesChargesTypeExtCode,
                                                     PayablesChargesTypeExtCode = f.PayablesChargesTypeExtCode,
                                                     PayableAccountId = f.PayableAccountId,
                                                     ReceivableAccountId = f.ReceivableAccountId,
                                                     PayableDebitGLAcountId = f.PayableDebitGLAcountId,
                                                     ReceivableCreditGLAccountId = f.ReceivableCreditGLAccountId,
                                                     RecCreditGLAcountLocalName = f.RecCreditGLAcountLocalName,
                                                     PayDebitGLAcountLocalName = f.PayDebitGLAcountLocalName,
                                                     IsBackToBack = f.IsBackToBack,
                                                     IsAutoDisplayInCustoms = f.IsAutoDisplayInCustoms,
                                                     IsCustoms = f.IsCustoms,
                                                     SATExternalId = f.SATExternalId,
                                                     IsExpense = f.IsExpense,
                                                     IsDomestic = f.IsDomestic,
                                                     IsImport = f.IsImport,
                                                     IsDrop = f.IsDrop,
                                                     IsExport = f.IsExport,
                                                     ReceivablesDefaultCurrencyId = f.ReceivablesDefaultCurrencyId,
                                                     PayablesDefaultCurrencyId = f.PayablesDefaultCurrencyId,
                                                     ApplyRegionalTax = f.ApplyRegionalTax,
                                                     HasPickup = f.HasPickup,
                                                     HasDelivery = f.HasDelivery,
                                                     IsDirectionRestricted = f.IsDirectionRestricted,
                                                     IsActiveInExport = f.IsActiveInExport,
                                                     IsActiveInImport = f.IsActiveInImport,
                                                     IsActiveInDrop = f.IsActiveInDrop,
                                                     IsActiveInDomestic = f.IsActiveInDomestic,
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
                                                     Field18= customFieldsMainObject != null ? customFieldsMainObject.Field18 : null,
                                                     Field19= customFieldsMainObject != null ? customFieldsMainObject.Field19 : null,
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
                                                 };
            return result;
        }



        public ChargesTypeList GetSingleChargesType(string id, int tenant)
        {
            ChargesTypeList chargesTypeList = (from f in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("VatType").Include("ChargesGroup").Include("QuoteChargesGroup")
                                               where f.Tenant == tenant && f.Id == id
                                               select new ChargesTypeList()
                                               {
                                                   AddedManually = f.AddedManually,
                                                   Id = f.Id,
                                                   InActive = f.InActive,
                                                   LocalName = f.LocalName,
                                                   EnglishName = f.EnglishName,
                                                   Code = f.Code,
                                                   Tenant = f.Tenant,
                                                   MeasurementId = f.MeasurementId,
                                                   MeasurementCode = f.Measurement != null ? f.Measurement.Code : null,
                                                   MeasurementShortName = f.Measurement != null ? f.Measurement.ShortName : null,
                                                   AWBPrintDescription = f.AWBPrintDescription,
                                                   ChargesGroupCode = f.ChargesGroupCode,
                                                   ChargesGroupId = f.ChargesGroupId,
                                                   QuoteChargesGroupCode = f.QuoteChargesGroupCode,
                                                   QuoteChargesGroupId = f.QuoteChargesGroupId,
                                                   IATACodeId = f.IATACodeId,
                                                   Description = f.Description,
                                                   IsAir = f.IsAir,
                                                   IsOcean = f.IsOcean,
                                                   IsInland = f.IsInland,
                                                   IsAutoDisplayInConsolidation = f.IsAutoDisplayInConsolidation,
                                                   IsAutoDisplayInShipment = f.IsAutoDisplayInShipment,
                                                   IsPayable = f.IsPayable,
                                                   IsReceivable = f.IsReceivable,
                                                   VatTypeId = f.VatTypeId,
                                                   VatTypeName = f.VatType == null ? "" : f.VatType.EnglishName,
                                                   VatIsMultiPercentage = f.VatType == null ? false : f.VatType.IsMultiPercentage,
                                                   DueTypeCode = f.DueTypeCode,
                                                   DueTypeName = f.DueType != null ? f.DueType.Name : null,
                                                   IsAutoDisplayInQuote = f.IsAutoDisplayInQuote,
                                                   ContainerMeasurementId = f.ContainerMeasurementId,
                                                   ContainerMeasurementCode = f.ContainerMeasurement != null ? f.ContainerMeasurement.Code : null,
                                                   ViewOrder = f.ViewOrder,
                                                   ChargesGroupName = f.ChargesGroup == null ? null : f.ChargesGroup.Name,
                                                   QuoteChargesGroupName = f.QuoteChargesGroup == null ? null : f.QuoteChargesGroup.Name,
                                                   SearchFields = f.SearchFields,
                                                   AccountingVATSplit = f.AccountingVATSplit,
                                                   ReceivableCreditAccount = f.ReceivableCreditAccount,
                                                   PayableDebitAccount = f.PayableDebitAccount,
                                                   ReceivablesChargesTypeExtCode = f.ReceivablesChargesTypeExtCode,
                                                   PayablesChargesTypeExtCode=f.PayablesChargesTypeExtCode,
                                                   PayableDebitGLAcountId = f.PayableDebitGLAcountId,
                                                   ReceivableCreditGLAccountId = f.ReceivableCreditGLAccountId,
                                                   RecCreditGLAcountLocalName = f.RecCreditGLAcountLocalName,
                                                   PayDebitGLAcountLocalName = f.PayDebitGLAcountLocalName,
                                                   IsBackToBack = f.IsBackToBack,
                                                   IsAutoDisplayInCustoms = f.IsAutoDisplayInCustoms,
                                                   IsCustoms = f.IsCustoms,
                                                   SATExternalId = f.SATExternalId,
                                                   IsExpense = f.IsExpense,
                                                   IsDomestic = f.IsDomestic,
                                                   IsImport = f.IsImport,
                                                   IsDrop = f.IsDrop,
                                                   IsExport = f.IsExport,
                                                   ReceivablesDefaultCurrencyId = f.ReceivablesDefaultCurrencyId,
                                                   PayablesDefaultCurrencyId = f.PayablesDefaultCurrencyId,
                                                   ApplyRegionalTax = f.ApplyRegionalTax,
                                                   HasPickup = f.HasPickup,
                                                   HasDelivery = f.HasDelivery,
                                                   IsDirectionRestricted = f.IsDirectionRestricted,
                                                   IsActiveInExport = f.IsActiveInExport,
                                                   IsActiveInImport = f.IsActiveInImport,
                                                   IsActiveInDrop = f.IsActiveInDrop,
                                                   IsActiveInDomestic = f.IsActiveInDomestic,

                                               }).FirstOrDefault();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", Tenant = tenant, Type = "List", Entities = new List<ChargesTypeList> { chargesTypeList }.Cast<object>().ToList() }).Set();

            return chargesTypeList;
        }

        public IQueryable<ChargesTypeList> GetChargesTypeLists(int tenant, int skip, int take)
        {
            IQueryable<ChargesTypeList> query = (from f in repository.context.ChargesTypes.Include("Measurement").Include("ContainerMeasurement").Include("VatType").Include("ChargesGroup").Include("QuoteChargesGroup")
                                                 where f.Tenant == tenant
                         select new ChargesTypeList()
                         {
                             AddedManually = f.AddedManually,
                             Id = f.Id,
                             InActive = f.InActive,
                             LocalName = f.LocalName,
                             EnglishName = f.EnglishName,
                             Code = f.Code,
                             Tenant = f.Tenant,
                             MeasurementId = f.MeasurementId,
                             MeasurementCode = f.Measurement != null ? f.Measurement.Code : null,
                             MeasurementShortName = f.Measurement != null ? f.Measurement.ShortName : null,
                             AWBPrintDescription = f.AWBPrintDescription,
                             ChargesGroupCode = f.ChargesGroupCode,
                             ChargesGroupId = f.ChargesGroupId,
                             QuoteChargesGroupCode = f.QuoteChargesGroupCode,
                             QuoteChargesGroupId = f.QuoteChargesGroupId,
                             IATACodeId = f.IATACodeId,
                             Description = f.Description,
                             IsAir = f.IsAir,
                             IsOcean = f.IsOcean,
                             IsInland = f.IsInland,
                             IsAutoDisplayInConsolidation = f.IsAutoDisplayInConsolidation,
                             IsAutoDisplayInShipment = f.IsAutoDisplayInShipment,
                             IsPayable = f.IsPayable,
                             IsReceivable = f.IsReceivable,
                             VatTypeId = f.VatTypeId,
                             VatTypeName = f.VatType == null ? "" : f.VatType.EnglishName,
                             VatIsMultiPercentage = f.VatType == null ? false : f.VatType.IsMultiPercentage,
                             DueTypeCode = f.DueTypeCode,
                             DueTypeName = f.DueType != null ? f.DueType.Name : null,
                             IsAutoDisplayInQuote = f.IsAutoDisplayInQuote,
                             ContainerMeasurementId = f.ContainerMeasurementId,
                             ContainerMeasurementCode = f.ContainerMeasurement != null ? f.ContainerMeasurement.Code : null,
                             ViewOrder = f.ViewOrder,
                             ChargesGroupName = f.ChargesGroup == null ? null : f.ChargesGroup.Name,
                             QuoteChargesGroupName = f.QuoteChargesGroup == null ? null : f.QuoteChargesGroup.Name,
                             SearchFields = f.SearchFields,
                             AccountingVATSplit = f.AccountingVATSplit,
                             ReceivableCreditAccount = f.ReceivableCreditAccount,
                             PayableDebitAccount = f.PayableDebitAccount,
                             ReceivablesChargesTypeExtCode = f.ReceivablesChargesTypeExtCode,
                             PayablesChargesTypeExtCode = f.PayablesChargesTypeExtCode,
                             PayableAccountId = f.PayableAccountId,
                             ReceivableAccountId = f.ReceivableAccountId,
                             PayableDebitGLAcountId = f.PayableDebitGLAcountId,
                             ReceivableCreditGLAccountId = f.ReceivableCreditGLAccountId,
                             RecCreditGLAcountLocalName = f.RecCreditGLAcountLocalName,
                             PayDebitGLAcountLocalName = f.PayDebitGLAcountLocalName,
                             IsBackToBack = f.IsBackToBack,
                             IsAutoDisplayInCustoms = f.IsAutoDisplayInCustoms,
                             IsCustoms = f.IsCustoms,
                             SATExternalId = f.SATExternalId,
                             IsExpense = f.IsExpense,
                             IsDomestic = f.IsDomestic,
                             IsImport = f.IsImport,
                             IsDrop = f.IsDrop,
                             IsExport = f.IsExport,
                             ReceivablesDefaultCurrencyId = f.ReceivablesDefaultCurrencyId,
                             PayablesDefaultCurrencyId = f.PayablesDefaultCurrencyId,
                             ApplyRegionalTax = f.ApplyRegionalTax,
                             HasPickup = f.HasPickup,
                             HasDelivery = f.HasDelivery,
                             IsDirectionRestricted = f.IsDirectionRestricted,
                             IsActiveInExport = f.IsActiveInExport,
                             IsActiveInImport = f.IsActiveInImport,
                             IsActiveInDrop = f.IsActiveInDrop,
                             IsActiveInDomestic = f.IsActiveInDomestic,

                         }).OrderBy(d=>d.Code).Skip(skip).Take(take);


            return query;
        }

        public IQueryable<ChargesTypeList> GetChargesTypeListsByTenant(int tenant)
        {
            IQueryable<ChargesTypeList> charges = from a in repository.context.ChargesTypes
                                                where a.Tenant == tenant
                                                select new ChargesTypeList()
                                                {
                                                  Id= a.Id,
                                                  Code = a.Code,
                                                  ViewOrder = a.ViewOrder, 
                                                  ChargesGroupId = a.ChargesGroupId,
                                                  QuoteChargesGroupId = a.QuoteChargesGroupId,
                                                };
            return charges;
        }

        public ChargesTypeList GetSingleChargesTypeListByCode(string code, int tenant)
        {
            ChargesTypeList chargesTypeList = (from f in repository.context.ChargesTypes.Include("Measurement").Include("VatType").Include("ChargesGroup").Include("QuoteChargesGroup")
                                               where f.Tenant == tenant && f.Code == code
                                               select new ChargesTypeList()
                                               {
                                                   AddedManually = f.AddedManually,
                                                   Id = f.Id,
                                                   InActive = f.InActive,
                                                   LocalName = f.LocalName,
                                                   EnglishName = f.EnglishName,
                                                   Code = f.Code,
                                                   Tenant = f.Tenant,
                                                   MeasurementId = f.MeasurementId,
                                                   MeasurementCode = f.Measurement != null ? f.Measurement.Code : null,
                                                   MeasurementShortName = f.Measurement != null ? f.Measurement.ShortName : null,
                                                   AWBPrintDescription = f.AWBPrintDescription,
                                                   ChargesGroupCode = f.ChargesGroupCode,
                                                   ChargesGroupId = f.ChargesGroupId,
                                                   QuoteChargesGroupCode = f.QuoteChargesGroupCode,
                                                   QuoteChargesGroupId = f.QuoteChargesGroupId,
                                                   IATACodeId = f.IATACodeId,
                                                   Description = f.Description,
                                                   IsAir = f.IsAir,
                                                   IsOcean = f.IsOcean,
                                                   IsInland = f.IsInland,
                                                   IsAutoDisplayInConsolidation = f.IsAutoDisplayInConsolidation,
                                                   IsAutoDisplayInShipment = f.IsAutoDisplayInShipment,
                                                   IsPayable = f.IsPayable,
                                                   IsReceivable = f.IsReceivable,
                                                   VatTypeId = f.VatTypeId,
                                                   VatTypeName = f.VatType == null ? "" : f.VatType.EnglishName,
                                                   VatIsMultiPercentage = f.VatType == null ? false : f.VatType.IsMultiPercentage,
                                                   DueTypeCode = f.DueTypeCode,
                                                   DueTypeName = f.DueType != null ? f.DueType.Name : null,
                                                   IsAutoDisplayInQuote = f.IsAutoDisplayInQuote,
                                                   ContainerMeasurementId = f.ContainerMeasurementId,
                                                   ViewOrder = f.ViewOrder,
                                                   ChargesGroupName = f.ChargesGroup == null ? null : f.ChargesGroup.Name,
                                                   QuoteChargesGroupName = f.QuoteChargesGroup == null ? null : f.QuoteChargesGroup.Name,
                                                   SearchFields = f.SearchFields,
                                                   AccountingVATSplit = f.AccountingVATSplit,
                                                   ReceivableCreditAccount = f.ReceivableCreditAccount,
                                                   PayableDebitAccount = f.PayableDebitAccount,
                                                   ReceivablesChargesTypeExtCode = f.ReceivablesChargesTypeExtCode,
                                                   PayablesChargesTypeExtCode = f.PayablesChargesTypeExtCode,
                                                   PayableDebitGLAcountId = f.PayableDebitGLAcountId,
                                                   ReceivableCreditGLAccountId = f.ReceivableCreditGLAccountId,
                                                   RecCreditGLAcountLocalName = f.RecCreditGLAcountLocalName,
                                                   PayDebitGLAcountLocalName = f.PayDebitGLAcountLocalName,
                                                   IsBackToBack = f.IsBackToBack,
                                                   IsAutoDisplayInCustoms = f.IsAutoDisplayInCustoms,
                                                   IsCustoms = f.IsCustoms,
                                                   SATExternalId = f.SATExternalId,
                                                   IsExpense = f.IsExpense,
                                                   IsDomestic = f.IsDomestic,
                                                   IsImport = f.IsImport,
                                                   IsDrop = f.IsDrop,
                                                   IsExport = f.IsExport,
                                                   ReceivablesDefaultCurrencyId = f.ReceivablesDefaultCurrencyId,
                                                   PayablesDefaultCurrencyId = f.PayablesDefaultCurrencyId,
                                                   ApplyRegionalTax = f.ApplyRegionalTax,
                                                   HasPickup = f.HasPickup,
                                                   HasDelivery = f.HasDelivery,
                                                   IsDirectionRestricted = f.IsDirectionRestricted,
                                                   IsActiveInExport = f.IsActiveInExport,
                                                   IsActiveInImport = f.IsActiveInImport,
                                                   IsActiveInDrop = f.IsActiveInDrop,
                                                   IsActiveInDomestic = f.IsActiveInDomestic,

                                               }).FirstOrDefault();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", Tenant = tenant, Type = "List", Entities = new List<ChargesTypeList> { chargesTypeList }.Cast<object>().ToList() }).Set();

            return chargesTypeList;
        }

        public List<string> GetChargesTypesIdsByChargeGroupCodeAndTenant(string chargeGroupCode, int tenant)
        {
            List<string> chargesTypesIds = (from a in repository.context.ChargesTypes
                                            where a.ChargesGroupCode == chargeGroupCode && a.Tenant == tenant
                                            select a.Id).ToList();

            return chargesTypesIds;
        }
    }
}
