using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceStockLineRepository : IRepository<ARInvoiceStockLine>
    {
        IInvoiceContext invoiceContext;
        public IInvoiceContext context
        {
            get { return this.invoiceContext; }
        }

        public ARInvoiceStockLineRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ARInvoiceStockLineRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }
        public ARInvoiceStockLineRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        
        public IQueryable<ARInvoiceStockLine> GetARInvoiceStockLinesByStockId(string stockId, int tenant)
        {
            return (from a in invoiceContext.ARInvoiceStockLines
                    where a.Tenant == tenant && a.ARInvoiceStockId == stockId
                    select a);
        }

        public IQueryable<ARInvoiceStockLine> GetARInvoiceStockLinesByTenant(int tenant)
        {
            IQueryable<ARInvoiceStockLine> invoiceLines = from a in invoiceContext.ARInvoiceStockLines
                                                     where a.Tenant == tenant
                                                     select a;
            return invoiceLines;
        }
               
        public ARInvoiceStockLine GetSingleARInvoiceStockLine(string id, int tenant)
        {
            return (from a in invoiceContext.ARInvoiceStockLines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        
        public void Add(ARInvoiceStockLine entity)
        {
            invoiceContext.ARInvoiceStockLines.Add(entity);
        }

        public void Remove(ARInvoiceStockLine entity)
        {
            invoiceContext.ARInvoiceStockLines.Attach(entity);
            invoiceContext.ARInvoiceStockLines.Remove(entity);
        }

        public void Update(ARInvoiceStockLine entity)
        {
            invoiceContext.ARInvoiceStockLines.Attach(entity);
            invoiceContext.SetAsModified(entity);
        }

        public List<ARInvoiceStockLine> All()
        {
            return invoiceContext.ARInvoiceStockLines.ToList();
        }

        public void SubmitChanges()
        {
            invoiceContext.SaveChanges();
        }
        
        public List<ARInvoiceStockLine> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARInvoiceStockLine GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
