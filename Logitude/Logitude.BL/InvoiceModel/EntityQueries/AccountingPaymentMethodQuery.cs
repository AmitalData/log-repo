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
    public class AccountingPaymentMethodQuery
    {
        AccountingPaymentMethodRepository repository;
        public AccountingPaymentMethodQuery()
        {
            repository = new AccountingPaymentMethodRepository(); 
        }


        public AccountingPaymentMethodQuery(int tenant)
        {
            repository = new AccountingPaymentMethodRepository(tenant);
        }

        public AccountingPaymentMethodQuery(AccountingPaymentMethodRepository arPaymentMethodRepository)
        {
            repository = arPaymentMethodRepository;
        }

        public AccountingPaymentMethodPM GetSinglePaymentMethodPM(string id, int tenant)
        {
            return (from a in repository.context.AccountingPaymentMethods
                    where a.Id == id && a.Tenant == tenant
                    select new AccountingPaymentMethodPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        AddedManually = a.AddedManually,
                        Inactive = a.Inactive,
                        APExternalId=a.APExternalId,
                        ARExternalId = a.ARExternalId,
                         IsAR = a.IsAR,
                        IsAP = a.IsAP,
                    }).FirstOrDefault();
        }

        public AccountingPaymentMethodPM GetSinglePaymentMethodPMByCode(string code, int tenant)
        {
            return (from a in repository.context.AccountingPaymentMethods
                    where a.Code == code && a.Tenant == tenant
                    select new AccountingPaymentMethodPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        AddedManually = a.AddedManually,
                        Inactive = a.Inactive,
                        APExternalId = a.APExternalId,
                        ARExternalId = a.ARExternalId,
                        IsAR = a.IsAR,
                        IsAP = a.IsAP,
                    }).FirstOrDefault();
        }


        public AccountingPaymentMethodPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.AccountingPaymentMethods
                    where a.Id == id && a.Tenant == tenant
                    select new AccountingPaymentMethodPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        AddedManually = a.AddedManually,
                        Inactive = a.Inactive,
                        APExternalId = a.APExternalId,
                        ARExternalId = a.ARExternalId,
                        IsAR = a.IsAR,
                        IsAP = a.IsAP,
                    }).FirstOrDefault();
        }

        public IQueryable<AccountingPaymentMethodPM> GetPaymentMethodPMs(int tenant)
        {
            return (from a in repository.context.AccountingPaymentMethods
                    where a.Tenant == tenant
                    select new AccountingPaymentMethodPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        AddedManually = a.AddedManually,
                        Inactive = a.Inactive,
                        APExternalId = a.APExternalId,
                        ARExternalId = a.ARExternalId,
                        IsAR = a.IsAR,
                        IsAP = a.IsAP,
                    });
        }

        public IQueryable<AccountingPaymentMethodList> GetIQueryableEntityList(IQueryable<AccountingPaymentMethod> iQueryable)
        {
            IQueryable<AccountingPaymentMethodList> result = from a in iQueryable
                                                     select new AccountingPaymentMethodList()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         Code = a.Code,
                                                         Name = a.Name,
                                                         SearchFields = a.SearchFields,
                                                         AddedManually = a.AddedManually,
                                                         Inactive = a.Inactive,
                                                         APExternalId = a.APExternalId,
                                                         ARExternalId = a.ARExternalId,
                                                         IsAR = a.IsAR,
                                                         IsAP = a.IsAP,
                                                     };

            return result;
        }
    }
}