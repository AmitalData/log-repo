using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceLineActionQuery
    {
        ARInvoiceLineActionRepository repository;

        public ARInvoiceLineActionQuery()
        {
            repository = new ARInvoiceLineActionRepository(); 
        }

        public ARInvoiceLineActionQuery(int tenant)
        {
            repository = new ARInvoiceLineActionRepository(tenant);
        }

        public ARInvoiceLineActionQuery(ARInvoiceLineActionRepository myRepository)
        {
            repository = myRepository;
        }

        public IQueryable<ARInvoiceLineActionList> GetIQueryableEntityList(IQueryable<ARInvoiceLineAction> iQueryable)
        {
            IQueryable<ARInvoiceLineActionList> query2 = from entity in iQueryable
                                                         select new ARInvoiceLineActionList()
                                                     {
                                                         Code = entity.Code,
                                                         Name = entity.Name,
                                                         LocalName = entity.LocalName,
                                                         Inactive = entity.Inactive,
                                                         SearchFields = entity.SearchFields,
                                                     };

            return query2;
        }

        public ARInvoiceLineActionList GetEntityList(string code)
        {
            ARInvoiceLineActionList enntityList = (from entity in repository.context.ARInvoiceLineActions
                                              where entity.Code == code
                                                         select new ARInvoiceLineActionList()
                                                         {
                                                             Code = entity.Code,
                                                             Name = entity.Name,
                                                             LocalName = entity.LocalName,
                                                             Inactive = entity.Inactive,
                                                             SearchFields = entity.SearchFields,
                                                         }).FirstOrDefault();

            return enntityList;
        }
    }
}
