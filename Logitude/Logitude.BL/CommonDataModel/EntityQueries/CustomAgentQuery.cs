using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
    public class CustomAgentQuery
    {
        CustomAgentRepository repository;



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
                                       RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                       SATReceptorName = a.Card.SATCustomerName,
                                       Card = new CardPM()
                                       {
                                           Id = a.Id,
                                           Tenant = a.Tenant,
                                           EnglishName = a.Card.EnglishName,
                                           PrimaryContactId = a.Card.PrimaryContactId,
                                           SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                           CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                           ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                           ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                                           EmailForSendingSingArinvoice = a.Card.EmailForSendingSingArinvoice,
                                           SendingInterestReport = a.Card.SendingInterestReport,
                                           PartnerTypeId = a.Card.PartnerTypeId,
                                           Code = a.Card.Code,
                                           GLAccountId=a.Card.GLAccountId,
                                           ExternalSystem = a.Card.ExternalSystem,
                                           IsAutonomy = a.Card.IsAutonomy

                                       },
                                       BillToId = a.Card.BillToId,
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
            if (securedPm != null && agent != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "CustomAgent", Tenant = agent.Tenant, Type = "PM", Entities = new List<CustomAgentPM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }

        public CustomAgentPM GetSinglePM(int tenant, string id)
        {
            CustomAgentPM agent = (from a in repository.context.CustomAgents.Include("Card")
                                   where a.Id == id && a.Tenant == tenant
                                   select new CustomAgentPM()
                                   {
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
                                       RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                       SATReceptorName = a.Card.SATCustomerName,
                                       Card = new CardPM()
                                       {
                                           Id = a.Id,
                                           Tenant = a.Tenant,
                                           EnglishName = a.Card.EnglishName,
                                           PrimaryContactId = a.Card.PrimaryContactId,
                                           SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                           CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                           ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                           ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                                           EmailForSendingSingArinvoice = a.Card.EmailForSendingSingArinvoice,
                                           SendingInterestReport = a.Card.SendingInterestReport,
                                           PartnerTypeId = a.Card.PartnerTypeId,
                                           Code = a.Card.Code,
                                           GLAccountId = a.Card.GLAccountId,
                                           ExternalSystem = a.Card.ExternalSystem,
                                           IsAutonomy = a.Card.IsAutonomy

                                       },
                                       BillToId = a.Card.BillToId,
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
            CustomAgentPM securedPm = new CustomAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "CustomAgent", tenant);
            if (securedPm != null && agent != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "CustomAgent", Tenant = tenant, Type = "PM", Entities = new List<CustomAgentPM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }

        public CustomAgentPM GetSinglePM(string id,int tenant)
        {
            CustomAgentPM agent = (from a in repository.context.CustomAgents.Include("Card")
                                   where a.Id == id && a.Tenant == tenant
                                   select new CustomAgentPM()
                                   {
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
                                           GLAccountDisplayNumber = a.Card.GLAccountDisplayNumber,
                                           SingleInvoiceTemplateId = a.Card.SingleInvoiceTemplateId,
                                           CustomsInvoiceTemplateId = a.Card.CustomsInvoiceTemplateId,
                                           ConsolidationInvoiceTemplateId = a.Card.ConsolidationInvoiceTemplateId,
                                           ManifestInvoiceTemplateId = a.Card.ManifestInvoiceTemplateId,
                                           EmailForSendingSingArinvoice = a.Card.EmailForSendingSingArinvoice,
                                           SendingInterestReport = a.Card.SendingInterestReport,
                                           GLAccountId= a.Card.GLAccountId,
                                           ExternalSystem = a.Card.ExternalSystem,
                                           IsAutonomy = a.Card.IsAutonomy

                                       },
                                       BillToId = a.Card.BillToId,
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
            CustomAgentPM securedPm = new CustomAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "CustomAgent", tenant);
            if (securedPm != null && agent != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "CustomAgent", Tenant = tenant, Type = "PM", Entities = new List<CustomAgentPM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }

        public IQueryable<CustomAgentPM> GetCustomAgentPMsByTenant(int tenant)
        {
            IQueryable<CustomAgentPM> customAgents = from a in repository.context.CustomAgents.Include("Card")
                                                     where a.Tenant == tenant
                                                     select new CustomAgentPM()
                                                     {
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
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<CustomAgentList> result = (from a in iQueryable.Include("Card")
                                                  join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                  from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
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
                                                      PaymentTermEnglishName = a.Card.PaymentTerm != null ? a.Card.PaymentTerm.EnglishName : null,
                                                  });


            return result;
        }
     
    }
}
