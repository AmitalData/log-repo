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
    public class AccountingSystemsSettingQuery
    {

         AccountingSystemsSettingRepository repository;
        public AccountingSystemsSettingQuery()
        {
            repository = new AccountingSystemsSettingRepository(); 
        }


        public AccountingSystemsSettingQuery(AccountingSystemsSettingRepository AccountingSystemsSettingRepository)
        {
            repository = AccountingSystemsSettingRepository;
        }

        public AccountingSystemsSettingQuery(int tenant)
        {
            repository = new AccountingSystemsSettingRepository(tenant);
        }

        public IQueryable<AccountingSystemsSettingPM> GetAccountingSystemsSettingPMs()
        {
            return from a in repository.context.AccountingSystemsSettings
                   select new AccountingSystemsSettingPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       GetExternalCodeInterval = a.GetExternalCodeInterval,
                       UpdateOnNextRequest = a.UpdateOnNextRequest
                   };
        }

        public AccountingSystemsSettingPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.AccountingSystemsSettings
                    where a.Id == id && a.Tenant == tenant
                    select new AccountingSystemsSettingPM()
                    {
                        Id = a.Id,
                       Tenant = a.Tenant,
                        GetExternalCodeInterval = a.GetExternalCodeInterval,
                        UpdateOnNextRequest = a.UpdateOnNextRequest
                    }).FirstOrDefault();
        }

        public AccountingSystemsSettingPM GetAccountingSystemsSettingPMByTenant(int tenant)
        {
            return (from a in repository.context.AccountingSystemsSettings
                    where  a.Tenant == tenant
                    select new AccountingSystemsSettingPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        GetExternalCodeInterval = a.GetExternalCodeInterval,
                        UpdateOnNextRequest = a.UpdateOnNextRequest
                    }).FirstOrDefault();
        }



        public IQueryable<AccountingSystemsSettingList> GetIQueryableEntityList(IQueryable<AccountingSystemsSetting> iQueryable)
        {
            IQueryable<AccountingSystemsSettingList> result = from AccountingSystemsSetting in iQueryable
                                             select new AccountingSystemsSettingList()
                                             {
                                                 Id = AccountingSystemsSetting.Id,
                                                 Tenant =AccountingSystemsSetting.Tenant,
                                                 GetExternalCodeInterval = AccountingSystemsSetting.GetExternalCodeInterval,
                                                 UpdateOnNextRequest = AccountingSystemsSetting.UpdateOnNextRequest
                                             };
            return result;
        }


    }
}