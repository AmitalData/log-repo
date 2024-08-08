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
                                             EmailForSendingSingArinvoice = a.Card.EmailForSendingSingArinvoice,
                                             SendingInterestReport = a.Card.SendingInterestReport,
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

            ShippingAgentPM securedPm = new ShippingAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "ShippingAgent", tenant);
            if (securedPm != null && agent != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingAgent", Tenant = tenant, Type = "PM", Entities = new List<ShippingAgentPM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }

        public ShippingAgentPM GetSinglePMByCode(string code, int tenant)
        {
            ShippingAgentPM agent = (from a in repository.context.ShippingAgents.Include("Card")
                                     where a.Card.Code == code && a.Tenant == tenant
                                     select new ShippingAgentPM()
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
                                             EmailForSendingSingArinvoice = a.Card.EmailForSendingSingArinvoice,
                                             SendingInterestReport = a.Card.SendingInterestReport,
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

            ShippingAgentPM securedPm = new ShippingAgentPM();
            SecuredMapping.GetMappedPM(agent, securedPm, "ShippingAgent", tenant);
            if (securedPm != null && agent != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingAgent", Tenant = tenant, Type = "PM", Entities = new List<ShippingAgentPM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }
        public IQueryable<ShippingAgentPM> GetShippingAgentPMsByTenant(int tenant)
        {
            IQueryable<ShippingAgentPM> shippingAgents = from a in repository.context.ShippingAgents.Include("Card")
                                                         where a.Tenant == tenant
                                                         select new ShippingAgentPM()
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
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<ShippingAgentList> result = (from a in iQueryable.Include("Card").Include("Card.PaymentTerm").Include("Card.VatType")
                                                    join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                    from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
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
                                                       RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                                       SATReceptorName = a.Card.SATCustomerName,
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
            if (shippingAgentList != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingAgent", Tenant = tenant, Type = "List", Entities = new List<ShippingAgentList> { shippingAgentList }.Cast<object>().ToList() }).Set();
            }
            return shippingAgentList;
        }
    }
}
