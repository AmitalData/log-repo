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
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;

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
                                                         AccountingVATSplit = a.Card.AccountingVATSplit,
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
                                                         GLAccountId = a.Card.GLAccountId,
                                                         GLAccountNumber = a.Card.GLAccountDisplayNumber,
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
                                                         BillToId = a.Card.BillToId,
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
                                                         AccountingVATSplit = a.Card.AccountingVATSplit,
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
                                                         BillToId = a.Card.BillToId,
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
                                                         AccountingVATSplit = a.Card.AccountingVATSplit,
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
                                                         BillToId = a.Card.BillToId,
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
                                                                     AccountingVATSplit = a.Card.AccountingVATSplit,
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
                                                                     BillToId = a.Card.BillToId,
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
                             AccountingVATSplit = a.Card.AccountingVATSplit,
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
                             BillToId = a.Card.BillToId,
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


            IQueryable<AccountingPartnerList> result = (from a in iQueryable.Include("Card").Include("Card")
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
                                                            GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                                            CollectorId = a.Card.CollectorId, 
                                                        });


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
                                                         AccountingVATSplit = a.Card.AccountingVATSplit,
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
                                                         BillToId = a.Card.BillToId,
                                                         //??
                                                         CollectorId = a.Card.CollectorId,
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

        private void SetAccountingPartnerAddressData(AccountingPartnerPM entity)
        {
            AddressRepository addressRep = new AddressRepository(repository.context);
            AddressQuery addressQuery = new AddressQuery(addressRep);
            AddressPM entityAddress = addressQuery.GetAddressPMByTypeAndCard(entity.Id, "M", entity.Tenant);

            if (entityAddress != null)
            {
                 
                entity.CountryId = entityAddress.CountryId; 

                Country country = CountryRepository.GetSingleCountry(entityAddress.CountryId, entity.Tenant, true);
                if (country != null)
                {
                    entity.CountryCode = country.Code;
                    entity.CountryName = country.EnglishName;
                     
                } 
            }
        }

        public AccountingPartnerPM GetSinglePMForHybrid(string id, int tenant)
        {
            bool getFromCache = false;
            string entityName = "HybridAccountingPartnerPM" + id + tenant;
            AccountingPartnerPM entity;

            if (HttpContext.Current != null && getFromCache)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = GetAccountingPartnerById(id, tenant);

                    if (entity != null)
                    {
                        this.SetAccountingPartnerAddressData(entity);

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    entity = (AccountingPartnerPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                entity = GetAccountingPartnerById(id, tenant);

                if (entity != null)
                {
                    this.SetAccountingPartnerAddressData(entity);
                }
            }

            if (entity != null) return setAccountingPartnerSetting(tenant, entity);

            else
            {
                return null;
            }
        } 
        public AccountingPartnerPM GetSinglePMByCodeForHybrid(string code, int tenant, bool getFromCache)

        {
            string entityName = "HybridAccountingPartnerPM" + code + tenant;
            AccountingPartnerPM entity;

            if (HttpContext.Current != null && getFromCache)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = GetAccountingPartnerByCode(code, tenant);

                    if (entity != null)
                    {
                        this.SetAccountingPartnerAddressData(entity);

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    entity = (AccountingPartnerPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                entity = GetAccountingPartnerByCode(code, tenant);

                if (entity != null)
                {
                    this.SetAccountingPartnerAddressData(entity);
                }
            }

            if (entity != null) return setAccountingPartnerSetting(tenant, entity);

            else
            {
                return null;
            }
        }  
        public AccountingPartnerPM GetSinglePMByVatNumberForHybrid(string vatNumber, int tenant, bool getFromCache)


        {
            string entityName = "HybridAccountingPartnerPM" + vatNumber + tenant;
            AccountingPartnerPM entity;

            if (HttpContext.Current != null && getFromCache)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = GetAccountingPartnerByVatNumber(vatNumber, tenant);

                    if (entity != null)
                    {
                        this.SetAccountingPartnerAddressData(entity);

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    entity = (AccountingPartnerPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                entity = GetAccountingPartnerByVatNumber(vatNumber, tenant);

                if (entity != null)
                {
                    this.SetAccountingPartnerAddressData(entity);
                }
            }

            if (entity != null) return setAccountingPartnerSetting(tenant, entity);

            else
            {
                return null;
            }
        }

        private AccountingPartnerPM setAccountingPartnerSetting(int tenant, AccountingPartnerPM entity)
        {
            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);

            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);

            entity.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(entity.Id, entity.Tenant);


            entity.IsExternal = false;

            AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
            AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
            if (accountingSystem != null)
            {
                if (accountingSystem.IsExternalCodesFromTable)
                {
                    entity.IsExternal = true;
                }
            }

            if (!string.IsNullOrEmpty(entity.ReceivablesAccountingCard))
            {
                int accountingCard = 0;
                bool isParsed = Int32.TryParse(entity.ReceivablesAccountingCard, out accountingCard);

                if (isParsed)
                {
                    TenantManagement tenantManagement = null;

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                        tenantManagement = tenantManagementRepository.GetSingleTenantManagement(accountingCard);

                        scope.Complete();
                    }

                    if (tenantManagement != null)
                    {
                        AirlineRepository airlineRepository = new AirlineRepository(repository.context);
                        List<Airline> tenantZeroAirlines = airlineRepository.GetAirlines(0).ToList();

                        if (tenantZeroAirlines.Count > 0)
                        {
                            if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                            {
                                tenantZeroAirlines = tenantZeroAirlines.Where(d => d.GLSHKPIMA != null).ToList();
                            }

                            else
                            {
                                tenantZeroAirlines = tenantZeroAirlines.Where(d => d.TTY != null).ToList();
                            }

                            string requested = "";
                            string registered = "";
                            string pending = "";

                            List<Airline> currenctTenantAilines = airlineRepository.GetAirlines(tenantManagement.Id).ToList();
                            foreach (Airline tenantZeroItem in tenantZeroAirlines)
                            {
                                Airline myTenantItem = currenctTenantAilines.Where(d => d.Card.Code == tenantZeroItem.Card.Code).FirstOrDefault();

                                if (myTenantItem != null)
                                {
                                    if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                                    {
                                        if (myTenantItem.GLSHKRegistrationRequested)
                                        {
                                            if (string.IsNullOrEmpty(requested))
                                            {
                                                requested = myTenantItem.Card.Code;
                                            }

                                            else
                                            {
                                                requested = requested + ", " + myTenantItem.Card.Code;
                                            }
                                        }

                                        if (myTenantItem.IsGLSHKRegistered)
                                        {
                                            if (string.IsNullOrEmpty(registered))
                                            {
                                                registered = myTenantItem.Card.Code;
                                            }

                                            else
                                            {
                                                registered = registered + ", " + myTenantItem.Card.Code;
                                            }
                                        }

                                        if (myTenantItem.GLSHKRegistrationRequested && !myTenantItem.IsGLSHKRegistered)
                                        {
                                            if (string.IsNullOrEmpty(pending))
                                            {
                                                pending = myTenantItem.Card.Code;
                                            }

                                            else
                                            {
                                                pending = pending + ", " + myTenantItem.Card.Code;
                                            }
                                        }
                                    }

                                    else
                                    {
                                        if (myTenantItem.ChampRegistrationRequested)
                                        {
                                            if (string.IsNullOrEmpty(requested))
                                            {
                                                requested = myTenantItem.Card.Code;
                                            }

                                            else
                                            {
                                                requested = requested + ", " + myTenantItem.Card.Code;
                                            }
                                        }

                                        if (myTenantItem.IsChampRegistered)
                                        {
                                            if (string.IsNullOrEmpty(registered))
                                            {
                                                registered = myTenantItem.Card.Code;
                                            }

                                            else
                                            {
                                                registered = registered + ", " + myTenantItem.Card.Code;
                                            }
                                        }

                                        if (myTenantItem.ChampRegistrationRequested && !myTenantItem.IsChampRegistered)
                                        {
                                            if (string.IsNullOrEmpty(pending))
                                            {
                                                pending = myTenantItem.Card.Code;
                                            }

                                            else
                                            {
                                                pending = pending + ", " + myTenantItem.Card.Code;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            AccountingPartnerPM securedPm = new AccountingPartnerPM();
            SecuredMapping.GetMappedPM(entity, securedPm, "AccountingPartner", tenant);

            if (securedPm != null && entity != null)
            {
                AccountingPartner entityPOC = (from s in repository.context.AccountingPartners where s.Id == securedPm.Id select s).FirstOrDefault();

            }
            return securedPm;
        } 
        private AccountingPartnerPM GetAccountingPartnerByVatNumber(string vatNumber, int tenant)
        {
            return (from a in repository.context.AccountingPartners.Include("Card")
                    where a.Tenant == tenant && a.Card.VatNumber == vatNumber
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
                        AccountingVATSplit = a.Card.AccountingVATSplit,
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
                        BillToId = a.Card.BillToId,
                    }).FirstOrDefault();
        }
        private AccountingPartnerPM GetAccountingPartnerByCode(string code, int tenant)
        {
            return (from a in repository.context.AccountingPartners.Include("Card")
                    where a.Tenant == tenant && a.Card.Code == code
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
                        AccountingVATSplit = a.Card.AccountingVATSplit,
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
                        BillToId = a.Card.BillToId,
                    }).FirstOrDefault();
        }
        private AccountingPartnerPM GetAccountingPartnerById(string id, int tenant)
        {
            return (from a in repository.context.AccountingPartners.Include("Card")
                    where a.Tenant == tenant && a.Id == id
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
                        AccountingVATSplit = a.Card.AccountingVATSplit,
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
                        BillToId = a.Card.BillToId,
                    }).FirstOrDefault();
        }
    }
}
