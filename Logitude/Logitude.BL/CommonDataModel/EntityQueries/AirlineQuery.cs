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
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                               Card = new CardPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   EnglishName = a.Card.EnglishName,
                                   PrimaryContactId = a.Card.PrimaryContactId,
                               },
                           }).FirstOrDefault();


            if (airline != null)
            {
                AirlinePM securedPm = new AirlinePM();
                SecuredMapping.GetMappedPM(airline, securedPm, "Airline", tenant);
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
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                               Card = new CardPM()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   EnglishName = a.Card.EnglishName,
                                   PrimaryContactId = a.Card.PrimaryContactId,
                               },
                           }).FirstOrDefault();


            if (airline != null)
            {
                AirlinePM securedPm = new AirlinePM();
                SecuredMapping.GetMappedPM(airline, securedPm, "Airline", tenant);
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
                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                                                 ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                                 PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                             ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                             PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
            IQueryable<AirlineList> result = from a in iQueryable.Include("Card").Include("Card.PaymentTerm")
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
                                             };
            return result;
        }
    }
}
