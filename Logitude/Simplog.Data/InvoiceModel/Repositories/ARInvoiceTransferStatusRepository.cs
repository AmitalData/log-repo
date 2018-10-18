using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceTransferStatusRepository : IRepository<ARInvoiceTransferStatus>
    {
        IInvoiceContext invoiceContext;

        public ARInvoiceTransferStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceTransferStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoiceTransferStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ARInvoiceTransferStatus> GetARInvoiceTransferStatus()
        {
            return context.ARInvoiceTransferStatuses;
        }
        public IQueryable<ARInvoiceTransferStatus> GetAll()
        {
            return context.ARInvoiceTransferStatuses;
        }
        public ARInvoiceTransferStatus GetSingleARInvoiceTransferStatus(string code)
        {
            return (from a in context.ARInvoiceTransferStatuses where a.Code == code select a).FirstOrDefault();
        }

        public void Add(ARInvoiceTransferStatus entity)
        {
            context.ARInvoiceTransferStatuses.Add(entity);
        }

        public void Remove(ARInvoiceTransferStatus entity)
        {
            context.ARInvoiceTransferStatuses.Attach(entity);
            context.ARInvoiceTransferStatuses.Remove(entity);
        }

        public void Update(ARInvoiceTransferStatus entity)
        {
            context.ARInvoiceTransferStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceTransferStatus> All()
        {
            return context.ARInvoiceTransferStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARInvoiceTransferStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARInvoiceTransferStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
