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
    public class ARInvoiceTypeQuery
    {
        ARInvoiceTypeRepository repository;
        public ARInvoiceTypeQuery()
        {
            repository = new ARInvoiceTypeRepository(); 
        }


        public ARInvoiceTypeQuery(int tenant)
        {
            repository = new ARInvoiceTypeRepository(tenant);
        }

        public ARInvoiceTypeQuery(ARInvoiceTypeRepository arInvoiceTypeRepository)
        {
            repository = arInvoiceTypeRepository;
        }
        public IQueryable<ARInvoiceTypePM> GetInvoiceTypePMs()
        {
            return from a in repository.context.ARInvoiceTypes
                   select new ARInvoiceTypePM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public ARInvoiceTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.ARInvoiceTypes
                    where a.Code == code
                    select new ARInvoiceTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }


        public ARInvoiceTypePM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.ARInvoiceTypes
                    where a.Code == code
                    select new ARInvoiceTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }
        public IQueryable<ARInvoiceTypeList> GetIQueryableEntityList(IQueryable<ARInvoiceType> iQueryable)
        {
            IQueryable<ARInvoiceTypeList> result = from entity in iQueryable
                                                   select new ARInvoiceTypeList()
                                                   {
                                                       SearchFields = entity.SearchFields,
                                                       Name = entity.Name,
                                                       Code = entity.Code,
                                                   };
            return result;
        }

    }
}