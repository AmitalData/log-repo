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
using Logitude.BL.CommonDataModel.ExternalService;
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.InfrastructureModel.EntityQueries;

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
                                 EORInumber  = a.Card.EORInumber,
                                 ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                 ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
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
                                 BankName = a.Card.BankName,
                                 BankAddress = a.Card.BankAddress,
                                 IBANNumber = a.Card.IBANNumber,
                                 Swift = a.Card.Swift,
                                 AccountNumber = a.Card.AccountNumber,
                                 SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                                 CargoTrackingInvitationStatusName = a.Card.CargoTrackingInvitationStatus != null ? a.Card.CargoTrackingInvitationStatus.Name : null,
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
                                 GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                 RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                 SATReceptorName = a.Card.SATCustomerName,
                                 Card = new CardPM()
                                 {
                                     Id = a.Id,
                                     Tenant = a.Tenant,
                                     EnglishName = a.Card.EnglishName,
                                     PrimaryContactId = a.Card.PrimaryContactId,
                                     GLAccountDisplayNumber = a.Card.GLAccountDisplayNumber,
                                     SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                     CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                     ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                     ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                                     PartnerTypeId = a.Card.PartnerTypeId,
                                     Code = a.Card.Code,
                                 },
                                 BillToId = a.Card.BillToId,
                                 ImageDetailId = a.Card.ImageDetailId,
                             }).FirstOrDefault();

            if(agent != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(agent.Tenant);
                agent.Card = partnerARinvoiceDocumentTypeService.Set(agent.Card);
            }
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
            if (securedPm != null && agent != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Agent", Tenant = tenant, Type = "PM", Entities = new List<AgentPM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }

        public AgentPM GetSinglePMByCode(string code, int tenant)
        {
            AgentPM agent = (from a in repository.context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus ")
                             where a.Card.Code == code && a.Tenant == tenant
                             select new AgentPM()
                             {
                                 EORInumber = a.Card.EORInumber,
                                 ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                 ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
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
                                 BankName = a.Card.BankName,
                                 BankAddress = a.Card.BankAddress,
                                 IBANNumber = a.Card.IBANNumber,
                                 Swift = a.Card.Swift,
                                 AccountNumber = a.Card.AccountNumber,
                                 SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                                 CargoTrackingInvitationStatusName = a.Card.CargoTrackingInvitationStatus != null ? a.Card.CargoTrackingInvitationStatus.Name : null,
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
                                 GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                 RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                 SATReceptorName = a.Card.SATCustomerName,
                                 Card = new CardPM()
                                 {
                                     Id = a.Id,
                                     Tenant = a.Tenant,
                                     EnglishName = a.Card.EnglishName,
                                     PrimaryContactId = a.Card.PrimaryContactId,
                                     GLAccountDisplayNumber = a.Card.GLAccountDisplayNumber,
                                     SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                     CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                     ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                     ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                                     PartnerTypeId = a.Card.PartnerTypeId,
                                     Code = a.Card.Code,
                                 },
                                 BillToId = a.Card.BillToId,
                                 ImageDetailId = a.Card.ImageDetailId,
                             }).FirstOrDefault();

            if (agent != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(agent.Tenant);
                agent.Card = partnerARinvoiceDocumentTypeService.Set(agent.Card);
            }
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

            if (securedPm != null && agent != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Agent", Tenant = tenant, Type = "PM", Entities = new List<AgentPM> { securedPm }.Cast<object>().ToList() }).Set();
            }

            return securedPm;
        }
        public IQueryable<AgentPM> GetAgentPMsByTenant(int tenant)
        {
            IQueryable<AgentPM> agents = from a in repository.context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus")
                                         where a.Tenant == tenant
                                         select new AgentPM()
                                         {
                                             EORInumber = a.Card.EORInumber,
                                             ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                             ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
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
                                             BankName = a.Card.BankName,
                                             BankAddress = a.Card.BankAddress,
                                             IBANNumber = a.Card.IBANNumber,
                                             Swift = a.Card.Swift,
                                             AccountNumber = a.Card.AccountNumber,
                                             SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                                             CargoTrackingInvitationStatusName = a.Card.CargoTrackingInvitationStatus != null ? a.Card.CargoTrackingInvitationStatus.Name : null,
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
                                             RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                             SATReceptorName = a.Card.SATCustomerName,
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
                                             ImageDetailId = a.Card.ImageDetailId
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
                             EORInumber = a.Card.EORInumber,
                             ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                             ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
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
                             BankName = a.Card.BankName,
                             BankAddress = a.Card.BankAddress,
                             IBANNumber = a.Card.IBANNumber,
                             Swift = a.Card.Swift,
                             AccountNumber = a.Card.AccountNumber,
                             SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                             CargoTrackingInvitationStatusName = a.Card.CargoTrackingInvitationStatus != null ? a.Card.CargoTrackingInvitationStatus.Name : null,
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
                             RegimenFiscalCode = a.Card.RegimenFiscalCode,
                             SATReceptorName = a.Card.SATCustomerName,
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
                             ImageDetailId = a.Card.ImageDetailId,

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
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<AgentList> result = (from a in iQueryable.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.InvoiceCurrency").Include("Card.PaymentTerm")
                                            join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                            from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
                                            select  new AgentList()
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
                                               CargoTrackingInvitationStatusName = a.Card.CargoTrackingInvitationStatus != null ? a.Card.CargoTrackingInvitationStatus.Name : null,
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
                                               GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                               RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                               SATReceptorName = a.Card.SATCustomerName,
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
                                               Field18 = customFieldsMainObject != null ? customFieldsMainObject.Field18 : null,
                                               Field19 = customFieldsMainObject != null ? customFieldsMainObject.Field19 : null,
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
                                           });


            return result;
        }
  
    }
}
