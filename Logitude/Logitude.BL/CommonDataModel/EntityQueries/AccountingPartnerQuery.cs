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
    public class AccountingPartnerQuery
    {
        AccountingPartnerRepository repository;

        public AccountingPartnerQuery()
        {
            repository = new AccountingPartnerRepository(); 
        }

        public AccountingPartnerQuery(int tenant)
        {
            repository = new AccountingPartnerRepository(tenant);
        }

        public AccountingPartnerQuery(AccountingPartnerRepository repository)
        {
            this.repository = repository;
        }

        public AccountingPartnerPM GetSinglePM(string id, int tenant)
        {
            AccountingPartnerPM AccountingPartner = (from a in repository.context.AccountingPartners.Include("Card")
                               where a.Id == id && a.Tenant == tenant
                               select new AccountingPartnerPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   Code = a.Card.Code,
                                   EnglishName = a.Card.EnglishName,
                                   LocalName = a.Card.LocalName,
                                   CardPMId = a.Id,
                                   ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                   PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                   CreateDate = a.Card.CreateDate,
                                   InActive = a.Card.InActive,
                                   Notes = a.Card.Notes,
                                   PartnerTypeId = a.Card.PartnerTypeId,
                                   PaymentTermId = a.Card.PaymentTermId,
                                   VatNumber = a.Card.VatNumber,
                                   ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                   Website = a.Card.Website,
                                   InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
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
                                       PartnerTypeId = a.Card.PartnerTypeId,
                                       Code = a.Card.Code,
                                   },
                               }).FirstOrDefault();

            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            AccountingPartner.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(AccountingPartner.Id, AccountingPartner.Tenant);

            if (AccountingPartner != null)
            {
                AccountingPartner.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        AccountingPartner.IsExternal = true;
                    }
                }
            }

            AccountingPartnerPM securedPm = new AccountingPartnerPM();
            SecuredMapping.GetMappedPM(AccountingPartner, securedPm, "AccountingPartner", tenant);

            return securedPm;
        }

        public AccountingPartnerPM GetSingleAccountingPartnerPM(string id, int tenant)
        {
            AccountingPartnerPM AccountingPartner = (from a in repository.context.AccountingPartners.Include("Card")
                               where a.Id == id && a.Tenant == tenant
                               select new AccountingPartnerPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   Code = a.Card.Code,
                                   EnglishName = a.Card.EnglishName,
                                   LocalName = a.Card.LocalName,
                                   CardPMId = a.Id,
                                   ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                   PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                   CreateDate = a.Card.CreateDate,
                                   InActive = a.Card.InActive,
                                   Notes = a.Card.Notes,
                                   PartnerTypeId = a.Card.PartnerTypeId,
                                   PaymentTermId = a.Card.PaymentTermId,
                                   VatNumber = a.Card.VatNumber,
                                   ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                   Website = a.Card.Website,
                                   InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
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

            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            AccountingPartner.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(AccountingPartner.Id, AccountingPartner.Tenant);

            if (AccountingPartner != null)
            {
                AccountingPartner.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        AccountingPartner.IsExternal = true;
                    }
                }
            }

            AccountingPartnerPM securedPm = new AccountingPartnerPM();
            SecuredMapping.GetMappedPM(AccountingPartner, securedPm, "AccountingPartner", tenant);

            return securedPm;
        }

        public AccountingPartnerPM GetSingleAccountingPartnerPM(int tenant, string id)
        {
            AccountingPartnerPM AccountingPartner = (from a in repository.context.AccountingPartners.Include("Card")
                               where a.Id == id && a.Tenant == tenant
                               select new AccountingPartnerPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   Code = a.Card.Code,
                                   EnglishName = a.Card.EnglishName,
                                   LocalName = a.Card.LocalName,
                                   CardPMId = a.Id,
                                   ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                   PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                   CreateDate = a.Card.CreateDate,
                                   InActive = a.Card.InActive,
                                   Notes = a.Card.Notes,
                                   PartnerTypeId = a.Card.PartnerTypeId,
                                   PaymentTermId = a.Card.PaymentTermId,
                                   VatNumber = a.Card.VatNumber,
                                   ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                   Website = a.Card.Website,
                                   InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
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

            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            AccountingPartner.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(AccountingPartner.Id, AccountingPartner.Tenant);
            
            if (AccountingPartner != null)
            {
                AccountingPartner.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        AccountingPartner.IsExternal = true;
                    }
                }
            }

            AccountingPartnerPM securedPm = new AccountingPartnerPM();
            SecuredMapping.GetMappedPM(AccountingPartner, securedPm, "AccountingPartner", tenant);

            return securedPm;
        }

        public IQueryable<AccountingPartnerPM> GetAccountingPartnerPMsByTenant(int tenant)
        {
            IQueryable<AccountingPartnerPM> AccountingPartners = from a in repository.context.AccountingPartners.Include("Card")
                                           where a.Tenant == tenant
                                           select new AccountingPartnerPM()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               Code = a.Card.Code,
                                               EnglishName = a.Card.EnglishName,
                                               LocalName = a.Card.LocalName,
                                               CardPMId = a.Id,
                                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                               CreateDate = a.Card.CreateDate,
                                               InActive = a.Card.InActive,
                                               Notes = a.Card.Notes,
                                               PartnerTypeId = a.Card.PartnerTypeId,
                                               PaymentTermId = a.Card.PaymentTermId,
                                               VatNumber = a.Card.VatNumber,
                                               Website = a.Card.Website,
                                               ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                               InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                               VatTypeId = a.Card.VatTypeId,
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
                                           };
            return AccountingPartners;
        }

        public IQueryable<AccountingPartnerPM> GetAccountingPartnersByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.AccountingPartners.Include("Card")
                         where a.Tenant == tenant
                         select new AccountingPartnerPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             Code = a.Card.Code,
                             EnglishName = a.Card.EnglishName,
                             LocalName = a.Card.LocalName,
                             CardPMId = a.Id,
                             ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                             PayablesAccountingCard = a.Card.PayablesAccountingCard,
                             CreateDate = a.Card.CreateDate,
                             InActive = a.Card.InActive,
                             Notes = a.Card.Notes,
                             PartnerTypeId = a.Card.PartnerTypeId,
                             PaymentTermId = a.Card.PaymentTermId,
                             VatNumber = a.Card.VatNumber,
                             Website = a.Card.Website,
                             ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                             InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                             VatTypeId = a.Card.VatTypeId,
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
                         }).AsQueryable();

            IQueryable<AccountingPartnerPM> query2 = null;
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

        public IQueryable<AccountingPartnerList> GetIQueryableEntityList(IQueryable<AccountingPartner> iQueryable)
        {
            IQueryable<AccountingPartnerList> result = from a in iQueryable.Include("Card")
                                            select new AccountingPartnerList()
                                            {
                                                Code = a.Card.Code,
                                                EnglishName = a.Card.EnglishName,
                                                LocalName = a.Card.LocalName,
                                                ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                InActive = a.Card.InActive,
                                                Notes = a.Card.Notes,
                                                PaymentTermEnglishName = (a.Card.PaymentTerm != null ? a.Card.PaymentTerm.EnglishName : ""),
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                VatNumber = a.Card.VatNumber,
                                                PaymentTermId = a.Card.PaymentTermId,
                                                InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                VatTypeId = a.Card.VatTypeId,
                                                Website = a.Card.Website,
                                                SearchFields = a.Card.SearchFields,
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
                                            };
            return result;
        }

        public AccountingPartnerPM GetSingleAccountingPartnerPMByCode(string code, int tenant)
        {
            AccountingPartnerPM AccountingPartner = (from a in repository.context.AccountingPartners.Include("Card")
                               where a.Card.Code == code && a.Tenant == tenant
                               select new AccountingPartnerPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   Code = a.Card.Code,
                                   EnglishName = a.Card.EnglishName,
                                   LocalName = a.Card.LocalName,
                                   CardPMId = a.Id,
                                   ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                   PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                   CreateDate = a.Card.CreateDate,
                                   InActive = a.Card.InActive,
                                   Notes = a.Card.Notes,
                                   PartnerTypeId = a.Card.PartnerTypeId,
                                   PaymentTermId = a.Card.PaymentTermId,
                                   VatNumber = a.Card.VatNumber,
                                   ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                   Website = a.Card.Website,
                                   InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
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
                                       PartnerTypeId = a.Card.PartnerTypeId,
                                       Code = a.Card.Code,
                                   },
                               }).FirstOrDefault();

            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            AccountingPartner.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(AccountingPartner.Id, AccountingPartner.Tenant);

            if (AccountingPartner != null)
            {
                AccountingPartner.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        AccountingPartner.IsExternal = true;
                    }
                }
            }

            AccountingPartnerPM securedPm = new AccountingPartnerPM();
            SecuredMapping.GetMappedPM(AccountingPartner, securedPm, "AccountingPartner", tenant);

            return securedPm;
        }
    }
}
