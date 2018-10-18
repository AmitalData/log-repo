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
    public class ARInvoiceStatusQuery
    {
       ARInvoiceStatusRepository repository;

        public ARInvoiceStatusQuery()
        {
            repository = new ARInvoiceStatusRepository(); 
        }

        public ARInvoiceStatusQuery(int tenant)
        {
            repository = new ARInvoiceStatusRepository(tenant);
        }

        public ARInvoiceStatusQuery(ARInvoiceStatusRepository arInvoiceStatusRepository)
        {
            repository = arInvoiceStatusRepository;
        }

        public IQueryable<ARInvoiceStatusPM> GetInvoiceStatusePMs()
        {
            return from a in repository.context.ARInvoiceStatuses
                   select new ARInvoiceStatusPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public ARInvoiceStatusPM GetSingleInvoiceStatusPM(string code)
        {
            return (from a in repository.context.ARInvoiceStatuses
                    where a.Code == code
                    select new ARInvoiceStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }


        public ARInvoiceStatusPM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.ARInvoiceStatuses
                    where a.Code == code 
                    select new ARInvoiceStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public ARInvoiceStatusPM GetSinglePM(string code)
        {
            return (from a in repository.context.ARInvoiceStatuses
                    where a.Code == code
                    select new ARInvoiceStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }
        public IQueryable<ARInvoiceStatusList> GetIQueryableEntityList(IQueryable<ARInvoiceStatus> iQueryable)
        {
            IQueryable<ARInvoiceStatusList> result = from entity in iQueryable
                                                     select new ARInvoiceStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }

    }
}