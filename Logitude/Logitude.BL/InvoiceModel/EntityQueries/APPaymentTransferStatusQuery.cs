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
    public class APPaymentTransferStatusQuery
    {
        APPaymentTransferStatusRepository repository;

        public APPaymentTransferStatusQuery()
        {
            this.repository = new APPaymentTransferStatusRepository();
        }

        public APPaymentTransferStatusQuery(int tenant)
        {
            this.repository = new APPaymentTransferStatusRepository(tenant);
        }

        public APPaymentTransferStatusQuery(APPaymentTransferStatusRepository rep)
        {
            this.repository = rep;
        }

        public IQueryable<APPaymentTransferStatusPM> GetInvoiceStatusePMs()
        {
            return from a in repository.context.APPaymentTransferStatuses
                   select new APPaymentTransferStatusPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public APPaymentTransferStatusPM GetSingleAPPaymentTransferStatusPM(string code)
        {
            return (from a in repository.context.APPaymentTransferStatuses
                    where a.Code == code
                    select new APPaymentTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<APPaymentTransferStatusList> GetIQueryableEntityList(IQueryable<APPaymentTransferStatus> iQueryable)
        {
            IQueryable<APPaymentTransferStatusList> result = from entity in iQueryable
                                                             select new APPaymentTransferStatusList()
                                                             {
                                                                 Name = entity.Name,
                                                                 Code = entity.Code,
                                                                 SearchFields = entity.SearchFields,
                                                             };
            return result;
        }
    }
}
