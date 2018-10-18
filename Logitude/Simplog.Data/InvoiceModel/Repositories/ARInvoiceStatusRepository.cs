using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceStatusRepository: IRepository<ARInvoiceStatus>
    {
        IInvoiceContext invoiceContext;

        public ARInvoiceStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoiceStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ARInvoiceStatus> GetARInvoiceStatus()
        {
            return context.ARInvoiceStatuses;
        }

        public IQueryable<ARInvoiceStatus> GetAll()
        {
            return context.ARInvoiceStatuses;
        }

        public ARInvoiceStatus GetSingleARInvoiceStatus(string code)
        {
            return (from a in context.ARInvoiceStatuses where a.Code == code select a).FirstOrDefault();
        }
        
        public void Add(ARInvoiceStatus entity)
        {
            context.ARInvoiceStatuses.Add(entity);
        }

        public void Remove(ARInvoiceStatus entity)
        {
            context.ARInvoiceStatuses.Attach(entity);
            context.ARInvoiceStatuses.Remove(entity);
        }

        public void Update(ARInvoiceStatus entity)
        {
            context.ARInvoiceStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceStatus> All()
        {
            return context.ARInvoiceStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARInvoiceStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARInvoiceStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}