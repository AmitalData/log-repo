using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.InvoiceModel.Repositories
{
    public class SATInvoiceStatusRepository : IRepository<SATInvoiceStatus>
    {
        IInvoiceContext invoiceContext;

        public SATInvoiceStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public SATInvoiceStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public SATInvoiceStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public SATInvoiceStatus GetSingleSATInvoiceStatus(string code)
        {
            return (from a in context.SATInvoiceStatus where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<SATInvoiceStatus> GetSATInvoiceStatus()
        {
            return (from a in context.SATInvoiceStatus select a);
        }

        public IQueryable<SATInvoiceStatus> GetAll()
        {
            return (from a in context.SATInvoiceStatus select a);
        }

        public void Add(SATInvoiceStatus entity)
        {
            context.SATInvoiceStatus.Add(entity);
        }

        public void Remove(SATInvoiceStatus entity)
        {
            context.SATInvoiceStatus.Attach(entity);
            context.SATInvoiceStatus.Remove(entity);
        }

        public void Update(SATInvoiceStatus entity)
        {
            try
            {
                context.SATInvoiceStatus.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<SATInvoiceStatus> All()
        {
            return context.SATInvoiceStatus.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SATInvoiceStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SATInvoiceStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}