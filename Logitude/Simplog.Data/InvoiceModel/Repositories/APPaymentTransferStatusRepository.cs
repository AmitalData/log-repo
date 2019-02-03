using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APPaymentTransferStatusRepository : IRepository<APPaymentTransferStatus>
    {
        IInvoiceContext invoiceContext;

        public APPaymentTransferStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public APPaymentTransferStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public APPaymentTransferStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<APPaymentTransferStatus> GetAPPaymentTransferStatus()
        {
            return context.APPaymentTransferStatuses;
        }
        public IQueryable<APPaymentTransferStatus> GetAll()
        {
            return context.APPaymentTransferStatuses;
        }

        public APPaymentTransferStatus GetSingleAPPaymentTransferStatus(string code)
        {
            return (from a in context.APPaymentTransferStatuses where a.Code == code select a).FirstOrDefault();
        }

        public void Add(APPaymentTransferStatus entity)
        {
            context.APPaymentTransferStatuses.Add(entity);
        }

        public void Remove(APPaymentTransferStatus entity)
        {
            context.APPaymentTransferStatuses.Attach(entity);
            context.APPaymentTransferStatuses.Remove(entity);
        }

        public void Update(APPaymentTransferStatus entity)
        {
            context.APPaymentTransferStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<APPaymentTransferStatus> All()
        {
            return context.APPaymentTransferStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APPaymentTransferStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public APPaymentTransferStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
