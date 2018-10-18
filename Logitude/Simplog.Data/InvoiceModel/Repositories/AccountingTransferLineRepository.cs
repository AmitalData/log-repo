using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountingTransferLineRepository : IRepository<AccountingTransferLine>
    {                
        IInvoiceContext invoiceContext;
        public AccountingTransferLineRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public AccountingTransferLineRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public AccountingTransferLineRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public AccountingTransferLine GetSingleEntity(string id)
        {
            return (from a in context.AccountingTransferLines where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<AccountingTransferLine> GetARInvoiceTransferLines(string invoiceId, int tenant)
        {
            return (from a in context.AccountingTransferLines where a.EntityId == invoiceId && a.Tenant == tenant select a);
        }

        public IQueryable<AccountingTransferLine> GetAPInvoiceTransferLines(string invoiceId, int tenant)
        {
            return (from a in context.AccountingTransferLines where a.EntityId == invoiceId && a.Tenant == tenant select a);
        }

        public List<string> GetInvoicesIdsList(string transferHeaderId, int tenant)
        {
            List<string> myResult = (from a in context.AccountingTransferLines
                                     where a.Tenant == tenant
                                     && a.AccountingTransferHeaderId == transferHeaderId
                                     select a.EntityId).ToList();

            return myResult;
        }

        public void Add(AccountingTransferLine entity)
        {
            context.AccountingTransferLines.Add(entity);
        }

        public void Remove(AccountingTransferLine entity)
        {
            context.AccountingTransferLines.Attach(entity);
            context.AccountingTransferLines.Remove(entity);
        }

        public void Update(AccountingTransferLine entity)
        {
            context.AccountingTransferLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingTransferLine> All()
        {
            return context.AccountingTransferLines.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AccountingTransferLine> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AccountingTransferLine GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
