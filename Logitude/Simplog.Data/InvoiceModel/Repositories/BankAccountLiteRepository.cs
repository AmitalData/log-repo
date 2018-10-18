using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class BankAccountLiteRepository : IRepository<BankAccountLite>
    {
        IInvoiceContext invoiceContext;

        public BankAccountLiteRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public BankAccountLiteRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public BankAccountLiteRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public BankAccountLite GetSingleBankAccountLite(string id, int tenant)
        {
            return (from a in context.BankAccountLites.Include("Currency") where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<BankAccountLite> GetBankAccountLites(int tenant)
        {
            return (from a in context.BankAccountLites where a.Tenant == tenant select a);
        }

        public void Add(BankAccountLite entity)
        {
            context.BankAccountLites.Add(entity);
        }

        public void Remove(BankAccountLite entity)
        {
            context.BankAccountLites.Attach(entity);
            context.BankAccountLites.Remove(entity);
        }

        public void Update(BankAccountLite entity)
        {
            try
            {
                context.BankAccountLites.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<BankAccountLite> All()
        {
            return context.BankAccountLites.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<BankAccountLite> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BankAccountLite GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }

}
   
