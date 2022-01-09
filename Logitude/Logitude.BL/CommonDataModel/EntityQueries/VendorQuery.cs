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
    public class VendorQuery
    {
        VendorRepository repository;

        public VendorQuery()
        {
            repository = new VendorRepository(); 
        }

        public VendorQuery(int tenant)
        {
            repository = new VendorRepository(tenant);
        }

        public VendorQuery(VendorRepository repository)
        {
            this.repository = repository;
        }

        public VendorPM GetSinglePM(string id, int tenant)
        {
            VendorPM vendor = (from a in repository.context.Vendors.Include("Card")
                               where a.Id == id && a.Tenant == tenant
                               select new VendorPM()
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
                                   BillToId = a.Card.BillToId,
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
                                   GLAccountId = a.Card.GLAccountId,
                                   CreatedByPartner = a.Card.CreatedByPartner,
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
                               }).FirstOrDefault();

            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            vendor.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(vendor.Id, vendor.Tenant);

            if (vendor != null)
            {
                vendor.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        vendor.IsExternal = true;
                    }
                }
            }

            VendorPM securedPm = new VendorPM();
            SecuredMapping.GetMappedPM(vendor, securedPm, "Vendor", tenant);

            if (securedPm != null && vendor != null)
            {
                Vendor entityPoco = (from s in repository.context.Vendors where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }
            return securedPm;
        }

        private void MapCustomFields(VendorPM vendorPM, Vendor vendor)
        {
            vendorPM.Field1 = new CustomFieldClass("Field1", "Vendor", vendor.Field1);
            vendorPM.Field2 = new CustomFieldClass("Field2", "Vendor", vendor.Field2);
            vendorPM.Field3 = new CustomFieldClass("Field3", "Vendor", vendor.Field3);
            vendorPM.Field4 = new CustomFieldClass("Field4", "Vendor", vendor.Field4);
            vendorPM.Field5 = new CustomFieldClass("Field5", "Vendor", vendor.Field5);
            vendorPM.Field6 = new CustomFieldClass("Field6", "Vendor", vendor.Field6);
            vendorPM.Field7 = new CustomFieldClass("Field7", "Vendor", vendor.Field7);
            vendorPM.Field8 = new CustomFieldClass("Field8", "Vendor", vendor.Field8);
            vendorPM.Field9 = new CustomFieldClass("Field9", "Vendor", vendor.Field9);
            vendorPM.Field10 = new CustomFieldClass("Field10", "Vendor", vendor.Field10);
        }

        public VendorPM GetSingleVendorPM(string id, int tenant)
        {
            VendorPM vendor = (from a in repository.context.Vendors.Include("Card")
                               where a.Id == id && a.Tenant == tenant
                               select new VendorPM()
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
                                   BillToId = a.Card.BillToId,
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
                                   CreatedByPartner = a.Card.CreatedByPartner,

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
            vendor.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(vendor.Id, vendor.Tenant);

            if (vendor != null)
            {
                vendor.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        vendor.IsExternal = true;
                    }
                }
            }

            VendorPM securedPm = new VendorPM();
            SecuredMapping.GetMappedPM(vendor, securedPm, "Vendor", tenant);

            if (securedPm != null && vendor != null)
            {
                Vendor entityPoco = (from s in repository.context.Vendors where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }

            return securedPm;
        }

        public VendorPM GetSingleVendorPM(int tenant, string id)
        {
            VendorPM vendor = (from a in repository.context.Vendors.Include("Card")
                               where a.Id == id && a.Tenant == tenant
                               select new VendorPM()
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
                                   BillToId = a.Card.BillToId,
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
                                   CreatedByPartner = a.Card.CreatedByPartner,
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
            vendor.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(vendor.Id, vendor.Tenant);
           
            if (vendor != null)
            {
                vendor.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        vendor.IsExternal = true;
                    }
                }
            }

            VendorPM securedPm = new VendorPM();
            SecuredMapping.GetMappedPM(vendor, securedPm, "Vendor", tenant);

            if (securedPm != null && vendor != null)
            {
                Vendor entityPoco = (from s in repository.context.Vendors where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }
            return securedPm;
        }

        public IQueryable<VendorPM> GetVendorPMsByTenant(int tenant)
        {
            IQueryable<VendorPM> vendors = from a in repository.context.Vendors.Include("Card")
                                           where a.Tenant == tenant
                                           select new VendorPM()
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
                                               BillToId = a.Card.BillToId,
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
                                               CreatedByPartner = a.Card.CreatedByPartner,
                                               Card = new CardPM()
                                               {
                                                   Id = a.Id,
                                                   Tenant = a.Tenant,
                                                   EnglishName = a.Card.EnglishName,
                                                   PrimaryContactId = a.Card.PrimaryContactId,
                                               },
                                           };
            return vendors;
        }

        public IQueryable<VendorPM> GetVendorsByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Vendors.Include("Card")
                         where a.Tenant == tenant
                         select new VendorPM()
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
                             BillToId = a.Card.BillToId,
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
                             CreatedByPartner = a.Card.CreatedByPartner,
                             Card = new CardPM()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 EnglishName = a.Card.EnglishName,
                                 PrimaryContactId = a.Card.PrimaryContactId,
                             },
                         }).AsQueryable();

            IQueryable<VendorPM> query2 = null;
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

        public IQueryable<VendorList> GetIQueryableEntityList(IQueryable<Vendor> iQueryable)
        {
            IQueryable<VendorList> result = (from a in iQueryable.Include("Card")
                                             select new VendorList()
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
                                                 EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                                                 CityName = a.Card.CityName,
                                                 CountryId = a.Card.CountryId,
                                                 CountryCode = a.Card.CountryCode,
                                                 CountryName = a.Card.CountryName,
                                                 ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                 PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                 ExternalId2 = a.Card.ExternalId2,
                                                 SATForeignRFC = a.Card.SATForeignRFC,
                                                 MetodoPagoCode = a.Card.MetodoPagoCode,
                                                 UsoCFDICode = a.Card.UsoCFDICode,
                                                 PrimaryContactName = a.PrimaryContactName,
                                                 PrimaryContactEmail = a.PrimaryContactEmail,
                                                 PrimaryContactPhone = a.PrimaryContactPhone,
                                                 CreatedByPartner = a.Card.CreatedByPartner,
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
     
        public VendorPM GetSingleVendorPMByCode(string code, int tenant)
        {
            VendorPM vendor = (from a in repository.context.Vendors.Include("Card")
                               where a.Card.Code == code && a.Tenant == tenant
                               select new VendorPM()
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
                                   BillToId = a.Card.BillToId,
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
                                   CreatedByPartner = a.Card.CreatedByPartner,
                                   Card = new CardPM()
                                   {
                                       Id = a.Id,
                                       Tenant = a.Tenant,
                                       EnglishName = a.Card.EnglishName,
                                       PrimaryContactId = a.Card.PrimaryContactId,
                                       PartnerTypeId = a.Card.PartnerTypeId,
                                       Code = a.Card.Code,
                                   },
                               }).FirstOrDefault();

            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);
            CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);
            vendor.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(vendor.Id, vendor.Tenant);
           
            if (vendor != null)
            {
                vendor.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        vendor.IsExternal = true;
                    }
                }
            }

            VendorPM securedPm = new VendorPM();
            SecuredMapping.GetMappedPM(vendor, securedPm, "Vendor", tenant);


            if (securedPm != null && vendor != null)
            {
                Vendor entityPoco = (from s in repository.context.Vendors where s.Id == securedPm.Id select s).FirstOrDefault();
                MapCustomFields(securedPm, entityPoco);
            }
            return securedPm;
        }
    }
}
