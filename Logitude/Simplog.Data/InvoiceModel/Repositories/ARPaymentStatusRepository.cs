using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARPaymentStatusRepository : IRepository<ARPaymentStatus>
    {
        IInvoiceContext invoiceContext;

        public ARPaymentStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARPaymentStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARPaymentStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARPaymentStatus GetSingleARPaymentStatus(string code)
        {
            return (from a in context.ARPaymentStatus where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<ARPaymentStatus> GetARPaymentStatus()
        {
            return (from a in context.ARPaymentStatus select a);
        }
        public IQueryable<ARPaymentStatus> GetAll()
        {
            return (from a in context.ARPaymentStatus select a);
        }

        public void Add(ARPaymentStatus entity)
        {
            context.ARPaymentStatus.Add(entity);
        }

        public void Remove(ARPaymentStatus entity)
        {
            context.ARPaymentStatus.Attach(entity);
            context.ARPaymentStatus.Remove(entity);
        }

        public void Update(ARPaymentStatus entity)
        {
            try
            {
                context.ARPaymentStatus.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ARPaymentStatus> All()
        {
            return context.ARPaymentStatus.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARPaymentStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARPaymentStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}