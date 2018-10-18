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
    public class APPaymentStatusQuery
    {
        APPaymentStatusRepository repository;
        public APPaymentStatusQuery()
        {
            repository = new APPaymentStatusRepository(); 
        }


        public APPaymentStatusQuery(int tenant)
        {
            repository = new APPaymentStatusRepository(tenant);
        }

        public APPaymentStatusQuery(APPaymentStatusRepository apPaymentStatusRepository)
        {
            repository = apPaymentStatusRepository;
        }

        public APPaymentStatusPM GetSingleAPPaymentStatusPM(string code)
        {
            return (from a in repository.context.APPaymentStatus
                    where a.Code == code
                    select new APPaymentStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<APPaymentStatusPM> GetAPPaymentStatusPMs()
        {
            return (from a in repository.context.APPaymentStatus

                    select new APPaymentStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<APPaymentStatusList> GetIQueryableEntityList(IQueryable<APPaymentStatus> iQueryable)
        {
            IQueryable<APPaymentStatusList> query2 = from entity in iQueryable
                                                     select new APPaymentStatusList()
                                                     {
                                                         SearchFields = entity.SearchFields,
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                     };

            return query2;
        }
    }
}