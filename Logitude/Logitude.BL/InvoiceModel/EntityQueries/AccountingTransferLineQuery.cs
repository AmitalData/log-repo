using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class AccountingTransferLineQuery
    {
        AccountingTransferLineRepository repository;
        public AccountingTransferLineQuery()
        {
            this.repository = new AccountingTransferLineRepository();
        }
                
        public AccountingTransferLineQuery(int tenant)
        {
            this.repository = new AccountingTransferLineRepository(tenant);
        }

        public AccountingTransferLineQuery(AccountingTransferLineRepository repository)
        {
            this.repository = repository;
        }

        public AccountingTransferLinePM GetSingleAccountingTransferLinePM(string id)
        {
            AccountingTransferLinePM result =

                (from a in repository.context.AccountingTransferLines
                 where a.Id == id
                 select new AccountingTransferLinePM()
                 {
                     Id = a.Id,
                     AccountingTransferHeaderId = a.AccountingTransferHeaderId,
                     EntityId = a.EntityId,
                     EntityReference = a.EntityReference,
                     Tenant = a.Tenant
                 }).FirstOrDefault();

            return result;
        }

        public IQueryable<AccountingTransferLinePM> GetAccountingTransferLinePMsForTransferHeader(string transferHeaderId, int tenant)
        {
            IQueryable<AccountingTransferLinePM> result =

                (from a in repository.context.AccountingTransferLines
                 where a.AccountingTransferHeaderId == transferHeaderId
                 && a.Tenant == tenant
                 select new AccountingTransferLinePM()
                 {
                     Id = a.Id,
                     AccountingTransferHeaderId = a.AccountingTransferHeaderId,
                     EntityId = a.EntityId,
                     EntityReference = a.EntityReference,
                     Tenant = a.Tenant
                 });

            return result;
        }
    }
}