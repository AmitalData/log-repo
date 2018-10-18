using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityLists;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class BankAccountLiteQuery
    {
        BankAccountLiteRepository repository;
        public BankAccountLiteQuery()
        {
            repository = new BankAccountLiteRepository();
        }

        public BankAccountLiteQuery(int tenant)
        {
            repository = new BankAccountLiteRepository(tenant);
        }

        public BankAccountLiteQuery(BankAccountLiteRepository BankAccountLiteRepository)
        {
            repository = BankAccountLiteRepository;
        }

        public IQueryable<BankAccountLitePM> GetBankAccountLitePMs(int tenant)
        {
            return from a in repository.context.BankAccountLites
                   where a.Tenant == tenant
                   select new BankAccountLitePM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       CreateDate = a.CreateDate,
                       UpdateDate = a.UpdateDate,
                       LocalName = a.LocalName,
                       EnglishName = a.EnglishName,
                       BranchNumber = a.BranchNumber,
                       AccountNumber = a.AccountNumber,
                       IBAN = a.IBAN,
                       SwiftCode = a.SwiftCode,
                       BankCode = a.BankCode,
                       CurrencyId = a.CurrencyId,
                       CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                       BranchAddress = a.BranchAddress,
                       Inactive = a.Inactive,
                       SearchFields = a.SearchFields,
                       VatNumber = a.VatNumber,
                   };
        }

        public BankAccountLitePM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.BankAccountLites
                    where a.Id == id && a.Tenant == tenant
                    select new BankAccountLitePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        LocalName = a.LocalName,
                        EnglishName = a.EnglishName,
                        BranchNumber = a.BranchNumber,
                        AccountNumber = a.AccountNumber,
                        IBAN = a.IBAN,
                        SwiftCode = a.SwiftCode,
                        BankCode = a.BankCode,
                        CurrencyId = a.CurrencyId,
                        CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                        BranchAddress = a.BranchAddress,
                        Inactive = a.Inactive,
                        SearchFields = a.SearchFields,
                        VatNumber = a.VatNumber,

                    }).FirstOrDefault();
        }

        public IQueryable<BankAccountLitePM> GetBankAccountLitePMsbyTenant(int tenant)
        {
            return (from a in repository.context.BankAccountLites
                    where a.Tenant == tenant
                    select new BankAccountLitePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        LocalName = a.LocalName,
                        EnglishName = a.EnglishName,
                        BranchNumber = a.BranchNumber,
                        AccountNumber = a.AccountNumber,
                        IBAN = a.IBAN,
                        SwiftCode = a.SwiftCode,
                        BankCode = a.BankCode,
                        CurrencyId = a.CurrencyId,
                        CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                        BranchAddress = a.BranchAddress,
                        Inactive = a.Inactive,
                        SearchFields = a.SearchFields,
                        VatNumber = a.VatNumber,
                    });
        }

        public IQueryable<BankAccountLiteList> GetIQueryableEntityList(IQueryable<BankAccountLite> iQueryable)
        {
            IQueryable<BankAccountLiteList> result = from a in iQueryable
                                                     select new BankAccountLiteList()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         CreateDate = a.CreateDate,
                                                         UpdateDate = a.UpdateDate,
                                                         LocalName = a.LocalName,
                                                         EnglishName = a.EnglishName,
                                                         BranchNumber = a.BranchNumber,
                                                         AccountNumber = a.AccountNumber,
                                                         IBAN = a.IBAN,
                                                         SwiftCode = a.SwiftCode,
                                                         BankCode = a.BankCode,
                                                         CurrencyId = a.CurrencyId,
                                                         CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                         BranchAddress = a.BranchAddress,
                                                         Inactive = a.Inactive,
                                                         SearchFields = a.SearchFields,
                                                         VatNumber = a.VatNumber,
                                                     };

            return result;
        }

    }
}
