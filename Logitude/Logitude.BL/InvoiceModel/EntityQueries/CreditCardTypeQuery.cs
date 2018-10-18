using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class CreditCardTypeQuery
    {
        CreditCardTypeRepository repository;

        public CreditCardTypeQuery()
        {
            repository = new CreditCardTypeRepository();
        }

        public CreditCardTypeQuery(int tenant)
        {
            repository = new CreditCardTypeRepository(tenant);
        }

        public CreditCardTypeQuery(CreditCardTypeRepository creditCardTypeRepository)
        {
            repository = creditCardTypeRepository;
        }

        public CreditCardTypePM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.CreditCardTypes
                    where a.Id == id && a.Tenant == tenant
                    select new CreditCardTypePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        Tenant = a.Tenant,
                        SearchFields = a.SearchFields,
                        InActive = a.InActive,
                        BankAccountId = a.BankAccountId,
                    }).FirstOrDefault();
        }

        public IQueryable<CreditCardTypePM> GetCreditCardTypePMs(int tenant)
        {
            return (from a in repository.context.CreditCardTypes
                    where a.Tenant == tenant
                    select new CreditCardTypePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        Tenant = a.Tenant,
                        SearchFields = a.SearchFields,
                        InActive = a.InActive,
                        BankAccountId = a.BankAccountId,
                    });
        }


        public IQueryable<CreditCardTypePM> GetCreditCardTypePMsByTenant(int tenant)
        {
            return (from a in repository.context.CreditCardTypes
                    where a.Tenant == tenant
                    select new CreditCardTypePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        Tenant = a.Tenant,
                        SearchFields = a.SearchFields,
                        InActive = a.InActive,
                        BankAccountId = a.BankAccountId,
                    });
        }

        public IQueryable<CreditCardTypeList> GetIQueryableEntityList(IQueryable<CreditCardType> iQueryable)
        {
            IQueryable<CreditCardTypeList> result = from entity in iQueryable
                                                    select new CreditCardTypeList()
                                                     {
                                                         Id = entity.Id,
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         Tenant = entity.Tenant,
                                                         SearchFields = entity.SearchFields,
                                                         InActive = entity.InActive,
                                                         BankAccountId = entity.BankAccountId,
                                                     };

            return result;
        }
    }
}