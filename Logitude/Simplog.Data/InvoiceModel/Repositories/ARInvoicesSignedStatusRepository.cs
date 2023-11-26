using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoicesSignedStatusRepository : IRepository<ARInvoicesSignedStatus>
    {
        IInvoiceContext invoiceContext;

        public ARInvoicesSignedStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoicesSignedStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoicesSignedStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ARInvoicesSignedStatus> GetARInvoicesSignedStatuses()
        {
            return context.ARInvoicesSignedStatuses;
        }

        public IQueryable<ARInvoicesSignedStatus> GetAll()
        {
            return context.ARInvoicesSignedStatuses;
        }

        public ARInvoicesSignedStatus GetSingleARInvoicesSignedStatus(string code)
        {
            return (from a in context.ARInvoicesSignedStatuses where a.Code == code select a).FirstOrDefault();
        }

        public void Add(ARInvoicesSignedStatus entity)
        {
            context.ARInvoicesSignedStatuses.Add(entity);
        }

        public void Remove(ARInvoicesSignedStatus entity)
        {
            context.ARInvoicesSignedStatuses.Attach(entity);
            context.ARInvoicesSignedStatuses.Remove(entity);
        }

        public void Update(ARInvoicesSignedStatus entity)
        {
            context.ARInvoicesSignedStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoicesSignedStatus> All()
        {
            return context.ARInvoicesSignedStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARInvoicesSignedStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARInvoicesSignedStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
