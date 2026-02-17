using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARPaymentTransferStatusQuery
    {
        ARPaymentTransferStatusRepository repository;

        public ARPaymentTransferStatusQuery()
        {
            this.repository = new ARPaymentTransferStatusRepository();
        }

        public ARPaymentTransferStatusQuery(int tenant)
        {
            this.repository = new ARPaymentTransferStatusRepository(tenant);
        }

        public ARPaymentTransferStatusQuery(ARPaymentTransferStatusRepository rep)
        {
            this.repository = rep;
        }

        public IQueryable<ARPaymentTransferStatusPM> GetInvoiceStatusePMs()
        {
            return from a in repository.context.ARPaymentTransferStatuses
                   select new ARPaymentTransferStatusPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public ARPaymentTransferStatusPM GetSingleARPaymentTransferStatusPM(string code)
        {
            return (from a in repository.context.ARPaymentTransferStatuses
                    where a.Code == code
                    select new ARPaymentTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<ARPaymentTransferStatusList> GetIQueryableEntityList(IQueryable<ARPaymentTransferStatus> iQueryable)
        {
            IQueryable<ARPaymentTransferStatusList> result = from entity in iQueryable
                                                             select new ARPaymentTransferStatusList()
                                                             {
                                                                 Name = entity.Name,
                                                                 Code = entity.Code,
                                                                 SearchFields = entity.SearchFields,
                                                             };
            return result;
        }
    }
}
