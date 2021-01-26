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
    public class CustomAgentQuery
    {
        CustomAgentRepository repository;

        public CustomAgentQuery()
        {
            repository = new CustomAgentRepository(); 
        }

        public CustomAgentQuery(int tenant)
        {
            repository = new CustomAgentRepository(tenant);
        }

        public CustomAgentQuery(CustomAgentRepository repository)
        {
            this.repository = repository;
        }

        public CustomAgentPM GetSinglePM(string id)
        {
            CustomAgentPM agent = (from a in repository.context.CustomAgents.Include("Card")
                                   where a.Id == id
                                   select new CustomAgentPM()
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
            agent.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(agent.Id, agent.Tenant);

            if (agent != null)
            {
                agent.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(agent.Tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        agent.IsExternal = true;
                    }
                }
            }
            CustomAgentPM securedPm = new CustomAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "CustomAgent", agent.Tenant);

            return securedPm;
        }

        public CustomAgentPM GetSinglePM(int tenant, string id)
        {
            CustomAgentPM agent = (from a in repository.context.CustomAgents.Include("Card")
                                   where a.Id == id && a.Tenant == tenant
                                   select new CustomAgentPM()
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
                                       BillToId = a.Card.BillToId,
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
            CustomAgentPM securedPm = new CustomAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "CustomAgent", tenant);

            return securedPm;
        }

        public CustomAgentPM GetSinglePM(string id,int tenant)
        {
            CustomAgentPM agent = (from a in repository.context.CustomAgents.Include("Card")
                                   where a.Id == id && a.Tenant == tenant
                                   select new CustomAgentPM()
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
                                           PartnerTypeId = a.Card.PartnerTypeId,
                                           Code = a.Card.Code,
                                           GLAccountDisplayNumber = a.Card.GLAccountDisplayNumber,
                                       },
                                       BillToId = a.Card.BillToId,
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
            CustomAgentPM securedPm = new CustomAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "CustomAgent", tenant);

            return securedPm;
        }

        public IQueryable<CustomAgentPM> GetCustomAgentPMsByTenant(int tenant)
        {
            IQueryable<CustomAgentPM> customAgents = from a in repository.context.CustomAgents.Include("Card")
                                                     where a.Tenant == tenant
                                                     select new CustomAgentPM()
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
                                                         UsoCFDICode = a.Card.UsoCFDICode,
                                                         SATForeignRFC = a.Card.SATForeignRFC,
                                                         Card = new CardPM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             EnglishName = a.Card.EnglishName,
                                                             PrimaryContactId = a.Card.PrimaryContactId,
                                                         },
                                                         BillToId = a.Card.BillToId,
                                                     };
            return customAgents;
        }

        public IQueryable<CustomAgentPM> GetCustomAgentsByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.CustomAgents.Include("Card")
                         where a.Tenant == tenant
                         select new CustomAgentPM()
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
                             UsoCFDICode = a.Card.UsoCFDICode,
                             SATForeignRFC = a.Card.SATForeignRFC,
                             Card = new CardPM()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 EnglishName = a.Card.EnglishName,
                                 PrimaryContactId = a.Card.PrimaryContactId,
                             },
                             BillToId = a.Card.BillToId,
                         }).AsQueryable();

            IQueryable<CustomAgentPM> query2 = null;
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

        public IQueryable<CustomAgentList> GetIQueryableEntityList(IQueryable<CustomAgent> iQueryable)
        {
            //int Tenant = iQueryable.Select(s => s.Tenant).FirstOrDefault();
            IQueryable<CustomAgentList> result = (from a in iQueryable.Include("Card")
                                                  select new CustomAgentList()
                                                  {
                                                      Code = a.Card.Code,
                                                      EnglishName = a.Card.EnglishName,
                                                      LocalName = a.Card.LocalName,
                                                      ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                      PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                      InActive = a.Card.InActive,
                                                      Notes = a.Card.Notes,
                                                      PaymentTermId = a.Card.PaymentTermId,
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      VatNumber = a.Card.VatNumber,
                                                      SearchFields = a.Card.SearchFields,
                                                      Website = a.Card.Website,
                                                      InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                      VatTypeId = a.Card.VatTypeId,
                                                      EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                      CityName = a.Card.CityName,
                                                      CountryId = a.Card.CountryId,
                                                      CountryCode = a.Card.CountryCode,
                                                      CountryName = a.Card.CountryName,
                                                      PaymentTermEnglishName = "",
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
     
    }
}
