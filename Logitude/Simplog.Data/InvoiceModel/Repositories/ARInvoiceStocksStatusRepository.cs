using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceStocksStatusRepository : IRepository<ARInvoiceStocksStatus>
    {
        IInvoiceContext invoiceContext;

        public ARInvoiceStocksStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARInvoiceStocksStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARInvoiceStocksStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARInvoiceStocksStatus GetSingleARInvoiceStocksStatus(string code)
        {
            return (from a in context.ARInvoiceStocksStatus where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<ARInvoiceStocksStatus> GetARInvoiceStocksStatus()
        {
            return (from a in context.ARInvoiceStocksStatus select a);
        }
        public IQueryable<ARInvoiceStocksStatus> GetAll()
        {
            return (from a in context.ARInvoiceStocksStatus select a);
        }

        public void Add(ARInvoiceStocksStatus entity)
        {
            context.ARInvoiceStocksStatus.Add(entity);
        }

        public void Remove(ARInvoiceStocksStatus entity)
        {
            context.ARInvoiceStocksStatus.Attach(entity);
            context.ARInvoiceStocksStatus.Remove(entity);
        }

        public void Update(ARInvoiceStocksStatus entity)
        {
            context.ARInvoiceStocksStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceStocksStatus> All()
        {
            return context.ARInvoiceStocksStatus.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARInvoiceStocksStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARInvoiceStocksStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}