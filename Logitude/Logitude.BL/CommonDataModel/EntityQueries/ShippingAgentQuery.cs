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
    public class ShippingAgentQuery
    {
        ShippingAgentRepository repository;

        public ShippingAgentQuery()
        {
            repository = new ShippingAgentRepository(); 
        }

        public ShippingAgentQuery(int tenant)
        {
            repository = new ShippingAgentRepository(tenant);
        }

        public ShippingAgentQuery(ShippingAgentRepository repository)
        {
            this.repository = repository;
        }

        public ShippingAgentPM GetSinglePM(string id,int tenant)
        {
            ShippingAgentPM agent = (from a in repository.context.ShippingAgents.Include("Card")
                                     where a.Id == id && a.Tenant == tenant
                                     select new ShippingAgentPM()
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
                                         ForwarderAccountNumber = a.ForwarderAccountNumber,
                                         ForwarderCreditNumber = a.ForwarderCreditNumber,
                                         Website = a.Card.Website,
                                         InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                         ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                         VatTypeId = a.Card.VatTypeId,
                                         AccountNumber = a.Card.AccountNumber,
                                         Swift = a.Card.Swift,
                                         IBANNumber = a.Card.IBANNumber,
                                         BankName = a.Card.BankName,
                                         BankAddress = a.Card.BankAddress,
                                         LocalCustomsCode = a.LocalCustomsCode,
                                         PrimaryContactId = a.Card.PrimaryContactId,
                                         EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                         IRSNumber = a.Card.IRSNumber,
                                         IRSPlace = a.Card.IRSPlace,
                                         ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                         PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                         ExternalId2 = a.Card.ExternalId2,
                                         MetodoPagoCode = a.Card.MetodoPagoCode,
                                         UsoCFDICode = a.Card.UsoCFDICode,
                                         SATForeignRFC = a.Card.SATForeignRFC,
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
            agent.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(agent.Id, agent.Tenant);


            if (agent != null)
            {
                agent.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        agent.IsExternal = true;
                    }
                }
            }

            ShippingAgentPM securedPm = new ShippingAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "ShippingAgent", tenant);

            return securedPm;
        }

        public ShippingAgentPM GetSinglePMByCode(string code, int tenant)
        {
            ShippingAgentPM agent = (from a in repository.context.ShippingAgents.Include("Card")
                                     where a.Card.Code == code && a.Tenant == tenant
                                     select new ShippingAgentPM()
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
                                         ForwarderAccountNumber = a.ForwarderAccountNumber,
                                         ForwarderCreditNumber = a.ForwarderCreditNumber,
                                         Website = a.Card.Website,
                                         InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                         ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                         VatTypeId = a.Card.VatTypeId,
                                         AccountNumber = a.Card.AccountNumber,
                                         Swift = a.Card.Swift,
                                         IBANNumber = a.Card.IBANNumber,
                                         BankName = a.Card.BankName,
                                         BankAddress = a.Card.BankAddress,
                                         LocalCustomsCode = a.LocalCustomsCode,
                                         PrimaryContactId = a.Card.PrimaryContactId,
                                         EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                         IRSNumber = a.Card.IRSNumber,
                                         IRSPlace = a.Card.IRSPlace,
                                         ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                         PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                         ExternalId2 = a.Card.ExternalId2,
                                         MetodoPagoCode = a.Card.MetodoPagoCode,
                                         UsoCFDICode = a.Card.UsoCFDICode,
                                         SATForeignRFC = a.Card.SATForeignRFC,
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
            agent.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(agent.Id, agent.Tenant);


            if (agent != null)
            {
                agent.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        agent.IsExternal = true;
                    }
                }
            }

            ShippingAgentPM securedPm = new ShippingAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "ShippingAgent", tenant);

            return securedPm;
        }
        public IQueryable<ShippingAgentPM> GetShippingAgentPMsByTenant(int tenant)
        {
            IQueryable<ShippingAgentPM> shippingAgents = from a in repository.context.ShippingAgents.Include("Card")
                                                         where a.Tenant == tenant
                                                         select new ShippingAgentPM()
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
                                                             ForwarderAccountNumber = a.ForwarderAccountNumber,
                                                             ForwarderCreditNumber = a.ForwarderCreditNumber,
                                                             Website = a.Card.Website,
                                                             InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                             ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                                             VatTypeId = a.Card.VatTypeId,
                                                             AccountNumber = a.Card.AccountNumber,
                                                             Swift = a.Card.Swift,
                                                             IBANNumber = a.Card.IBANNumber,
                                                             BankName = a.Card.BankName,
                                                             BankAddress = a.Card.BankAddress,
                                                             LocalCustomsCode = a.LocalCustomsCode,
                                                             PrimaryContactId = a.Card.PrimaryContactId,
                                                             EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                             IRSNumber = a.Card.IRSNumber,
                                                             IRSPlace = a.Card.IRSPlace,
                                                             ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                             PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                             ExternalId2 = a.Card.ExternalId2,
                                                             MetodoPagoCode = a.Card.MetodoPagoCode,
                                                             UsoCFDICode = a.Card.UsoCFDICode,
                                                             SATForeignRFC = a.Card.SATForeignRFC,
                                                             Card = new CardPM()
                                                             {
                                                                 Id = a.Id,
                                                                 Tenant = a.Tenant,
                                                                 EnglishName = a.Card.EnglishName,
                                                                 PrimaryContactId = a.Card.PrimaryContactId,
                                                             },
                                                         };
            return shippingAgents;
        }

        public IQueryable<ShippingAgentPM> GetShippingAgentsByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.ShippingAgents.Include("Card")
                         where a.Tenant == tenant
                         select new ShippingAgentPM()
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
                             ForwarderAccountNumber = a.ForwarderAccountNumber,
                             ForwarderCreditNumber = a.ForwarderCreditNumber,
                             Website = a.Card.Website,
                             VatNumber = a.Card.VatNumber,
                             InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                             ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                             VatTypeId = a.Card.VatTypeId,
                             AccountNumber = a.Card.AccountNumber,
                             Swift = a.Card.Swift,
                             IBANNumber = a.Card.IBANNumber,
                             BankName = a.Card.BankName,
                             BankAddress = a.Card.BankAddress,
                             LocalCustomsCode = a.LocalCustomsCode,
                             PrimaryContactId = a.Card.PrimaryContactId,
                             EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                             IRSNumber = a.Card.IRSNumber,
                             IRSPlace = a.Card.IRSPlace,
                             ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                             PaymentMethodCode = a.Card.SATPaymentMethodCode,
                             ExternalId2 = a.Card.ExternalId2,
                             MetodoPagoCode = a.Card.MetodoPagoCode,
                             UsoCFDICode = a.Card.UsoCFDICode,
                             SATForeignRFC = a.Card.SATForeignRFC,
                             Card = new CardPM()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 EnglishName = a.Card.EnglishName,
                                 PrimaryContactId = a.Card.PrimaryContactId,
                             },
                         }).AsQueryable();

            IQueryable<ShippingAgentPM> query2 = null;
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

        public IQueryable<ShippingAgentList> GetIQueryableEntityList(IQueryable<ShippingAgent> iQueryable)
        {
            IQueryable<ShippingAgentList> result = (from a in iQueryable.Include("Card").Include("Card.PaymentTerm").Include("Card.VatType")
                                                    select new ShippingAgentList()
                                                    {
                                                        Code = a.Card.Code,
                                                        EnglishName = a.Card.EnglishName,
                                                        LocalName = a.Card.LocalName,
                                                        ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                        PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                        InActive = a.Card.InActive,
                                                        Remark = a.Card.Notes,
                                                        PaymentTermEnglishName = a.Card.PaymentTerm != null ? a.Card.PaymentTerm.EnglishName : "",
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        VatNumber = a.Card.VatNumber,
                                                        PaymentTermId = a.Card.PaymentTermId,
                                                        SearchFields = a.Card.SearchFields,
                                                        Website = a.Card.Website,
                                                        ForwarderAccountNumber = a.ForwarderAccountNumber,
                                                        ForwarderCreditNumber = a.ForwarderCreditNumber,
                                                        Notes = a.Card.Notes,
                                                        InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                        VatTypeId = a.Card.VatTypeId,
                                                        LocalCustomsCode = a.LocalCustomsCode,
                                                        EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                        CityName = a.Card.CityName,
                                                        CountryId = a.Card.CountryId,
                                                        CountryCode = a.Card.CountryCode,
                                                        CountryName = a.Card.CountryName,
                                                        ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                        PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                        ExternalId2 = a.Card.ExternalId2,
                                                        MetodoPagoCode = a.Card.MetodoPagoCode,
                                                        UsoCFDICode = a.Card.UsoCFDICode,
                                                        SATForeignRFC = a.Card.SATForeignRFC,
                                                        PrimaryContactName = a.PrimaryContactName,
                                                        PrimaryContactEmail = a.PrimaryContactEmail,
                                                        PrimaryContactPhone = a.PrimaryContactPhone,
                                                        StateName = a.Card.StateName,
                                                        GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                                    });


            return result;
        }
      
        public ShippingAgentList GetSingleShippingAgentList(string id, int tenant)
        {
            ShippingAgentList shippingAgentList = (from a in repository.context.ShippingAgents.Include("Card").Include("Card.PaymentTerm").Include("Card.VatType")
                                                   where a.Tenant == tenant && a.Id == id
                                                   select new ShippingAgentList()
                                                   {
                                                       Code = a.Card.Code,
                                                       EnglishName = a.Card.EnglishName,
                                                       LocalName = a.Card.LocalName,
                                                       ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                       PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                       InActive = a.Card.InActive,
                                                       Remark = a.Card.Notes,
                                                       PaymentTermEnglishName = a.Card.PaymentTerm.EnglishName,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       VatNumber = a.Card.VatNumber,
                                                       PaymentTermId = a.Card.PaymentTermId,
                                                       SearchFields = a.Card.SearchFields,
                                                       Website = a.Card.Website,
                                                       ForwarderAccountNumber = a.ForwarderAccountNumber,
                                                       ForwarderCreditNumber = a.ForwarderCreditNumber,
                                                       Notes = a.Card.Notes,
                                                       InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                       VatTypeId = a.Card.VatTypeId,
                                                       LocalCustomsCode = a.LocalCustomsCode,
                                                       EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                       CityName = a.Card.CityName,
                                                       CountryId = a.Card.CountryId,
                                                       CountryCode = a.Card.CountryCode,
                                                       CountryName = a.Card.CountryName,
                                                       ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                       PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                       ExternalId2 = a.Card.ExternalId2,
                                                       MetodoPagoCode = a.Card.MetodoPagoCode,
                                                       UsoCFDICode = a.Card.UsoCFDICode,
                                                       SATForeignRFC = a.Card.SATForeignRFC,
                                                       PrimaryContactName = a.PrimaryContactName,
                                                       PrimaryContactEmail = a.PrimaryContactEmail,
                                                       PrimaryContactPhone = a.PrimaryContactPhone,
                                                   }).FirstOrDefault();

            return shippingAgentList;
        }
    }
}
