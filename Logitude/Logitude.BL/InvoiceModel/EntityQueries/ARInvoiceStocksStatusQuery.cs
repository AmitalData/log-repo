using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System.Linq;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceStocksStatusQuery
    {
        ARInvoiceStocksStatusRepository repository;
        public ARInvoiceStocksStatusQuery()
        {
            repository = new ARInvoiceStocksStatusRepository();
        }

        public ARInvoiceStocksStatusQuery(int tenant)
        {
            repository = new ARInvoiceStocksStatusRepository(tenant);
        }

        public ARInvoiceStocksStatusQuery(ARInvoiceStocksStatusRepository apInvoicePaymentRepository)
        {
            repository = apInvoicePaymentRepository;
        }
        
        public IQueryable<ARInvoiceStocksStatusList> GetIQueryableEntityList(IQueryable<ARInvoiceStocksStatus> iQueryable)
        {
            IQueryable<ARInvoiceStocksStatusList> result = from entity in iQueryable
                                                     select new ARInvoiceStocksStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
