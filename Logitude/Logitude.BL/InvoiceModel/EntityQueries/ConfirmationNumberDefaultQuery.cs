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
    public class ConfirmationNumberDefaultQuery
    {
        ConfirmationNumberDefaultRepository repository;

        public ConfirmationNumberDefaultQuery()
        {
            repository = new ConfirmationNumberDefaultRepository(); 
        }

        public ConfirmationNumberDefaultQuery(int tenant)
        {
            repository = new ConfirmationNumberDefaultRepository(tenant);
        }

        public ConfirmationNumberDefaultQuery(ConfirmationNumberDefaultRepository arInvoiceStatusRepository)
        {
            repository = arInvoiceStatusRepository;
        }
        public ConfirmationNumberDefaultPM GetSinglePM(string Id,int tenant)
        {
            return (from a in repository.context.ConfirmationNumberDefaults
                    where a.Id == Id
                    select new ConfirmationNumberDefaultPM()
                    {
                        Tenant = a.Tenant,
                        FromDate = a.FromDate,
                        AmountForConfirmationNumber = a.AmountForConfirmationNumber,
                        Id = a.Id,
                    }).FirstOrDefault();
        }

        public IQueryable<ConfirmationNumberDefaultList> GetIQueryableEntityList(IQueryable<ConfirmationNumberDefault> iQueryable)
        {
            IQueryable<ConfirmationNumberDefaultList> result = from entity in iQueryable
                                                     select new ConfirmationNumberDefaultList()
                                                     {
                                                         Tenant = entity.Tenant,
                                                         Id = entity.Id,
                                                         FromDate = entity.FromDate,
                                                         AmountForConfirmationNumber=entity.AmountForConfirmationNumber,
                                                     };
            return result;
        }
        public IQueryable<ConfirmationNumberDefaultList> GetIQueryableEntityListByTenant(int tenant)
        {
            IQueryable<ConfirmationNumberDefaultList> result = from entity in repository.context.ConfirmationNumberDefaults
                                                               where entity.Tenant== tenant
                                                               select new ConfirmationNumberDefaultList()
                                                               {
                                                                   Tenant = entity.Tenant,
                                                                   Id = entity.Id,
                                                                   FromDate = entity.FromDate,
                                                                   AmountForConfirmationNumber = entity.AmountForConfirmationNumber,
                                                               };
            return result;
        }


    }
}