using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;

using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class AccountQuery
    {
       AccountRepository repository;
        public AccountQuery()
        {
            repository = new AccountRepository(); 
        }


        public AccountQuery(AccountRepository accountRepository)
        {
            repository = accountRepository;
        }

        public AccountQuery(int tenant)
        {
            repository = new AccountRepository(tenant);
        }

        public IQueryable<AccountPM> GetAccountPMs()
        {
            return from a in repository.context.Accounts
                   select new AccountPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Code = a.Code,
                       Name = a.Name,
                       AccountTypeCode = a.AccountTypeCode,
                       ExternalAccountingCard = a.ExternalAccountingCard,
                       InActive = a.InActive,
                       AddedManually = a.AddedManually,
                       SearchFields = a.SearchFields,
                   };
        }

        public AccountPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.Accounts
                    where a.Id == id && a.Tenant == tenant
                    select new AccountPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        AccountTypeCode = a.AccountTypeCode,
                        ExternalAccountingCard = a.ExternalAccountingCard,
                        InActive = a.InActive,
                        AddedManually = a.AddedManually,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<AccountPM> GetAccountPMsByTenant(int tenant)
        {
            IQueryable<AccountPM> accounts = (from a in repository.context.Accounts
                                              where a.Tenant == tenant
                                              select new AccountPM()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  AccountTypeCode = a.AccountTypeCode,
                                                  ExternalAccountingCard = a.ExternalAccountingCard,
                                                  InActive = a.InActive,
                                                  AddedManually = a.AddedManually,
                                                  SearchFields = a.SearchFields,
                                              });
            return accounts;
        }

        public IQueryable<AccountPM> GetAccountByCodeOrName(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Accounts
                         where a.Tenant == tenant
                         select new AccountPM()
                         {
                             AddedManually = a.AddedManually,
                             Code = a.Code,
                             Id = a.Id,
                             InActive = a.InActive,
                             Name = a.Name,
                             Tenant = a.Tenant,
                             ExternalAccountingCard = a.ExternalAccountingCard,
                             AccountTypeCode = a.AccountTypeCode,
                             SearchFields = a.SearchFields,
                         }).AsQueryable();

            IQueryable<AccountPM> query2 = null;
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
                        query2 = query.Where(d => d.Name.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.Name.ToUpper().StartsWith(name.ToUpper()));

                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<AccountList> GetIQueryableEntityList(IQueryable<Account> iQueryable)
        {
            IQueryable<AccountList> result = from account in iQueryable
                                             select new AccountList()
                                             {
                                                 Id = account.Id,
                                                 Tenant = account.Tenant,
                                                 Code = account.Code,
                                                 Name = account.Name,
                                                 AccountTypeCode = account.AccountTypeCode,
                                                 ExternalAccountingCard = account.ExternalAccountingCard,
                                                 InActive = account.InActive,
                                                 AddedManually = account.AddedManually,
                                                 SearchFields = account.SearchFields,
                                             };
            return result;
        }

    }
}