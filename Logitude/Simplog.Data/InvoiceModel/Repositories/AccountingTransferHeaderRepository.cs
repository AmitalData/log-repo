using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountingTransferHeaderRepository:IRepository<AccountingTransferHeader>
    {
        IInvoiceContext invoiceContext;
        public AccountingTransferHeaderRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public AccountingTransferHeaderRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public AccountingTransferHeaderRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public AccountingTransferHeader GetSingleEntity(string id, int tenant)
        {
            return (from a in context.AccountingTransferHeaders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public AccountingTransferHeader GetSingleAccountingTransferHeader(string id, int tenant)
        {
            return (from a in context.AccountingTransferHeaders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AccountingTransferHeader> GetAccountingTransferHeaders(int tenant)
        {
            return (from a in context.AccountingTransferHeaders
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(AccountingTransferHeader entity)
        {
            context.AccountingTransferHeaders.Add(entity);
        }

        public void Remove(AccountingTransferHeader entity)
        {
            context.AccountingTransferHeaders.Attach(entity);
            context.AccountingTransferHeaders.Remove(entity);
        }

        public void Update(AccountingTransferHeader entity)
        {
            context.AccountingTransferHeaders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingTransferHeader> All()
        {
            return context.AccountingTransferHeaders.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AccountingTransferHeader> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AccountingTransferHeader GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
