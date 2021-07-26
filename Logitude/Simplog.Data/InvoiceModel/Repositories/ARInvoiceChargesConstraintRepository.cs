using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceChargesConstraintRepository : IRepository<ARInvoiceChargesConstraint>
    {
        IInvoiceContext invoiceContext;

        public ARInvoiceChargesConstraintRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARInvoiceChargesConstraintRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceChargesConstraintRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoiceChargesConstraint GetSingleARInvoiceChargesConstraint(string id, int tenant)
        {
            return (from a in context.ARInvoiceChargesConstraints where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }
        public ARInvoiceChargesConstraint GetARInvoiceChargesConstraintByReceivableId(string receivableId, int tenant)
        {
            return (from a in context.ARInvoiceChargesConstraints where a.ReceivableId == receivableId && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<ARInvoiceChargesConstraint> GetARInvoiceChargesConstraints(int tenant)
        {
            return (from a in context.ARInvoiceChargesConstraints where a.Tenant == tenant select a);
        }

        public void Add(ARInvoiceChargesConstraint entity)
        {
            context.ARInvoiceChargesConstraints.Add(entity);
        }

        public void Remove(ARInvoiceChargesConstraint entity)
        {
            context.ARInvoiceChargesConstraints.Attach(entity);
            context.ARInvoiceChargesConstraints.Remove(entity);
        }

        public void Update(ARInvoiceChargesConstraint entity)
        {
            try
            {
                context.ARInvoiceChargesConstraints.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ARInvoiceChargesConstraint> All()
        {
            return context.ARInvoiceChargesConstraints.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARInvoiceChargesConstraint> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARInvoiceChargesConstraint GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
