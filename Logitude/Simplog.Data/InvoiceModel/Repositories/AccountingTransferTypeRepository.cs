using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountingTransferTypeRepository : IRepository<AccountingTransferType>
    {              
        IInvoiceContext invoiceContext;
        public AccountingTransferTypeRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public AccountingTransferTypeRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public AccountingTransferTypeRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public AccountingTransferType GetSingleEntity(string code)
        {
            return (from a in context.AccountingTransferTypes where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<AccountingTransferType> GetAllEntities()
        {
            return (from a in context.AccountingTransferTypes select a);
        }

        public IQueryable<AccountingTransferType> GetAll()
        {
            return (from a in context.AccountingTransferTypes select a);
        }

        public void Add(AccountingTransferType entity)
        {
            context.AccountingTransferTypes.Add(entity);
        }

        public void Remove(AccountingTransferType entity)
        {
            context.AccountingTransferTypes.Attach(entity);
            context.AccountingTransferTypes.Remove(entity);
        }

        public void Update(AccountingTransferType entity)
        {
            context.AccountingTransferTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingTransferType> All()
        {
            return context.AccountingTransferTypes.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AccountingTransferType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AccountingTransferType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
