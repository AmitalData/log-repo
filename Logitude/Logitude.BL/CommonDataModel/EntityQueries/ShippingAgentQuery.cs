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
                                         BillToId = a.Card.BillToId,
                                         ImageDetailId = a.Card.ImageDetailId,
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
            if (securedPm != null && agent != null)
            {
                ShippingAgent entityPoco = (from s in repository.context.ShippingAgents where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }
            return securedPm;
        }

        private void MapCustomFields(ShippingAgentPM shippingAgent, ShippingAgent entityPoco)
        {
            shippingAgent.Field1 = new CustomFieldClass("Field1", "ShippingAgent", entityPoco.Field1);
            shippingAgent.Field2 = new CustomFieldClass("Field2", "ShippingAgent", entityPoco.Field2);
            shippingAgent.Field3 = new CustomFieldClass("Field3", "ShippingAgent", entityPoco.Field3);
            shippingAgent.Field4 = new CustomFieldClass("Field4", "ShippingAgent", entityPoco.Field4);
            shippingAgent.Field5 = new CustomFieldClass("Field5", "ShippingAgent", entityPoco.Field5);
            shippingAgent.Field6 = new CustomFieldClass("Field6", "ShippingAgent", entityPoco.Field6);
            shippingAgent.Field7 = new CustomFieldClass("Field7", "ShippingAgent", entityPoco.Field7);
            shippingAgent.Field8 = new CustomFieldClass("Field8", "ShippingAgent", entityPoco.Field8);
            shippingAgent.Field9 = new CustomFieldClass("Field9", "ShippingAgent", entityPoco.Field9);
            shippingAgent.Field10 = new CustomFieldClass("Field10", "ShippingAgent", entityPoco.Field10);
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
                                         BillToId = a.Card.BillToId,
                                         ImageDetailId = a.Card.ImageDetailId,
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
            if (securedPm != null && agent != null)
            {
                ShippingAgent entityPoco = (from s in repository.context.ShippingAgents where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }
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
                                                             BillToId = a.Card.BillToId,
                                                             ImageDetailId = a.Card.ImageDetailId,
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
                             BillToId = a.Card.BillToId,
                             ImageDetailId = a.Card.ImageDetailId,
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
                                                   }).FirstOrDefault();

            return shippingAgentList;
        }
    }
}
