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
    public class APInvoiceStatusQuery
    {
        APInvoiceStatusRepository repository;
        public APInvoiceStatusQuery()
        {
            repository = new APInvoiceStatusRepository(); 
        }


        public APInvoiceStatusQuery(int tenant)
        {
            repository = new APInvoiceStatusRepository(tenant);
        }

        public APInvoiceStatusQuery(APInvoiceStatusRepository apInvoicePaymentRepository)
        {
            repository = apInvoicePaymentRepository;
        }

        public APInvoiceStatusPM GetSingleAPInvoiceStatusPM(string code)
        {
            return (from a in repository.context.APInvoiceStatus
                    where a.Code == code
                    select new APInvoiceStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,


                    }).FirstOrDefault();
        }

        public IQueryable<APInvoiceStatusPM> GetAPInvoiceStatusPMs()
        {
            return (from a in repository.context.APInvoiceStatus

                    select new APInvoiceStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,

                    });
        }


        public IQueryable<APInvoiceStatusList> GetIQueryableEntityList(IQueryable<APInvoiceStatus> iQueryable)
        {
            IQueryable<APInvoiceStatusList> result = from entity in iQueryable
                                                     select new APInvoiceStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }

    }
}