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
    public class ARPaymentStatusQuery
    {

        ARPaymentStatusRepository repository;
        public ARPaymentStatusQuery()
        {
            repository = new ARPaymentStatusRepository(); 
        }


        public ARPaymentStatusQuery(int tenant)
        {
            repository = new ARPaymentStatusRepository(tenant);
        }

        public ARPaymentStatusQuery(ARPaymentStatusRepository arPaymentStatusRepository)
        {
            repository = arPaymentStatusRepository;
        }


        public ARPaymentStatusPM GetSingleARPaymentStatusPM(string code)
        {
            return (from a in repository.context.ARPaymentStatus
                    where a.Code == code
                    select new ARPaymentStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,

                    }).FirstOrDefault();
        }

        public IQueryable<ARPaymentStatusPM> GetARPaymentStatusPMs()
        {
            return (from a in repository.context.ARPaymentStatus

                    select new ARPaymentStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                    });
        }

        public IQueryable<ARPaymentStatusList> GetIQueryableEntityList(IQueryable<ARPaymentStatus> iQueryable)
        {
            IQueryable<ARPaymentStatusList> result = from entity in iQueryable
                                                     select new ARPaymentStatusList()
                                                     {
                                                         SearchFields = entity.SearchFields,
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                     };
            return result;
        }
    }
}