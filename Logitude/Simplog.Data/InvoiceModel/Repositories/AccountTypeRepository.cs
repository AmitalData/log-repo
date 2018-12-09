using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountTypeRepository : IRepository<AccountType>
    {
         IInvoiceContext invoiceContext;
        public AccountTypeRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public AccountTypeRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public AccountTypeRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<AccountType> GetAccountTypes()
        {
            return context.AccountTypes;
        }

        public IQueryable<AccountType> GetAll()
        {
            return context.AccountTypes;
        }

        public AccountType GetSingleAccountType(string code)
        {
            return (from a in context.AccountTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

     
        public void Add(AccountType entity)
        {
            context.AccountTypes.Add(entity);
        }

        public void Remove(AccountType entity)
        {
            context.AccountTypes.Attach(entity);
            context.AccountTypes.Remove(entity);
        }

        public void Update(AccountType entity)
        {
            context.AccountTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountType> All()
        {
            return context.AccountTypes.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<AccountType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AccountType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}