using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class AccountTypeQuery
    {
         AccountTypeRepository repository;
        public AccountTypeQuery()
        {
            repository = new AccountTypeRepository(); 
        }


        public AccountTypeQuery(int tenant)
        {
            repository = new AccountTypeRepository(tenant);
        }

        public AccountTypeQuery(AccountTypeRepository accountTypeRepository)
        {
            repository = accountTypeRepository;
        }
        public IQueryable<AccountTypePM> GetAccountTypePMs()
        {
            return from a in repository.context.AccountTypes
                   select new AccountTypePM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public AccountTypePM GetSingleAccountTypePM(string code)
        {
            return (from a in repository.context.AccountTypes
                    where a.Code == code
                    select new AccountTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }


        public IQueryable<AccountTypeList> GetIQueryableEntityList(IQueryable<AccountType> iQueryable)
        {
            IQueryable<AccountTypeList> result = from entity in iQueryable
                                                 select new AccountTypeList()
                                                 {
                                                     Code = entity.Code,
                                                     Name = entity.Name,
                                                     SearchFields = entity.SearchFields,
                                                 };
            return result;
        }
    }
}