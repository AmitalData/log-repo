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
                                    Card = new CardPM()
                                    {
                                        Id = a.Id,
                                        Tenant = a.Tenant,
                                        EnglishName = a.Card.EnglishName,
                                        PrimaryContactId = a.Card.PrimaryContactId,
                                    },
                                }).FirstOrDefault();

            return shippingLine;
        }

        public IQueryable<ShippingLinePM> GetShippinngLinePMsByTenant(int tenant)
        {
            IQueryable<ShippingLinePM> shippingLines = from a in repository.context.ShippingLines.Include("Card").Include("ShippingAgent")
                                                       where a.Tenant == tenant
                                                       select new ShippingLinePM()
                                                       {
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
            IQueryable<ShippingLineList> result = (from a in iQueryable.Include("Card").Include("ShippingAgent")
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
                                                       PaymentTermEnglishName = "",
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
                                                       GLAccountNumber = a.Card.GLAccountDisplayNumber
                                                   });


            return result;
        }
 
    }
}
