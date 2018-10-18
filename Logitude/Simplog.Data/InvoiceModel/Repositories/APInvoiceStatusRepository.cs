using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoiceStatusRepository: IRepository<APInvoiceStatus>
    {
        IInvoiceContext invoiceContext;

        public APInvoiceStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public APInvoiceStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public APInvoiceStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public APInvoiceStatus GetSingleAPInvoiceStatus(string code)
        {
            return (from a in context.APInvoiceStatus where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<APInvoiceStatus> GetAPInvoiceStatus()
        {
            return (from a in context.APInvoiceStatus select a);
        }
        public IQueryable<APInvoiceStatus> GetAll()
        {
            return (from a in context.APInvoiceStatus select a);
        }
              
        public void Add(APInvoiceStatus entity)
        {
            context.APInvoiceStatus.Add(entity);
        }

        public void Remove(APInvoiceStatus entity)
        {
            context.APInvoiceStatus.Attach(entity);
            context.APInvoiceStatus.Remove(entity);
        }

        public void Update(APInvoiceStatus entity)
        {
            context.APInvoiceStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<APInvoiceStatus> All()
        {
            return  context.APInvoiceStatus.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APInvoiceStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public APInvoiceStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}