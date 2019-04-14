using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceStockRepository : IRepository<ARInvoiceStock>
    {
        IInvoiceContext invoiceContext;
        public ARInvoiceStockRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceStockRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoiceStockRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }
        
        public IQueryable<ARInvoiceStock> GetARInvoiceStocks(int tenant)
        {
            return (from record in context.ARInvoiceStocks where record.Tenant == tenant select record);
        }
        
        public ARInvoiceStock GetSingleARInvoiceStock(string id, int tenant)
        {
            return (from record in context.ARInvoiceStocks where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(ARInvoiceStock entity)
        {
            context.ARInvoiceStocks.Add(entity);
        }

        public void Remove(ARInvoiceStock entity)
        {
            context.ARInvoiceStocks.Attach(entity);
            context.ARInvoiceStocks.Remove(entity);
        }

        public void Update(ARInvoiceStock entity)
        {
            context.ARInvoiceStocks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceStock> All()
        {
            return context.ARInvoiceStocks.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
        
        public List<ARInvoiceStock> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARInvoiceStock GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

     
    }
}
