using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
          IInvoiceContext invoiceContext;
        public AccountRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public AccountRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public AccountRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<Account> GetAccounts()
        {
            return context.Accounts;
        }

        public IQueryable<Account> GetAccounts(int tenant)
        {
            return (from record in context.Accounts where record.Tenant == tenant select record);
        }

        public IQueryable<Account> GetAccountsByTenant(int tenant)
        {
            return (from record in context.Accounts where record.Tenant == tenant select record);
        }

     

        public Account GetSingleAccount(string id, int tenant)
        {
            return (from record in context.Accounts where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();          
        }

        public Account GetSingleAccountByCode(string code, int tenant)
        {
            return (from record in context.Accounts where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

      
        public void Add(Account entity)
        {
            context.Accounts.Add(entity);
        }

        public void Remove(Account entity)
        {
            context.Accounts.Attach(entity);
            context.Accounts.Remove(entity);
        }

        public void Update(Account entity)
        {
            context.Accounts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Account> All()
        {
            return context.Accounts.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<Account> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Account GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}