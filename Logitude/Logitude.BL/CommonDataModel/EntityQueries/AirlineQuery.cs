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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.ExternalService;
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AirlineQuery
    {
        AirlineRepository repository;

        public AirlineQuery()
        {
            repository = new AirlineRepository();
        }

        public AirlineQuery(int tenant)
        {
            repository = new AirlineRepository(tenant);
        }

        public AirlineQuery(AirlineRepository airlineRepositoryRepository)
        {
            repository = airlineRepositoryRepository;
        }
        
        public AirlinePM GetSinglePMByCode(string code, int tenant)
        {
            var airline = (from a in repository.context.Airlines.Include("Card")
                           where a.Tenant == tenant && a.Card.Code == code
                           select new AirlinePM()
                           {
                               ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                               ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
                               AccountingVATSplit = a.Card.AccountingVATSplit,
                               AddedManually = a.AddedManually,
                               AWBAccount = a.AWBAccount,
                               Id = a.Id,
                               Prefix = a.Prefix,
                               Remark = a.Card.Notes,
                               Tenant = a.Tenant,
                               VatNumber = a.Card.VatNumber,
                               Code = a.Card.Code,
                               EnglishName = a.Card.EnglishName,
                               LocalName = a.Card.LocalName,
                               CarrierTypeId = a.Card.PartnerTypeId,
                               InActive = a.Card.InActive,
                               PaymentTermId = a.Card.PaymentTermId,
                               Website = a.Card.Website,
                               ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                               SearchFields = a.Card.SearchFields,
                               Notes = a.Card.Notes,
                               InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                               VatTypeId = a.Card.VatTypeId,
                               CheckDigit = a.CheckDigit,
                               LimitedLength = a.LimitedLength,
                               BankAccountNumber = a.Card.AccountNumber,
                               Swift = a.Card.Swift,
                               IBANNumber = a.Card.IBANNumber,
                               BankName = a.Card.BankName,
                               BankAddress = a.Card.BankAddress,
                               TTY = a.TTY,
                               IsChampRegistered = a.IsChampRegistered,
                               ChampNeedsRegistration = a.ChampNeedsRegistration,
                               AccountNumber = a.AccountNumber,
                               PrimaryContactId = a.Card.PrimaryContactId,
                               EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                               GLSHKPIMA = a.GLSHKPIMA,
                               IsGLSHKRegistered = a.IsGLSHKRegistered,
                               GLSHKNeedsRegistration = a.GLSHKNeedsRegistration,
                               ChampFWB = a.ChampFWB,
                               ChampFHL = a.ChampFHL,
                               ChampFSU = a.ChampFSU,
                               ChampFSRFSA = a.ChampFSRFSA,
                               ChampFVRFVA = a.ChampFVRFVA,
                               ChampFFRFFA = a.ChampFFRFFA,
                               GLSHKFWB = a.GLSHKFWB,
                               GLSHKFHL = a.GLSHKFHL,
                               GLSHKFSU = a.GLSHKFSU,
                               GLSHKFSRFSA = a.GLSHKFSRFSA,
                               GLSHKFVRFVA = a.GLSHKFVRFVA,
                               GLSHKFFRFFA = a.GLSHKFFRFFA,
                               IsAllowedInAirlinesRestriction = a.IsAllowedInAirlinesRestriction,
                               RegistrationNotes = a.RegistrationNotes,
                               ChampRegistrationRequested = a.ChampRegistrationRequested,
                               GLSHKRegistrationRequested = a.GLSHKRegistrationRequested,
                               HasAdaptations = a.HasAdaptations,
                               ICAO = a.ICAO,
                               RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                               IsManagingProduct = a.IsManagingProduct,
                               IsProductMandatory = a.IsProductMandatory,
                               IsDescriptionOfGoodsFromList = a.IsDescriptionOfGoodsFromList,
                               ScheduleDays = a.ScheduleDays,
                               NoAvailabilityInFVAMessages = a.NoAvailabilityInFVAMessages,
                               IRSNumber = a.Card.IRSNumber,
                               IRSPlace = a.Card.IRSPlace,
                               IsDeclined = a.IsDeclined,
                               DeclineNotes = a.DeclineNotes,
                               ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                               PaymentMethodCode = a.Card.SATPaymentMethodCode,
                               ExternalId2 = a.Card.ExternalId2,
                               SATForeignRFC = a.Card.ExternalId2,
                               MetodoPagoCode = a.Card.MetodoPagoCode,
                               UsoCFDICode = a.Card.UsoCFDICode,
                               ImageDetailId = a.Card.ImageDetailId,
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
                                   PartnerTypeId = a.Card.PartnerTypeId,
                                   Code = a.Card.Code,
                               },
                               BillToId = a.Card.BillToId,
                           }).FirstOrDefault();

            if (airline != null)
            {
            PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(airline.Tenant);
            airline.Card = partnerARinvoiceDocumentTypeService.Set(airline.Card);
            }

            if (airline != null)
            {
                AirlinePM securedPm = new AirlinePM();
                SecuredMapping.GetMappedPM(airline, securedPm, "Airline", tenant);
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Airline", Tenant = tenant, Type = "PM", Entities = new List<AirlinePM> { securedPm }.Cast<object>().ToList() }).Set();
                return securedPm;
            }

            else
            {
                return airline;
            }
        }

        public AirlinePM GetSinglePMByICAO(string ICAO, int tenant)
        {
            var airline = (from a in repository.context.Airlines.Include("Card")
                           where a.Tenant == tenant && a.ICAO == ICAO
                           select new AirlinePM()
                           {
                               ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                               ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
                               AccountingVATSplit = a.Card.AccountingVATSplit,
                               AddedManually = a.AddedManually,
                               AWBAccount = a.AWBAccount,
                               Id = a.Id,
                               Prefix = a.Prefix,
                               Remark = a.Card.Notes,
                               Tenant = a.Tenant,
                               VatNumber = a.Card.VatNumber,
                               Code = a.Card.Code,
                               EnglishName = a.Card.EnglishName,
                               LocalName = a.Card.LocalName,
                               CarrierTypeId = a.Card.PartnerTypeId,
                               InActive = a.Card.InActive,
                               PaymentTermId = a.Card.PaymentTermId,
                               Website = a.Card.Website,
                               ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                               SearchFields = a.Card.SearchFields,
                               Notes = a.Card.Notes,
                               InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                               VatTypeId = a.Card.VatTypeId,
                               CheckDigit = a.CheckDigit,
                               LimitedLength = a.LimitedLength,
                               BankAccountNumber = a.Card.AccountNumber,
                               Swift = a.Card.Swift,
                               IBANNumber = a.Card.IBANNumber,
                               BankName = a.Card.BankName,
                               BankAddress = a.Card.BankAddress,
                               TTY = a.TTY,
                               IsChampRegistered = a.IsChampRegistered,
                               ChampNeedsRegistration = a.ChampNeedsRegistration,
                               AccountNumber = a.AccountNumber,
                               PrimaryContactId = a.Card.PrimaryContactId,
                               EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                               GLSHKPIMA = a.GLSHKPIMA,
                               IsGLSHKRegistered = a.IsGLSHKRegistered,
                               GLSHKNeedsRegistration = a.GLSHKNeedsRegistration,
                               ChampFWB = a.ChampFWB,
                               ChampFHL = a.ChampFHL,
                               ChampFSU = a.ChampFSU,
                               ChampFSRFSA = a.ChampFSRFSA,
                               ChampFVRFVA = a.ChampFVRFVA,
                               ChampFFRFFA = a.ChampFFRFFA,
                               GLSHKFWB = a.GLSHKFWB,
                               GLSHKFHL = a.GLSHKFHL,
                               GLSHKFSU = a.GLSHKFSU,
                               GLSHKFSRFSA = a.GLSHKFSRFSA,
                               GLSHKFVRFVA = a.GLSHKFVRFVA,
                               GLSHKFFRFFA = a.GLSHKFFRFFA,
                               IsAllowedInAirlinesRestriction = a.IsAllowedInAirlinesRestriction,
                               RegistrationNotes = a.RegistrationNotes,
                               ChampRegistrationRequested = a.ChampRegistrationRequested,
                               GLSHKRegistrationRequested = a.GLSHKRegistrationRequested,
                               HasAdaptations = a.HasAdaptations,
                               ICAO = a.ICAO,
                               RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                               IsManagingProduct = a.IsManagingProduct,
                               IsProductMandatory = a.IsProductMandatory,
                               IsDescriptionOfGoodsFromList = a.IsDescriptionOfGoodsFromList,
                               ScheduleDays = a.ScheduleDays,
                               NoAvailabilityInFVAMessages = a.NoAvailabilityInFVAMessages,
                               IRSNumber = a.Card.IRSNumber,
                               IRSPlace = a.Card.IRSPlace,
                               IsDeclined = a.IsDeclined,
                               DeclineNotes = a.DeclineNotes,
                               ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                               PaymentMethodCode = a.Card.SATPaymentMethodCode,
                               ExternalId2 = a.Card.ExternalId2,
                               SATForeignRFC = a.Card.ExternalId2,
                               MetodoPagoCode = a.Card.MetodoPagoCode,
                               UsoCFDICode = a.Card.UsoCFDICode,
                               ImageDetailId = a.Card.ImageDetailId,
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
                               },
                               BillToId = a.Card.BillToId,
                           }).FirstOrDefault();


            if (airline != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(airline.Tenant);
                airline.Card = partnerARinvoiceDocumentTypeService.Set(airline.Card);
            }
            if (airline != null)
            {
                AirlinePM securedPm = new AirlinePM();
                SecuredMapping.GetMappedPM(airline, securedPm, "Airline", tenant);
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Airline", Tenant = tenant, Type = "PM", Entities = new List<AirlinePM> { securedPm }.Cast<object>().ToList() }).Set();
                return securedPm;
            }
            else
            {
                return airline;
            }
        }

        public AirlinePM GetSinglePM(string id, int tenant)
        {
            var airline = (from a in repository.context.Airlines.Include("Card")
                           where a.Tenant == tenant && a.Id == id
                           select new AirlinePM()
                           {
                               ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                               ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
                               AccountingVATSplit = a.Card.AccountingVATSplit,
                               AddedManually = a.AddedManually,
                               AWBAccount = a.AWBAccount,
                               Id = a.Id,
                               Prefix = a.Prefix,
                               Remark = a.Card.Notes,
                               Tenant = a.Tenant,
                               VatNumber = a.Card.VatNumber,
                               Code = a.Card.Code,
                               EnglishName = a.Card.EnglishName,
                               LocalName = a.Card.LocalName,
                               CarrierTypeId = a.Card.PartnerTypeId,
                               InActive = a.Card.InActive,
                               PaymentTermId = a.Card.PaymentTermId,
                               Website = a.Card.Website,
                               ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                               SearchFields = a.Card.SearchFields,
                               Notes = a.Card.Notes,
                               InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                               VatTypeId = a.Card.VatTypeId,
                               CheckDigit = a.CheckDigit,
                               LimitedLength = a.LimitedLength,
                               BankAccountNumber = a.Card.AccountNumber,
                               Swift = a.Card.Swift,
                               IBANNumber = a.Card.IBANNumber,
                               BankName = a.Card.BankName,
                               BankAddress = a.Card.BankAddress,
                               TTY = a.TTY,
                               IsChampRegistered = a.IsChampRegistered,
                               ChampNeedsRegistration = a.ChampNeedsRegistration,
                               AccountNumber = a.AccountNumber,
                               PrimaryContactId = a.Card.PrimaryContactId,
                               EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                               GLSHKPIMA = a.GLSHKPIMA,
                               IsGLSHKRegistered = a.IsGLSHKRegistered,
                               GLSHKNeedsRegistration = a.GLSHKNeedsRegistration,
                               ChampFWB = a.ChampFWB,
                               ChampFHL = a.ChampFHL,
                               ChampFSU = a.ChampFSU,
                               ChampFSRFSA = a.ChampFSRFSA,
                               ChampFVRFVA = a.ChampFVRFVA,
                               ChampFFRFFA = a.ChampFFRFFA,
                               GLSHKFWB = a.GLSHKFWB,
                               GLSHKFHL = a.GLSHKFHL,
                               GLSHKFSU = a.GLSHKFSU,
                               GLSHKFSRFSA = a.GLSHKFSRFSA,
                               GLSHKFVRFVA = a.GLSHKFVRFVA,
                               GLSHKFFRFFA = a.GLSHKFFRFFA,
                               IsAllowedInAirlinesRestriction = a.IsAllowedInAirlinesRestriction,
                               RegistrationNotes = a.RegistrationNotes,
                               ChampRegistrationRequested = a.ChampRegistrationRequested,
                               GLSHKRegistrationRequested = a.GLSHKRegistrationRequested,
                               HasAdaptations = a.HasAdaptations,
                               ICAO = a.ICAO,
                               RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                               IsManagingProduct = a.IsManagingProduct,
                               IsProductMandatory = a.IsProductMandatory,
                               IsDescriptionOfGoodsFromList = a.IsDescriptionOfGoodsFromList,
                               ScheduleDays = a.ScheduleDays,
                               NoAvailabilityInFVAMessages = a.NoAvailabilityInFVAMessages,
                               IRSNumber = a.Card.IRSNumber,
                               IRSPlace = a.Card.IRSPlace,
                               IsDeclined = a.IsDeclined,
                               DeclineNotes = a.DeclineNotes,
                               ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                               PaymentMethodCode = a.Card.SATPaymentMethodCode,
                               ExternalId2 = a.Card.ExternalId2,
                               SATForeignRFC = a.Card.ExternalId2,
                               MetodoPagoCode = a.Card.MetodoPagoCode,
                               UsoCFDICode = a.Card.UsoCFDICode,
                               GLAccountId = a.Card.GLAccountId,
                               ImageDetailId = a.Card.ImageDetailId,
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
                                   Prefix = a.Prefix,
                               },
                               BillToId = a.Card.BillToId,
                           }).FirstOrDefault();

            if (airline != null)
            {
                PartnerARinvoiceDocumentTypeService partnerARinvoiceDocumentTypeService = new PartnerARinvoiceDocumentTypeService(airline.Tenant);
                airline.Card = partnerARinvoiceDocumentTypeService.Set(airline.Card);
            }
            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            airline.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(airline.Id, airline.Tenant);


            if (airline != null)
            {
                airline.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        airline.IsExternal = true;
                    }
                }
            }

            AirlinePM securedPm = new AirlinePM();
            SecuredMapping.GetMappedPM(airline, securedPm, "Airline", tenant);
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Airline", Tenant = tenant, Type = "PM", Entities = new List<AirlinePM> { securedPm }.Cast<object>().ToList() }).Set();

            return securedPm;
        }

        public bool CheckAirlinesAddedManually(string id, int tenant)
        {
            bool addedManually = (from a in repository.context.Airlines
                                  where a.Tenant == tenant && a.Id == id
                                  select a.AddedManually).FirstOrDefault();
            
            return addedManually;
        }

        public IQueryable<AirlinePM> GetAirlinePMsByTenant(int tenant)
        {
            IQueryable<AirlinePM> airlines = from a in repository.context.Airlines.Include("Card")
                                             where a.Tenant == tenant
                                             select new AirlinePM()
                                             {
                                                 ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                                                 ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                                                 ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                 PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                 AccountingVATSplit = a.Card.AccountingVATSplit,
                                                 AddedManually = a.AddedManually,
                                                 AWBAccount = a.AWBAccount,
                                                 Id = a.Id,
                                                 Prefix = a.Prefix,
                                                 Remark = a.Card.Notes,
                                                 Tenant = a.Tenant,
                                                 VatNumber = a.Card.VatNumber,
                                                 Code = a.Card.Code,
                                                 EnglishName = a.Card.EnglishName,
                                                 LocalName = a.Card.LocalName,
                                                 CarrierTypeId = a.Card.PartnerTypeId,
                                                 InActive = a.Card.InActive,
                                                 PaymentTermId = a.Card.PaymentTermId,
                                                 Website = a.Card.Website,
                                                 ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                                 SearchFields = a.Card.SearchFields,
                                                 Notes = a.Card.Notes,
                                                 InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                 VatTypeId = a.Card.VatTypeId,
                                                 CheckDigit = a.CheckDigit,
                                                 LimitedLength = a.LimitedLength,
                                                 BankAccountNumber = a.Card.AccountNumber,
                                                 Swift = a.Card.Swift,
                                                 IBANNumber = a.Card.IBANNumber,
                                                 BankName = a.Card.BankName,
                                                 BankAddress = a.Card.BankAddress,
                                                 TTY = a.TTY,
                                                 IsChampRegistered = a.IsChampRegistered,
                                                 ChampNeedsRegistration = a.ChampNeedsRegistration,
                                                 AccountNumber = a.AccountNumber,
                                                 EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                 GLSHKPIMA = a.GLSHKPIMA,
                                                 IsGLSHKRegistered = a.IsGLSHKRegistered,
                                                 GLSHKNeedsRegistration = a.GLSHKNeedsRegistration,
                                                 ChampFWB = a.ChampFWB,
                                                 ChampFHL = a.ChampFHL,
                                                 ChampFSU = a.ChampFSU,
                                                 ChampFSRFSA = a.ChampFSRFSA,
                                                 ChampFVRFVA = a.ChampFVRFVA,
                                                 ChampFFRFFA = a.ChampFFRFFA,
                                                 GLSHKFWB = a.GLSHKFWB,
                                                 GLSHKFHL = a.GLSHKFHL,
                                                 GLSHKFSU = a.GLSHKFSU,
                                                 GLSHKFSRFSA = a.GLSHKFSRFSA,
                                                 GLSHKFVRFVA = a.GLSHKFVRFVA,
                                                 GLSHKFFRFFA = a.GLSHKFFRFFA,
                                                 IsAllowedInAirlinesRestriction = a.IsAllowedInAirlinesRestriction,
                                                 RegistrationNotes = a.RegistrationNotes,
                                                 ChampRegistrationRequested = a.ChampRegistrationRequested,
                                                 GLSHKRegistrationRequested = a.GLSHKRegistrationRequested,
                                                 HasAdaptations = a.HasAdaptations,
                                                 ICAO = a.ICAO,
                                                 RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                                                 IsManagingProduct = a.IsManagingProduct,
                                                 IsProductMandatory = a.IsProductMandatory,
                                                 IsDescriptionOfGoodsFromList = a.IsDescriptionOfGoodsFromList,
                                                 ScheduleDays = a.ScheduleDays,
                                                 NoAvailabilityInFVAMessages = a.NoAvailabilityInFVAMessages,
                                                 IRSNumber = a.Card.IRSNumber,
                                                 IRSPlace = a.Card.IRSPlace,
                                                 IsDeclined = a.IsDeclined,
                                                 DeclineNotes = a.DeclineNotes,
                                                 ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                 PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                 ExternalId2 = a.Card.ExternalId2,
                                                 SATForeignRFC = a.Card.ExternalId2,
                                                 MetodoPagoCode = a.Card.MetodoPagoCode,
                                                 UsoCFDICode = a.Card.UsoCFDICode,
                                                 ImageDetailId = a.Card.ImageDetailId,
                                                 BillToId = a.Card.BillToId,
                                                 RegimenFiscalCode = a.Card.RegimenFiscalCode,
                                                 SATReceptorName = a.Card.SATCustomerName,
                                             };
            return airlines;
        }

        public IQueryable<AirlinePM> GetAirlineByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Airlines.Include("Card")
                         where a.Tenant == tenant
                         select new AirlinePM()
                         {
                             ExportLocalCustomerGroupId = a.Card.ExportLocalCustomerGroupId,
                             ImportLocalCustomerGroupId = a.Card.ImportLocalCustomerGroupId,
                             ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                             PayablesAccountingCard = a.Card.PayablesAccountingCard,
                             AccountingVATSplit = a.Card.AccountingVATSplit,
                             AddedManually = a.AddedManually,
                             AWBAccount = a.AWBAccount,
                             Id = a.Id,
                             Prefix = a.Prefix,
                             Remark = a.Card.Notes,
                             Tenant = a.Tenant,
                             VatNumber = a.Card.VatNumber,
                             Code = a.Card.Code,
                             EnglishName = a.Card.EnglishName,
                             LocalName = a.Card.LocalName,
                             CarrierTypeId = a.Card.PartnerTypeId,
                             InActive = a.Card.InActive,
                             PaymentTermId = a.Card.PaymentTermId,
                             Website = a.Card.Website,
                             ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                             SearchFields = a.Card.SearchFields,
                             Notes = a.Card.Notes,
                             InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                             VatTypeId = a.Card.VatTypeId,
                             CheckDigit = a.CheckDigit,
                             LimitedLength = a.LimitedLength,
                             BankAccountNumber = a.Card.AccountNumber,
                             Swift = a.Card.Swift,
                             IBANNumber = a.Card.IBANNumber,
                             BankName = a.Card.BankName,
                             BankAddress = a.Card.BankAddress,
                             TTY = a.TTY,
                             IsChampRegistered = a.IsChampRegistered,
                             ChampNeedsRegistration = a.ChampNeedsRegistration,
                             AccountNumber = a.AccountNumber,
                             EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                             GLSHKPIMA = a.GLSHKPIMA,
                             IsGLSHKRegistered = a.IsGLSHKRegistered,
                             GLSHKNeedsRegistration = a.GLSHKNeedsRegistration,
                             ChampFWB = a.ChampFWB,
                             ChampFHL = a.ChampFHL,
                             ChampFSU = a.ChampFSU,
                             ChampFSRFSA = a.ChampFSRFSA,
                             ChampFVRFVA = a.ChampFVRFVA,
                             ChampFFRFFA = a.ChampFFRFFA,
                             GLSHKFWB = a.GLSHKFWB,
                             GLSHKFHL = a.GLSHKFHL,
                             GLSHKFSU = a.GLSHKFSU,
                             GLSHKFSRFSA = a.GLSHKFSRFSA,
                             GLSHKFVRFVA = a.GLSHKFVRFVA,
                             GLSHKFFRFFA = a.GLSHKFFRFFA,
                             IsAllowedInAirlinesRestriction = a.IsAllowedInAirlinesRestriction,
                             RegistrationNotes = a.RegistrationNotes,
                             ChampRegistrationRequested = a.ChampRegistrationRequested,
                             GLSHKRegistrationRequested = a.GLSHKRegistrationRequested,
                             HasAdaptations = a.HasAdaptations,
                             ICAO = a.ICAO,
                             RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                             IsManagingProduct = a.IsManagingProduct,
                             IsProductMandatory = a.IsProductMandatory,
                             IsDescriptionOfGoodsFromList = a.IsDescriptionOfGoodsFromList,
                             ScheduleDays = a.ScheduleDays,
                             NoAvailabilityInFVAMessages = a.NoAvailabilityInFVAMessages,
                             IRSNumber = a.Card.IRSNumber,
                             IRSPlace = a.Card.IRSPlace,
                             IsDeclined = a.IsDeclined,
                             DeclineNotes = a.DeclineNotes,
                             ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                             PaymentMethodCode = a.Card.SATPaymentMethodCode,
                             ExternalId2 = a.Card.ExternalId2,
                             SATForeignRFC = a.Card.ExternalId2,
                             MetodoPagoCode = a.Card.MetodoPagoCode,
                             UsoCFDICode = a.Card.UsoCFDICode,
                             ImageDetailId = a.Card.ImageDetailId,
                             BillToId = a.Card.BillToId,
                             RegimenFiscalCode = a.Card.RegimenFiscalCode,
                             SATReceptorName = a.Card.SATCustomerName,
                         }).AsQueryable();

            IQueryable<AirlinePM> query2 = null;
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
            {
                return query;
            }
        }

        public IQueryable<AirlineList> GetIQueryableEntityList(IQueryable<Airline> iQueryable)
        {
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<AirlineList> result = (from a in iQueryable.Include("Card").Include("Card.PaymentTerm")
                                              join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                              from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
                                              select new AirlineList()
                                              {
                                                  Code = a.Card.Code,
                                                  EnglishName = a.Card.EnglishName,
                                                  LocalName = a.Card.LocalName,
                                                  Prefix = a.Prefix,
                                                  AWBAccount = a.AWBAccount,
                                                  AddedManually = a.AddedManually,
                                                  InActive = a.Card.InActive,
                                                  Remark = a.Card.Notes,
                                                  PaymentTermId = a.Card.PaymentTermId,
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  VatNumber = a.Card.VatNumber,
                                                  Website = a.Card.Website,
                                                  SearchFields = a.Card.SearchFields,
                                                  ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                  PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                                  Notes = a.Card.Notes,
                                                  InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                                  VatTypeId = a.Card.VatTypeId,
                                                  CheckDigit = a.CheckDigit,
                                                  LimitedLength = a.LimitedLength,
                                                  PaymentTermEnglishName = a.Card.PaymentTerm != null ? a.Card.PaymentTerm.EnglishName : null,
                                                  TTY = a.TTY,
                                                  IsChampRegistered = a.IsChampRegistered,
                                                  ChampNeedsRegistration = a.ChampNeedsRegistration,
                                                  AccountNumber = a.AccountNumber,
                                                  EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                  CityName = a.Card.CityName,
                                                  CountryId = a.Card.CountryId,
                                                  CountryCode = a.Card.CountryCode,
                                                  CountryName = a.Card.CountryName,
                                                  GLSHKPIMA = a.GLSHKPIMA,
                                                  IsGLSHKRegistered = a.IsGLSHKRegistered,
                                                  GLSHKNeedsRegistration = a.GLSHKNeedsRegistration,
                                                  ChampFWB = a.ChampFWB,
                                                  ChampFHL = a.ChampFHL,
                                                  ChampFSU = a.ChampFSU,
                                                  ChampFSRFSA = a.ChampFSRFSA,
                                                  ChampFVRFVA = a.ChampFVRFVA,
                                                  ChampFFRFFA = a.ChampFFRFFA,
                                                  GLSHKFWB = a.GLSHKFWB,
                                                  GLSHKFHL = a.GLSHKFHL,
                                                  GLSHKFSU = a.GLSHKFSU,
                                                  GLSHKFSRFSA = a.GLSHKFSRFSA,
                                                  GLSHKFVRFVA = a.GLSHKFVRFVA,
                                                  GLSHKFFRFFA = a.GLSHKFFRFFA,
                                                  IsAllowedInAirlinesRestriction = a.IsAllowedInAirlinesRestriction,
                                                  RegistrationNotes = a.RegistrationNotes,
                                                  ChampRegistrationRequested = a.ChampRegistrationRequested,
                                                  GLSHKRegistrationRequested = a.GLSHKRegistrationRequested,
                                                  HasAdaptations = a.HasAdaptations,
                                                  ICAO = a.ICAO,
                                                  RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                                                  IsManagingProduct = a.IsManagingProduct,
                                                  IsProductMandatory = a.IsProductMandatory,
                                                  IsDescriptionOfGoodsFromList = a.IsDescriptionOfGoodsFromList,
                                                  ScheduleDays = a.ScheduleDays,
                                                  NoAvailabilityInFVAMessages = a.NoAvailabilityInFVAMessages,
                                                  IsDeclined = a.IsDeclined,
                                                  DeclineNotes = a.DeclineNotes,
                                                  ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                  PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                  ExternalId2 = a.Card.ExternalId2,
                                                  SATForeignRFC = a.Card.ExternalId2,
                                                  MetodoPagoCode = a.Card.MetodoPagoCode,
                                                  UsoCFDICode = a.Card.UsoCFDICode,
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
