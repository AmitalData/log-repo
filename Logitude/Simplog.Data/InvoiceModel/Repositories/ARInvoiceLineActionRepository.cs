using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceLineActionRepository : IRepository<ARInvoiceLineAction>
    {
                IInvoiceContext invoiceContext;

        public ARInvoiceLineActionRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceLineActionRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoiceLineActionRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ARInvoiceLineAction> GetARInvoiceLineActions()
        {
            return context.ARInvoiceLineActions;
        }

        public ARInvoiceLineAction GetSingleARInvoiceLineAction(string code)
        {
            return (from a in context.ARInvoiceLineActions where a.Code == code select a).FirstOrDefault();
        }

        public void Add(ARInvoiceLineAction entity)
        {
            context.ARInvoiceLineActions.Add(entity);
        }

        public void Remove(ARInvoiceLineAction entity)
        {
            context.ARInvoiceLineActions.Attach(entity);
            context.ARInvoiceLineActions.Remove(entity);
        }

        public void Update(ARInvoiceLineAction entity)
        {
            context.ARInvoiceLineActions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceLineAction> All()
        {
            return context.ARInvoiceLineActions.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARInvoiceLineAction> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARInvoiceLineAction GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
