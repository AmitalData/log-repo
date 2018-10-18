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
    public class APInvoiceTypeQuery
    {
        APInvoiceTypeRepository repository;
        public APInvoiceTypeQuery()
        {
            repository = new APInvoiceTypeRepository(); 
        }


        public APInvoiceTypeQuery(int tenant)
        {
            repository = new APInvoiceTypeRepository(tenant);
        }

        public APInvoiceTypeQuery(APInvoiceTypeRepository apInvoiceTypeRepository)
        {
            repository = apInvoiceTypeRepository;
        }
        public APInvoiceTypePM GetSingleAPInvoiceTypePM(string code)
        {
            return (from a in repository.context.APInvoiceTypes
                    where a.Code == code
                    select new APInvoiceTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,


                    }).FirstOrDefault();
        }

        public IQueryable<APInvoiceTypePM> GetAPInvoiceTypePMs()
        {
            return (from a in repository.context.APInvoiceTypes

                    select new APInvoiceTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,

                    });
        }

        public IQueryable<APInvoiceTypeList> GetIQueryableEntityList(IQueryable<APInvoiceType> iQueryable)
        {
            IQueryable<APInvoiceTypeList> result = from entity in iQueryable
                                                   select new APInvoiceTypeList()
                                                   {
                                                       SearchFields = entity.SearchFields,
                                                       Name = entity.Name,
                                                       Code = entity.Code,
                                                   };
            return result;
        }


    }
}