using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class TenantSettingQuery
    {
        TenantSettingRepository repository;
        public TenantSettingQuery()
        {
            repository = new TenantSettingRepository(); 
        }

        public TenantSettingQuery(int tenant)
        {
            repository = new TenantSettingRepository(tenant);
        }

        public TenantSettingQuery(TenantSettingRepository tenantSettingRepository)
        {
            repository = tenantSettingRepository;
        }

        public IQueryable<TenantSettingPM> GetTenantSettingsByTenant(int tenant)
        {
            IQueryable<TenantSettingPM> result = (from a in repository.context.TenantSettings
                                                  where a.Tenant == tenant
                                                  select new TenantSettingPM()
                                                  {
                                                      Id = a.Id,
                                                      ObjectTableId = a.ObjectTableId,
                                                      Tenant = a.Tenant,
                                                      SettingCode = a.SettingCode,
                                                      SettingValue = a.SettingValue,
                                                      Prefix = a.Prefix,
                                                      Size = a.Size,
                                                      DontIncludeDirects = a.DontIncludeDirects,
                                                      IsDocumentFilingByEmailEnabled = a.IsDocumentFilingByEmailEnabled,
                                                  }
       );

            return result;
        }


        public IQueryable<TenantSettingPM> GetTenantSettingsByObjectTableId(string objectTableId, int tenant)
        {
            IQueryable<TenantSettingPM> result = (from a in repository.context.TenantSettings
                                                  where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                                  select new TenantSettingPM()
                                                  {
                                                      Id = a.Id,
                                                      ObjectTableId = a.ObjectTableId,
                                                      Tenant = a.Tenant,
                                                      SettingCode = a.SettingCode,
                                                      SettingValue = a.SettingValue,
                                                      Prefix = a.Prefix,
                                                      Size = a.Size,
                                                      DontIncludeDirects = a.DontIncludeDirects,
                                                      IsDocumentFilingByEmailEnabled = a.IsDocumentFilingByEmailEnabled,
                                                  }
         );

            return result;
        }

        public TenantSettingPM GetTenantSettingsByCode(string objectTableName, string settingCode, int tenant)
        {
            TenantSettingPM result = (from a in repository.context.TenantSettings
                                      where a.Tenant == tenant && a.ObjectTable.Name == objectTableName && a.SettingCode == settingCode
                                      select new TenantSettingPM()
                                      {
                                          Id = a.Id,
                                          ObjectTableId = a.ObjectTableId,
                                          Tenant = a.Tenant,
                                          SettingCode = a.SettingCode,
                                          SettingValue = a.SettingValue,
                                          Prefix = a.Prefix,
                                          Size = a.Size,
                                          DontIncludeDirects = a.DontIncludeDirects,
                                          IsDocumentFilingByEmailEnabled = a.IsDocumentFilingByEmailEnabled,
                                      }
         ).FirstOrDefault();

            return result;
        }


    }
}