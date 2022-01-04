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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TruckerQuery
    {
        TruckerRepository repository;

        public TruckerQuery()
        {
            repository = new TruckerRepository(); 
        }

        public TruckerQuery(int tenant)
        {
            repository = new TruckerRepository(tenant);
        }

        public TruckerQuery(TruckerRepository repository)
        {
            this.repository = repository;
        }

        public TruckerPM GetSinglePM(string id, int tenant)
        {
            var trucker = (from a in repository.context.Truckers.Include("Card")
                           where a.Tenant == tenant && a.Id == id
                           select new TruckerPM()
                           {
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
                               AccountingVATSplit = a.Card.AccountingVATSplit,
                               BillToId = a.Card.BillToId,
                               AddedManually = a.AddedManually,
                               Id = a.Id,
                               Remark = a.Card.Notes,
                               Tenant = a.Tenant,
                               VatNumber = a.Card.VatNumber,
                               Code = a.Card.Code,
                               EnglishName = a.Card.EnglishName,
                               LocalName = a.Card.LocalName,
                               CarrierTypeId = a.Card.PartnerTypeId,
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
                               SATForeignRFC = a.Card.SATForeignRFC,
                               MetodoPagoCode = a.Card.MetodoPagoCode,
                               UsoCFDICode = a.Card.UsoCFDICode,
                               GLAccountId = a.Card.GLAccountId,
                               GLAccountNumber = a.Card.GLAccountDisplayNumber,
                               Card = new CardPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   EnglishName = a.Card.EnglishName,
                                   PrimaryContactId = a.Card.PrimaryContactId,
                                   GLAccountDisplayNumber = a.Card.GLAccountDisplayNumber,
                               },
                           }).FirstOrDefault();
            
            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            trucker.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(trucker.Id, trucker.Tenant);


            if (trucker != null)
            {
                trucker.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        trucker.IsExternal = true;
                    }
                }
            }

            TruckerPM securedPm = new TruckerPM();
            SecuredMapping.GetMappedPM(trucker, securedPm, "Trucker", tenant);
            if (securedPm != null && trucker != null)
            {
                Trucker entityPoco = (from s in repository.context.Truckers where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }
            return securedPm;
        }

        private void MapCustomFields(TruckerPM trucker, Trucker entityPoco)
        {
            trucker.Field1 = new CustomFieldClass("Field1", "Trucker", entityPoco.Field1);
            trucker.Field2 = new CustomFieldClass("Field2", "Trucker", entityPoco.Field2);
            trucker.Field3 = new CustomFieldClass("Field3", "Trucker", entityPoco.Field3);
            trucker.Field4 = new CustomFieldClass("Field4", "Trucker", entityPoco.Field4);
            trucker.Field5 = new CustomFieldClass("Field5", "Trucker", entityPoco.Field5);
            trucker.Field6 = new CustomFieldClass("Field6", "Trucker", entityPoco.Field6);
            trucker.Field7 = new CustomFieldClass("Field7", "Trucker", entityPoco.Field7);
            trucker.Field8 = new CustomFieldClass("Field8", "Trucker", entityPoco.Field8);
            trucker.Field9 = new CustomFieldClass("Field9", "Trucker", entityPoco.Field9);
            trucker.Field10 = new CustomFieldClass("Field10", "Trucker", entityPoco.Field10);
        }

        public IQueryable<TruckerPM> GetTruckerPMsByTenant(int tenant)
        {
            IQueryable<TruckerPM> truckers = from a in repository.context.Truckers.Include("Card")
                                             where a.Tenant == tenant
                                             select new TruckerPM()
                                             {
                                                 ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                 PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                 AccountingVATSplit = a.Card.AccountingVATSplit,
                                                 BillToId = a.Card.BillToId,
                                                 AddedManually = a.AddedManually,
                                                 Id = a.Id,
                                                 Remark = a.Card.Notes,
                                                 Tenant = a.Tenant,
                                                 VatNumber = a.Card.VatNumber,
                                                 Code = a.Card.Code,
                                                 EnglishName = a.Card.EnglishName,
                                                 LocalName = a.Card.LocalName,
                                                 CarrierTypeId = a.Card.PartnerTypeId,
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
                                                 EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                 IRSNumber = a.Card.IRSNumber,
                                                 IRSPlace = a.Card.IRSPlace,
                                                 ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                 PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                 ExternalId2 = a.Card.ExternalId2,
                                                 SATForeignRFC = a.Card.SATForeignRFC,
                                                 MetodoPagoCode = a.Card.MetodoPagoCode,
                                                 UsoCFDICode = a.Card.UsoCFDICode,
                                             };
            return truckers;
        }

        public IQueryable<TruckerPM> GetTruckersByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Truckers.Include("Card")
                         where a.Tenant == tenant
                         select new TruckerPM()
                         {
                             ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                             PayablesAccountingCard = a.Card.PayablesAccountingCard,
                             AccountingVATSplit = a.Card.AccountingVATSplit,
                             BillToId = a.Card.BillToId,
                             AddedManually = a.AddedManually,
                             Id = a.Id,
                             Remark = a.Card.Notes,
                             Tenant = a.Tenant,
                             VatNumber = a.Card.VatNumber,
                             Code = a.Card.Code,
                             EnglishName = a.Card.EnglishName,
                             LocalName = a.Card.LocalName,
                             CarrierTypeId = a.Card.PartnerTypeId,
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
                             EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                             IRSNumber = a.Card.IRSNumber,
                             IRSPlace = a.Card.IRSPlace,
                             ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                             PaymentMethodCode = a.Card.SATPaymentMethodCode,
                             ExternalId2 = a.Card.ExternalId2,
                             SATForeignRFC = a.Card.SATForeignRFC,
                             MetodoPagoCode = a.Card.MetodoPagoCode,
                             UsoCFDICode = a.Card.UsoCFDICode,
                         }).AsQueryable();

            IQueryable<TruckerPM> query2 = null;
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

        public IQueryable<TruckerList> GetIQueryableEntityList(IQueryable<Trucker> iQueryable)
        {
            IQueryable<TruckerList> result = (from a in iQueryable.Include("Card").Include("Card.PaymentTerm")
                                              select new TruckerList()
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
                                                  PaymentTermEnglishName = (a.Card.PaymentTerm != null ? a.Card.PaymentTerm.EnglishName : ""),
                                                  SearchFields = a.Card.SearchFields,
                                                  Notes = a.Card.Notes,
                                                  Website = a.Card.Website,
                                                  InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                  VatTypeId = a.Card.VatTypeId,
                                                  EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                  CityName = a.Card.CityName,
                                                  CountryId = a.Card.CountryId,
                                                  CountryCode = a.Card.CountryCode,
                                                  CountryName = a.Card.CountryName,
                                                  ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                  PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                  ExternalId2 = a.Card.ExternalId2,
                                                  SATForeignRFC = a.Card.SATForeignRFC,
                                                  MetodoPagoCode = a.Card.MetodoPagoCode,
                                                  UsoCFDICode = a.Card.UsoCFDICode,
                                                  PrimaryContactName = a.PrimaryContactName,
                                                  PrimaryContactEmail = a.PrimaryContactEmail,
                                                  PrimaryContactPhone = a.PrimaryContactPhone,
                                                  StateName = a.Card.StateName,
                                                  GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                                  Field1 = a.Field1,
                                                  Field2 = a.Field2,
                                                  Field3 = a.Field3,
                                                  Field4 = a.Field4,
                                                  Field5 = a.Field5,
                                                  Field6 = a.Field6,
                                                  Field7 = a.Field7,
                                                  Field8 = a.Field8,
                                                  Field9 = a.Field9,
                                                  Field10 = a.Field10,
                                              });


            return result;
        }
 

        public TruckerPM GetSinglePMByCode(string code, int tenant)
        {
            var trucker = (from a in repository.context.Truckers.Include("Card")
                                where a.Card.Code == code && a.Tenant == tenant
                                select new TruckerPM()
                                {
                                    ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                    PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                    AccountingVATSplit = a.Card.AccountingVATSplit,
                                    BillToId = a.Card.BillToId,
                                    AddedManually = a.AddedManually,
                                    Id = a.Id,
                                    Remark = a.Card.Notes,
                                    Tenant = a.Tenant,
                                    VatNumber = a.Card.VatNumber,
                                    Code = a.Card.Code,
                                    EnglishName = a.Card.EnglishName,
                                    LocalName = a.Card.LocalName,
                                    CarrierTypeId = a.Card.PartnerTypeId,
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
                                    SATForeignRFC = a.Card.SATForeignRFC,
                                    MetodoPagoCode = a.Card.MetodoPagoCode,
                                    UsoCFDICode = a.Card.UsoCFDICode,

                                    Card = new CardPM()
                                    {
                                        Id = a.Id,
                                        Tenant = a.Tenant,
                                        EnglishName = a.Card.EnglishName,
                                        PrimaryContactId = a.Card.PrimaryContactId,
                                    },
                                }).FirstOrDefault();

            TruckerPM securedPm = new TruckerPM();
            SecuredMapping.GetMappedPM(trucker, securedPm, "Trucker", tenant);
            if (securedPm != null && trucker != null)
            {
                Trucker entityPoco = (from s in repository.context.Truckers where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }
            return trucker;
        }

        public bool CheckTruckerAddedManually(string id, int tenant)
        {
            bool addedManually = (from a in repository.context.Truckers
                           where a.Tenant == tenant && a.Id == id
                           select a.AddedManually).FirstOrDefault();

        
            return addedManually;
        }
    }
}
