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
    public class AgentQuery
    {
        AgentRepository repository;

        public AgentQuery()
        {
            repository = new AgentRepository();
        }

        public AgentQuery(int tenant)
        {
            repository = new AgentRepository(tenant);
        }

        public AgentQuery(AgentRepository agentRepository)
        {
            repository = agentRepository;
        }

        public AgentPM GetSinglePM(string id, int tenant)
        {
            AgentPM agent = (from a in repository.context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus ")
                             where a.Id == id && a.Tenant == tenant
                             select new AgentPM()
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
                                 BankName = a.Card.BankName,
                                 BankAddress = a.Card.BankAddress,
                                 IBANNumber = a.Card.IBANNumber,
                                 Swift = a.Card.Swift,
                                 AccountNumber = a.Card.AccountNumber,
                                 SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                                 LastLoginDate = a.Card.LastLoginDate,
                                 PrimaryContactId = a.Card.PrimaryContactId,
                                 EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                 CASSCode = a.CASSCode,
                                 IATACode = a.IATACode,
                                 RegulatedAgentCode = a.RegulatedAgentCode,
                                 IRSNumber = a.Card.IRSNumber,
                                 IRSPlace = a.Card.IRSPlace,
                                 AgentSharedLogisticsKey = a.AgentSharedLogisticsKey,
                                 ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                 PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                 IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                 BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                 BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                 ExternalId2 = a.Card.ExternalId2,
                                 SATForeignRFC = a.Card.SATForeignRFC,
                                 MetodoPagoCode = a.Card.MetodoPagoCode,
                                 UsoCFDICode = a.Card.UsoCFDICode,
                                 GLAccountId = a.Card.GLAccountId,
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

            AgentPM securedPm = new AgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "Agent", tenant);

            return securedPm;
        }

        public IQueryable<AgentPM> GetAgentPMsByTenant(int tenant)
        {
            IQueryable<AgentPM> agents = from a in repository.context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus")
                                         where a.Tenant == tenant
                                         select new AgentPM()
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
                                             BankName = a.Card.BankName,
                                             BankAddress = a.Card.BankAddress,
                                             IBANNumber = a.Card.IBANNumber,
                                             Swift = a.Card.Swift,
                                             AccountNumber = a.Card.AccountNumber,
                                             SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                                             LastLoginDate = a.Card.LastLoginDate,
                                             PrimaryContactId = a.Card.PrimaryContactId,
                                             EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                             CASSCode = a.CASSCode,
                                             IATACode = a.IATACode,
                                             RegulatedAgentCode = a.RegulatedAgentCode,
                                             IRSNumber = a.Card.IRSNumber,
                                             IRSPlace = a.Card.IRSPlace,
                                             AgentSharedLogisticsKey = a.AgentSharedLogisticsKey,
                                             ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                             PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                             IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                             BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                             BlockNewShipmentCreation = a.BlockNewShipmentCreation,
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
            return agents;
        }

        public IQueryable<AgentPM> GetAgentsByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus")
                         where a.Tenant == tenant
                         select new AgentPM()
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
                             BankName = a.Card.BankName,
                             BankAddress = a.Card.BankAddress,
                             IBANNumber = a.Card.IBANNumber,
                             Swift = a.Card.Swift,
                             AccountNumber = a.Card.AccountNumber,
                             SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                             LastLoginDate = a.Card.LastLoginDate,
                             PrimaryContactId = a.Card.PrimaryContactId,
                             EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                             CASSCode = a.CASSCode,
                             IATACode = a.IATACode,
                             RegulatedAgentCode = a.RegulatedAgentCode,
                             IRSNumber = a.Card.IRSNumber,
                             IRSPlace = a.Card.IRSPlace,
                             AgentSharedLogisticsKey = a.AgentSharedLogisticsKey,
                             ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                             PaymentMethodCode = a.Card.SATPaymentMethodCode,
                             IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                             BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                             BlockNewShipmentCreation = a.BlockNewShipmentCreation,
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

            IQueryable<AgentPM> query2 = null;
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

        public IQueryable<AgentList> GetIQueryableEntityList(IQueryable<Agent> iQueryable)
        {
            IQueryable<AgentList> result = from a in iQueryable.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.InvoiceCurrency")
                                           select new AgentList()
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
                                               SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                                               LastLoginDate = a.Card.LastLoginDate,
                                               EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                               CityName = a.Card.CityName,
                                               CountryId = a.Card.CountryId,
                                               CountryCode = a.Card.CountryCode,
                                               CountryName = a.Card.CountryName,
                                               CASSCode = a.CASSCode,
                                               IATACode = a.IATACode,
                                               RegulatedAgentCode = a.RegulatedAgentCode,
                                               InvoiceCurrencyCode = a.Card.InvoiceCurrency == null ? null : a.Card.InvoiceCurrency.Code,
                                               ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                               PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                               IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                               BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                               BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                               ExternalId2 = a.Card.ExternalId2,
                                               SATForeignRFC = a.Card.SATForeignRFC,
                                               MetodoPagoCode = a.Card.MetodoPagoCode,
                                               UsoCFDICode = a.Card.UsoCFDICode,
                                               Address1 = a.Card.Address1,
                                               Address2 = a.Card.Address2,
                                               Phone = a.Card.Phone,
                                               ZipCode = a.Card.ZipCode,
                                               PrimaryContactName = a.PrimaryContactName,
                                               PrimaryContactEmail = a.PrimaryContactEmail,
                                               PrimaryContactPhone = a.PrimaryContactPhone,
                                               StateName = a.Card.StateName,
                                           };
            return result;
        }
    }
}
