using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class AccountingSystemsSyncStatusQuery
    {

          AccountingSystemsSyncStatusRepository repository;
        public AccountingSystemsSyncStatusQuery()
        {
            repository = new AccountingSystemsSyncStatusRepository(); 
        }


        public AccountingSystemsSyncStatusQuery(AccountingSystemsSyncStatusRepository AccountingSystemsSyncStatusRepository)
        {
            repository = AccountingSystemsSyncStatusRepository;
        }

        public AccountingSystemsSyncStatusQuery(int tenant)
        {
            repository = new AccountingSystemsSyncStatusRepository(tenant);
        }

        public IQueryable<AccountingSystemsSyncStatusPM> GetAccountingSystemsSyncStatusPMs()
        {
            return from a in repository.context.AccountingSystemsSyncStatuses
                   select new AccountingSystemsSyncStatusPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ExternalCodesLastUpdate = a.ExternalCodesLastUpdate,
                       LastError = a.LastError,
                       LastRequestDate = a.LastRequestDate,
                       SyncInterval = a.SyncInterval,
                       LastErrorDate = a.LastErrorDate,
                   };
        }

        public AccountingSystemsSyncStatusPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.AccountingSystemsSyncStatuses
                    where a.Id == id && a.Tenant == tenant
                    select new AccountingSystemsSyncStatusPM()
                    {
                        Id = a.Id,
                       Tenant = a.Tenant,
                       ExternalCodesLastUpdate = a.ExternalCodesLastUpdate,
                       LastError = a.LastError,
                       LastRequestDate = a.LastRequestDate,
                       SyncInterval = a.SyncInterval,
                        LastErrorDate = a.LastErrorDate,
                    }).FirstOrDefault();
        }

        public AccountingSystemsSyncStatusPM GetSingleAccountingSystemsSyncStatusPMForTenant(int tenant)
        {
            return (from a in repository.context.AccountingSystemsSyncStatuses
                    where  a.Tenant == tenant
                    select new AccountingSystemsSyncStatusPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ExternalCodesLastUpdate = a.ExternalCodesLastUpdate,
                        LastError = a.LastError,
                        LastRequestDate = a.LastRequestDate,
                        SyncInterval = a.SyncInterval,
                        LastErrorDate = a.LastErrorDate,
                    }).FirstOrDefault();
        }


     



        public IQueryable<AccountingSystemsSyncStatusList> GetIQueryableEntityList(IQueryable<AccountingSystemsSyncStatus> iQueryable)
        {
            IQueryable<AccountingSystemsSyncStatusList> result = from AccountingSystemsSyncStatus in iQueryable
                                             select new AccountingSystemsSyncStatusList()
                                             {
                                                 Id = AccountingSystemsSyncStatus.Id,
                                                 Tenant =AccountingSystemsSyncStatus.Tenant,
                                                 ExternalCodesLastUpdate = AccountingSystemsSyncStatus.ExternalCodesLastUpdate,
                                                 LastError = AccountingSystemsSyncStatus.LastError,
                                                 LastRequestDate = AccountingSystemsSyncStatus.LastRequestDate,
                                                 SyncInterval = AccountingSystemsSyncStatus.SyncInterval,
                                                 LastErrorDate = AccountingSystemsSyncStatus.LastErrorDate,

                                             };
            return result;
        }


    }
}