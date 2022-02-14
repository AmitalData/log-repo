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
                          ReceivablesChargesTypeExternalCode = a.ReceivablesChargesTypeExternalCode,
                          PayablesChargesTypeExternalCode = a.PayablesChargesTypeExternalCode,
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
                                        ReceivablesChargesTypeExternalCode = a.ReceivablesChargesTypeExternalCode,
                                        PayablesChargesTypeExternalCode = a.PayablesChargesTypeExternalCode,
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
                                  ReceivablesChargesTypeExternalCode = a.ReceivablesChargesTypeExternalCode,
                                  PayablesChargesTypeExternalCode = a.PayablesChargesTypeExternalCode,
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
                              ReceivablesChargesTypeExternalCode = a.ReceivablesChargesTypeExternalCode,
                              PayablesChargesTypeExternalCode = a.PayablesChargesTypeExternalCode,
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
                                                    ReceivablesChargesTypeExternalCode = a.ReceivablesChargesTypeExternalCode,
                                                    PayablesChargesTypeExternalCode=a.PayablesChargesTypeExternalCode,
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
                             ReceivablesChargesTypeExternalCode = a.ReceivablesChargesTypeExternalCode,
                             PayablesChargesTypeExternalCode=a.PayablesChargesTypeExternalCode,
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
            IQueryable<ChargesTypeList> result = from f in iQueryable.Include("Measurement").Include("ContainerMeasurement").Include("VatType").Include("ChargesGroup").Include("QuoteChargesGroup")
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
                                                     VatIsMultiPercentage= f.VatType == null ? false : f.VatType.IsMultiPercentage,
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
                                                     ReceivablesChargesTypeExternalCode = f.ReceivablesChargesTypeExternalCode,
                                                     PayablesChargesTypeExternalCode=f.PayablesChargesTypeExternalCode,
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
                                                   ReceivablesChargesTypeExternalCode = f.ReceivablesChargesTypeExternalCode,
                                                   PayablesChargesTypeExternalCode=f.PayablesChargesTypeExternalCode,
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
                             ReceivablesChargesTypeExternalCode = f.ReceivablesChargesTypeExternalCode,
                             PayablesChargesTypeExternalCode = f.PayablesChargesTypeExternalCode,
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
                                                   ReceivablesChargesTypeExternalCode = f.ReceivablesChargesTypeExternalCode,
                                                   PayablesChargesTypeExternalCode = f.PayablesChargesTypeExternalCode,
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

            return chargesTypeList;
        }
    }
}
