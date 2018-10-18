using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceTransferStatusQuery
    {
        ARInvoiceTransferStatusRepository repository;

        public ARInvoiceTransferStatusQuery()
        {
            this.repository = new ARInvoiceTransferStatusRepository();
        }

        public ARInvoiceTransferStatusQuery(int tenant)
        {
            this.repository = new ARInvoiceTransferStatusRepository(tenant);
        }

        public ARInvoiceTransferStatusQuery(ARInvoiceTransferStatusRepository rep)
        {
            this.repository = rep;
        }

        public IQueryable<ARInvoiceTransferStatusPM> GetInvoiceStatusePMs()
        {
            return from a in repository.context.ARInvoiceTransferStatuses
                   select new ARInvoiceTransferStatusPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       
                       SearchFields = a.SearchFields,
                   };
        }

        public ARInvoiceTransferStatusPM GetSingleARInvoiceTransferStatusPM(string code)
        {
            return (from a in repository.context.ARInvoiceTransferStatuses
                    where a.Code == code
                    select new ARInvoiceTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }
        


       public ARInvoiceTransferStatusPM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.ARInvoiceTransferStatuses
                    where a.Code == code
                    select new ARInvoiceTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public ARInvoiceTransferStatusPM GetSinglePM(string code)
        {
            return (from a in repository.context.ARInvoiceTransferStatuses
                    where a.Code == code
                    select new ARInvoiceTransferStatusPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        

        public IQueryable<ARInvoiceTransferStatusList> GetIQueryableEntityList(IQueryable<ARInvoiceTransferStatus> iQueryable)
        {
            IQueryable<ARInvoiceTransferStatusList> result = from entity in iQueryable
                                                             select new ARInvoiceTransferStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}