using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class APInvoiceTransferStatusQuery
    {
        APInvoiceTransferStatusRepository repository;

        public APInvoiceTransferStatusQuery()
        {
            this.repository = new APInvoiceTransferStatusRepository();
        }

        public APInvoiceTransferStatusQuery(int tenant)
        {
            this.repository = new APInvoiceTransferStatusRepository(tenant);
        }

        public APInvoiceTransferStatusQuery(APInvoiceTransferStatusRepository rep)
        {
            this.repository = rep;
        }

        public IQueryable<APInvoiceTransferStatusPM> GetInvoiceStatusePMs()
        {
            return from a in repository.context.APInvoiceTransferStatuses
                   select new APInvoiceTransferStatusPM()
                   {
                       Code = a.Code,
                       Name = a.Name,                       
                       SearchFields = a.SearchFields,
                   };
        }

        public APInvoiceTransferStatusPM GetSingleAPInvoiceTransferStatusPM(string code)
        {
            return (from a in repository.context.APInvoiceTransferStatuses
                    where a.Code == code
                    select new APInvoiceTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<APInvoiceTransferStatusList> GetIQueryableEntityList(IQueryable<APInvoiceTransferStatus> iQueryable)
        {
            IQueryable<APInvoiceTransferStatusList> result = from entity in iQueryable
                                                             select new APInvoiceTransferStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}