using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.InvoiceModel.Repositories
{
    public class SATTransferStatusRepository : IRepository<SATTransferStatus>
    {
        IInvoiceContext invoiceContext;

        public SATTransferStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public SATTransferStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public SATTransferStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public SATTransferStatus GetSingleSATTransferStatus(string code)
        {
            return (from a in context.SATTransferStatus where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<SATTransferStatus> GetSATTransferStatus()
        {
            return (from a in context.SATTransferStatus select a);
        }
        public IQueryable<SATTransferStatus> GetAll()
        {
            return (from a in context.SATTransferStatus select a);
        }

        public void Add(SATTransferStatus entity)
        {
            context.SATTransferStatus.Add(entity);
        }

        public void Remove(SATTransferStatus entity)
        {
            context.SATTransferStatus.Attach(entity);
            context.SATTransferStatus.Remove(entity);
        }

        public void Update(SATTransferStatus entity)
        {
            try
            {
                context.SATTransferStatus.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<SATTransferStatus> All()
        {
            return context.SATTransferStatus.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SATTransferStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SATTransferStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}