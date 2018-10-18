using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoiceTransferStatusRepository : IRepository<APInvoiceTransferStatus>
    {                
        IInvoiceContext invoiceContext;

        public APInvoiceTransferStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public APInvoiceTransferStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public APInvoiceTransferStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<APInvoiceTransferStatus> GetAPInvoiceTransferStatus()
        {
            return context.APInvoiceTransferStatuses;
        }
        public IQueryable<APInvoiceTransferStatus> GetAll()
        {
            return context.APInvoiceTransferStatuses;
        }

        public APInvoiceTransferStatus GetSingleAPInvoiceTransferStatus(string code)
        {
            return (from a in context.APInvoiceTransferStatuses where a.Code == code select a).FirstOrDefault();
        }

        public void Add(APInvoiceTransferStatus entity)
        {
            context.APInvoiceTransferStatuses.Add(entity);
        }

        public void Remove(APInvoiceTransferStatus entity)
        {
            context.APInvoiceTransferStatuses.Attach(entity);
            context.APInvoiceTransferStatuses.Remove(entity);
        }

        public void Update(APInvoiceTransferStatus entity)
        {
            context.APInvoiceTransferStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<APInvoiceTransferStatus> All()
        {
            return context.APInvoiceTransferStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APInvoiceTransferStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public APInvoiceTransferStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
