using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARPaymentTransferStatusRepository : IRepository<ARPaymentTransferStatus>
    {
        IInvoiceContext invoiceContext;

        public ARPaymentTransferStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARPaymentTransferStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARPaymentTransferStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ARPaymentTransferStatus> GetARPaymentTransferStatus()
        {
            return context.ARPaymentTransferStatuses;
        }
        public IQueryable<ARPaymentTransferStatus> GetAll()
        {
            return context.ARPaymentTransferStatuses;
        }

        public ARPaymentTransferStatus GetSingleARPaymentTransferStatus(string code)
        {
            return (from a in context.ARPaymentTransferStatuses where a.Code == code select a).FirstOrDefault();
        }

        public void Add(ARPaymentTransferStatus entity)
        {
            context.ARPaymentTransferStatuses.Add(entity);
        }

        public void Remove(ARPaymentTransferStatus entity)
        {
            context.ARPaymentTransferStatuses.Attach(entity);
            context.ARPaymentTransferStatuses.Remove(entity);
        }

        public void Update(ARPaymentTransferStatus entity)
        {
            context.ARPaymentTransferStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARPaymentTransferStatus> All()
        {
            return context.ARPaymentTransferStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARPaymentTransferStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARPaymentTransferStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
