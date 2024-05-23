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
using Logitude.BL.CommonDataModel.ExternalService;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.CustomFields;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ShippingLineQuery
    {
        ShippingLineRepository repository;
        public ShippingLineQuery()
        {
            repository = new ShippingLineRepository(); 
        }
        public ShippingLineQuery(int tenant)
        {
            repository = new ShippingLineRepository(tenant);
        }
        public ShippingLineQuery(ShippingLineRepository repository)
        {
            this.repository = repository;
        }

        public ShippingLinePM GetSinglePM(string id, int tenant)
        {
            var shippingLine = (from a in repository.context.ShippingLines.Include("Card").Include("ShippingAgent")
                                where a.Id == id && a.Tenant == tenant
                                select new ShippingLinePM()
                                {
                                    ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                    ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                                    Tenant = a.Tenant,
                                    Id = a.Id,
                                    ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                    PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                    AccountingVATSplit = a.Card.AccountingVATSplit,
                                    AddedManually = a.AddedManually,
                                    OurCreditNumber = a.OurCreditNumber,
                                    Remark = a.Card.Notes,
                                    ShippingAgentId = a.ShippingAgentId,
                                    VatNumber = a.Card.VatNumber,
                                    Code = a.Card.Code,
                                    EnglishName = a.Card.EnglishName,
                                    LocalName = a.Card.LocalName,
                                    CarrierTypeId = a.Card.PartnerTypeId,
                                    InActive = a.Card.InActive,
                                    PaymentTermId = a.Card.PaymentTermId,
                                    SCACCode = a.SCACCode,
                                    Website = a.Card.Website,
                                    Notes = a.Card.Notes,
                                    ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                    ShippingAgentName = a.ShippingAgent != null ? a.ShippingAgent.Card.EnglishName : null,
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
                                    IsINTTRARegistered = a.IsINTTRARegistered,
                                    CAAT = a.CAAT,
                                    CBSA = a.CBSA,
                                    INTTRARegistrationNotes = a.INTTRARegistrationNotes,
                                    GLAccountId = a.Card.GLAccountId,
                                    INTTRAUpdatesShipment = a.INTTRAUpdatesShipment,
                                    GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                    ImageDetailId = a.Card.ImageDetailId,
                                    IsSendingByContainer = a.IsSendingByContainer,
                                    IsSendingByBillOfLading = a.IsSendingByBillOfLading,
                                    IsSupportsContainerTracking = a.IsSupportsContainerTracking,
                                    IsAutomaticRequestsSent = a.IsAutomaticRequestsSent,
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
                                }).FirstOrDefault();

            if(shippingLine != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(shippingLine.Tenant);
                shippingLine.Card = partnerARinvoiceDocumentTypeService.Set(shippingLine.Card);
            }
            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            shippingLine.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(shippingLine.Id, shippingLine.Tenant);

            if (shippingLine != null)
            {
                shippingLine.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        shippingLine.IsExternal = true;
                    }
                }
            }

            ShippingLinePM securedPm = new ShippingLinePM();
            SecuredMapping.GetMappedPM(shippingLine, securedPm, "ShippingLine", tenant);
            if (securedPm != null && shippingLine != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingLine", Tenant = tenant, Type = "PM", Entities = new List<ShippingLinePM> { securedPm }.Cast<object>().ToList() }).Set();
            }

            return securedPm;
        }
        
        public ShippingLinePM GetSinglePMByIdAndTenant(string id, int tenant)
        {
            var shippingLine = (from a in repository.context.ShippingLines.Include("Card")
                                where a.Id == id && a.Tenant == tenant
                                select new ShippingLinePM()
                                {
                                    ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                    ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                                    Tenant = a.Tenant,
                                    Id = a.Id,
                                    ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                    PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                    AccountingVATSplit = a.Card.AccountingVATSplit,
                                    AddedManually = a.AddedManually,
                                    OurCreditNumber = a.OurCreditNumber,
                                    Remark = a.OurCreditNumber,
                                    ShippingAgentId = a.ShippingAgentId,
                                    VatNumber = a.Card.VatNumber,
                                    Code = a.Card.Code,
                                    EnglishName = a.Card.EnglishName,
                                    LocalName = a.Card.LocalName,
                                    CarrierTypeId = a.Card.PartnerTypeId,
                                    InActive = a.Card.InActive,
                                    PaymentTermId = a.Card.PaymentTermId,
                                    SCACCode = a.SCACCode,
                                    Website = a.Card.Website,
                                    Notes = a.Card.Notes,
                                    ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                    ShippingAgentName = a.ShippingAgent != null ? a.ShippingAgent.Card.EnglishName : null,
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
                                    IsINTTRARegistered = a.IsINTTRARegistered,
                                    INTTRARegistrationNotes = a.INTTRARegistrationNotes,
                                    INTTRAUpdatesShipment = a.INTTRAUpdatesShipment,
                                    CAAT = a.CAAT,
                                    CBSA = a.CBSA,
                                    ImageDetailId = a.Card.ImageDetailId,
                                    IsSendingByContainer = a.IsSendingByContainer,
                                    IsSendingByBillOfLading = a.IsSendingByBillOfLading,
                                    IsSupportsContainerTracking = a.IsSupportsContainerTracking,
                                    IsAutomaticRequestsSent = a.IsAutomaticRequestsSent,
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
                                        PartnerTypeId = a.Card.PartnerTypeId,
                                        Code = a.Card.Code,
                                        EmailForSendingSingArinvoice = a.Card.EmailForSendingSingArinvoice,
                                        SendingInterestReport = a.Card.SendingInterestReport,
                                    },
                                    BillToId = a.Card.BillToId,
                                }).FirstOrDefault();

            if(shippingLine != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(shippingLine.Tenant);
                shippingLine.Card = partnerARinvoiceDocumentTypeService.Set(shippingLine.Card);
            }

            ShippingLinePM securedPm = new ShippingLinePM();
            SecuredMapping.GetMappedPM(shippingLine, securedPm, "ShippingLine", tenant);
            if (securedPm != null && shippingLine != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingLine", Tenant = tenant, Type = "PM", Entities = new List<ShippingLinePM> { securedPm }.Cast<object>().ToList() }).Set();
            }

            return securedPm;
        }

        public bool CheckShippingLinesAddedManually(string id, int tenant)
        {
            bool addedManually = (from a in repository.context.ShippingLines
                                  where a.Tenant == tenant && a.Id == id 
                                  select a.AddedManually).FirstOrDefault();


            return addedManually;
        }

        public ShippingLinePM GetSinglePMByCode(string code, int tenant)
        {
            var shippingLine = (from a in repository.context.ShippingLines.Include("Card").Include("ShippingAgent")
                                where a.Card.Code == code && a.Tenant == tenant
                                select new ShippingLinePM()
                                {
                                    ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                    ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                                    Tenant = a.Tenant,
                                    Id = a.Id,
                                    ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                    PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                    AccountingVATSplit = a.Card.AccountingVATSplit,
                                    AddedManually = a.AddedManually,
                                    OurCreditNumber = a.OurCreditNumber,
                                    Remark = a.OurCreditNumber,
                                    ShippingAgentId = a.ShippingAgentId,
                                    VatNumber = a.Card.VatNumber,
                                    Code = a.Card.Code,
                                    EnglishName = a.Card.EnglishName,
                                    LocalName = a.Card.LocalName,
                                    CarrierTypeId = a.Card.PartnerTypeId,
                                    InActive = a.Card.InActive,
                                    PaymentTermId = a.Card.PaymentTermId,
                                    SCACCode = a.SCACCode,
                                    Website = a.Card.Website,
                                    Notes = a.Card.Notes,
                                    ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                    ShippingAgentName = a.ShippingAgent != null ? a.ShippingAgent.Card.EnglishName : null,
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
                                    IsINTTRARegistered = a.IsINTTRARegistered,
                                    INTTRARegistrationNotes = a.INTTRARegistrationNotes,
                                    INTTRAUpdatesShipment = a.INTTRAUpdatesShipment,
                                    CAAT = a.CAAT,
                                    CBSA = a.CBSA,
                                    ImageDetailId = a.Card.ImageDetailId,
                                    IsSendingByContainer = a.IsSendingByContainer,
                                    IsSendingByBillOfLading = a.IsSendingByBillOfLading,
                                    IsSupportsContainerTracking = a.IsSupportsContainerTracking,
                                    IsAutomaticRequestsSent = a.IsAutomaticRequestsSent,
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
                                        PartnerTypeId = a.Card.PartnerTypeId,
                                        Code = a.Card.Code,
                                        EmailForSendingSingArinvoice = a.Card.EmailForSendingSingArinvoice,
                                        SendingInterestReport = a.Card.SendingInterestReport,
                                    },
                                    BillToId = a.Card.BillToId,
                                }).FirstOrDefault();

            if(shippingLine != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(shippingLine.Tenant);
                shippingLine.Card = partnerARinvoiceDocumentTypeService.Set(shippingLine.Card);
            }

            ShippingLinePM securedPm = new ShippingLinePM();
            SecuredMapping.GetMappedPM(shippingLine, securedPm, "ShippingLine", tenant);
            if (securedPm != null && shippingLine != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingLine", Tenant = tenant, Type = "PM", Entities = new List<ShippingLinePM> { securedPm }.Cast<object>().ToList() }).Set();
            }

            return securedPm;
        }

        public IQueryable<ShippingLinePM> GetShippinngLinePMsByTenant(int tenant)
        {
            IQueryable<ShippingLinePM> shippingLines = from a in repository.context.ShippingLines.Include("Card").Include("ShippingAgent")
                                                       where a.Tenant == tenant
                                                       select new ShippingLinePM()
                                                       {
                                                           ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                                           ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                                                           Tenant = a.Tenant,
                                                           Id = a.Id,
                                                           ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                           PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                           AccountingVATSplit = a.Card.AccountingVATSplit,
                                                           AddedManually = a.AddedManually,
                                                           OurCreditNumber = a.OurCreditNumber,
                                                           Remark = a.OurCreditNumber,
                                                           ShippingAgentId = a.ShippingAgentId,
                                                           VatNumber = a.Card.VatNumber,
                                                           Code = a.Card.Code,
                                                           EnglishName = a.Card.EnglishName,
                                                           LocalName = a.Card.LocalName,
                                                           CarrierTypeId = a.Card.PartnerTypeId,
                                                           InActive = a.Card.InActive,
                                                           PaymentTermId = a.Card.PaymentTermId,
                                                           SCACCode = a.SCACCode,
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
                                                           IsINTTRARegistered = a.IsINTTRARegistered,
                                                           INTTRARegistrationNotes = a.INTTRARegistrationNotes,
                                                           INTTRAUpdatesShipment = a.INTTRAUpdatesShipment,
                                                           CAAT = a.CAAT,
                                                           CBSA = a.CBSA,
                                                           ImageDetailId = a.Card.ImageDetailId,
                                                           BillToId = a.Card.BillToId,
                                                           IsSendingByContainer = a.IsSendingByContainer,
                                                           IsSendingByBillOfLading = a.IsSendingByBillOfLading,
                                                           IsSupportsContainerTracking = a.IsSupportsContainerTracking,
                                                           IsAutomaticRequestsSent = a.IsAutomaticRequestsSent,
                                                           RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                                           SATReceptorName = a.Card.SATCustomerName,
                                                       };
            return shippingLines;
        }

        public IQueryable<ShippingLinePM> GetShippingLinesByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.ShippingLines.Include("Card").Include("ShippingAgent")
                         where a.Tenant == tenant
                         select new ShippingLinePM()
                         {
                             ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                             ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                             Tenant = a.Tenant,
                             Id = a.Id,
                             ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                             PayablesAccountingCard = a.Card.PayablesAccountingCard,
                             AccountingVATSplit = a.Card.AccountingVATSplit,
                             AddedManually = a.AddedManually,
                             OurCreditNumber = a.OurCreditNumber,
                             Remark = a.OurCreditNumber,
                             ShippingAgentId = a.ShippingAgentId,
                             VatNumber = a.Card.VatNumber,
                             Code = a.Card.Code,
                             EnglishName = a.Card.EnglishName,
                             LocalName = a.Card.LocalName,
                             CarrierTypeId = a.Card.PartnerTypeId,
                             InActive = a.Card.InActive,
                             PaymentTermId = a.Card.PaymentTermId,
                             SCACCode = a.SCACCode,
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
                             IsINTTRARegistered = a.IsINTTRARegistered,
                             INTTRARegistrationNotes = a.INTTRARegistrationNotes,
                             INTTRAUpdatesShipment = a.INTTRAUpdatesShipment,
                             CAAT = a.CAAT,
                             CBSA = a.CBSA,
                             ImageDetailId = a.Card.ImageDetailId,
                             BillToId = a.Card.BillToId,
                             IsSendingByContainer = a.IsSendingByContainer,
                             IsSendingByBillOfLading = a.IsSendingByBillOfLading,
                             IsSupportsContainerTracking = a.IsSupportsContainerTracking,
                             IsAutomaticRequestsSent = a.IsAutomaticRequestsSent,
                             RegimenFiscalCode = a.Card.RegimenFiscalCode,
                             SATReceptorName = a.Card.SATCustomerName,
                         }).AsQueryable();

            IQueryable<ShippingLinePM> query2 = null;
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

        public IQueryable<ShippingLineList> GetIQueryableEntityList(IQueryable<ShippingLine> iQueryable)
        {
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<ShippingLineList> result = (from a in iQueryable.Include("Card").Include("ShippingAgent").Include("Card.PaymentTerm")
                                                   join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                   from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
                                                   select   new ShippingLineList()
                                                   {
                                                       Code = a.Card.Code,
                                                       EnglishName = a.Card.EnglishName,
                                                       LocalName = a.Card.LocalName,
                                                       ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                       PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                       InActive = a.Card.InActive,
                                                       Remark = a.Card.Notes,
                                                       SCACCode = a.SCACCode,
                                                       PaymentTermId = a.Card.PaymentTermId,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       VatNumber = a.Card.VatNumber,
                                                       AddedManually = a.AddedManually,
                                                       OurCreditNumber = a.OurCreditNumber,
                                                       SearchFields = a.Card.SearchFields,
                                                       Website = a.Card.Website,
                                                       Notes = a.Card.Notes,
                                                       InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                       VatTypeId = a.Card.VatTypeId,
                                                       EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                       CityName = a.Card.CityName,
                                                       CountryId = a.Card.CountryId,
                                                       CountryCode = a.Card.CountryCode,
                                                       CountryName = a.Card.CountryName,
                                                       ShippingAgentEnglishName = a.ShippingAgent != null ? (a.ShippingAgent.Card != null ? a.ShippingAgent.Card.EnglishName : "") : "",
                                                       ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                       PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                       ExternalId2 = a.Card.ExternalId2,
                                                       SATForeignRFC = a.Card.SATForeignRFC,
                                                       MetodoPagoCode = a.Card.MetodoPagoCode,
                                                       UsoCFDICode = a.Card.UsoCFDICode,
                                                       IsINTTRARegistered = a.IsINTTRARegistered,
                                                       INTTRARegistrationNotes = a.INTTRARegistrationNotes,
                                                       INTTRAUpdatesShipment = a.INTTRAUpdatesShipment,
                                                       PrimaryContactName = a.PrimaryContactName,
                                                       PrimaryContactEmail = a.PrimaryContactEmail,
                                                       PrimaryContactPhone = a.PrimaryContactPhone,
                                                       CAAT = a.CAAT,
                                                       CBSA = a.CBSA,
                                                       StateName = a.Card.StateName,
                                                       GLAccountNumber = a.Card.GLAccountDisplayNumber,
                                                       IsSendingByContainer = a.IsSendingByContainer,
                                                       IsSendingByBillOfLading = a.IsSendingByBillOfLading,
                                                       IsSupportsContainerTracking = a.IsSupportsContainerTracking,
                                                       IsAutomaticRequestsSent = a.IsAutomaticRequestsSent,
                                                       RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                                       SATReceptorName = a.Card.SATCustomerName,
                                                       PaymentTermEnglishName = a.Card.PaymentTerm != null ? a.Card.PaymentTerm.EnglishName : null,
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
