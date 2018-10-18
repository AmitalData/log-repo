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
    public class APPaymentMethodQuery
    {
        APPaymentMethodRepository repository;
        public APPaymentMethodQuery()
        {
            repository = new APPaymentMethodRepository(); 
        }


        public APPaymentMethodQuery(int tenant)
        {
            repository = new APPaymentMethodRepository(tenant);
        }

        public APPaymentMethodQuery(APPaymentMethodRepository apPaymentMethodRepository)
        {
            repository = apPaymentMethodRepository;
        }

        public APPaymentMethodPM GetSingleAPPaymentMethodPM(string id, int tenant)
        {
            return (from a in repository.context.APPaymentMethods
                    where a.Id == id && a.Tenant == tenant
                    select new APPaymentMethodPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        AddedManually = a.AddedManually,
                        Inactive = a.Inactive,
                        AccountingExternalId=a.AccountingExternalId
                    }).FirstOrDefault();
        }

        public APPaymentMethodPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.APPaymentMethods
                    where a.Id == id && a.Tenant == tenant
                    select new APPaymentMethodPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        AddedManually = a.AddedManually,
                        Inactive = a.Inactive,
                        AccountingExternalId = a.AccountingExternalId

                    }).FirstOrDefault();
        }

        public IQueryable<APPaymentMethodPM> GetAPPaymentMethodPMs(int tenant)
        {
            return (from a in repository.context.APPaymentMethods
                    where a.Tenant == tenant
                    select new APPaymentMethodPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        AddedManually = a.AddedManually,
                        Inactive = a.Inactive,
                        AccountingExternalId = a.AccountingExternalId

                    });
        }

        public IQueryable<APPaymentMethodList> GetIQueryableEntityList(IQueryable<APPaymentMethod> iQueryable)
        {
            IQueryable<APPaymentMethodList> result = from a in iQueryable
                                                     select new APPaymentMethodList()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         Code = a.Code,
                                                         Name = a.Name,
                                                         SearchFields = a.SearchFields,
                                                         AddedManually = a.AddedManually,
                                                         Inactive = a.Inactive,
                                                         AccountingExternalId = a.AccountingExternalId

                                                     };

            return result;
        }
    }
}