using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountingSystemsSyncStatusRepository : IRepository<AccountingSystemsSyncStatus>
    {

          IInvoiceContext invoiceContext;
        public AccountingSystemsSyncStatusRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public AccountingSystemsSyncStatusRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public AccountingSystemsSyncStatusRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<AccountingSystemsSyncStatus> GetAccountingSystemsSyncStatuses()
        {
            return context.AccountingSystemsSyncStatuses;
        }

        public IQueryable<AccountingSystemsSyncStatus> GetAccountingSystemsSyncStatuses(int tenant)
        {
            return (from record in context.AccountingSystemsSyncStatuses where record.Tenant == tenant select record);
        }

        public IQueryable<AccountingSystemsSyncStatus> GetAccountingSystemsSyncStatusesByTenant(int tenant)
        {
            return (from record in context.AccountingSystemsSyncStatuses where record.Tenant == tenant select record);
        }



        public AccountingSystemsSyncStatus GetSingleAccountingSystemsSyncStatus(string id, int tenant)
        {
            return (from record in context.AccountingSystemsSyncStatuses where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public AccountingSystemsSyncStatus GetSingleAccountingSystemsSyncStatus( int tenant)
        {
            return (from record in context.AccountingSystemsSyncStatuses where  record.Tenant == tenant select record).FirstOrDefault();
        }


      


        public AccountingSystemsSyncStatus GetSingleAccountingSystemsSyncStatusByCode(string id, int tenant)
        {
            return (from record in context.AccountingSystemsSyncStatuses where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public void Add(AccountingSystemsSyncStatus entity)
        {
            context.AccountingSystemsSyncStatuses.Add(entity);
        }

        public void Remove(AccountingSystemsSyncStatus entity)
        {
            context.AccountingSystemsSyncStatuses.Attach(entity);
            context.AccountingSystemsSyncStatuses.Remove(entity);
        }

        public void Update(AccountingSystemsSyncStatus entity)
        {
            context.AccountingSystemsSyncStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingSystemsSyncStatus> All()
        {
            return context.AccountingSystemsSyncStatuses.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }






        public List<AccountingSystemsSyncStatus> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AccountingSystemsSyncStatus GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
