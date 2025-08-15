
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TenantLoginPolicyQuery
    {
        
           TenantLoginPolicyRepository repository;



        public TenantLoginPolicyQuery(int tenant)
        {
            repository = new TenantLoginPolicyRepository(tenant);
        }

        public TenantLoginPolicyQuery(TenantLoginPolicyRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TenantLoginPolicyList> GetIQueryableEntityList(IQueryable<TenantLoginPolicy> iQueryable)
        {
            IQueryable<TenantLoginPolicyList> result = from a in iQueryable
                                                       select new TenantLoginPolicyList()
                                                       {

                                                           Tenant = a.Tenant,
                                                           IsEnabledForSpecificUsers = a.IsEnabledForSpecificUsers,
                                                           LoginPolicyCode = a.LoginPolicyCode,
                                                           TwoFactorInternalIPs = a.TwoFactorInternalIPs,
                                                           KeepUserLoggedIn = a.KeepUserLoggedIn,
                                                           ExcludeInternalIPs = a.ExcludeInternalIPs,
                                                           AllowedIPs = a.AllowedIPs,
                                                           SessionTimeout = a.SessionTimeout,
                                                       };


            return result;
        }

        public TenantLoginPolicyPM GetSinglePM(int tenant)
        {
            TenantLoginPolicyPM entity = (from a in repository.context.TenantLoginPolicies
                                            where a.Tenant == tenant
                                      
                                           select new TenantLoginPolicyPM()
                                            {
                                               Tenant = a.Tenant,
                                               IsEnabledForSpecificUsers = a.IsEnabledForSpecificUsers,
                                               LoginPolicyCode = a.LoginPolicyCode,
                                               TwoFactorInternalIPs = a.TwoFactorInternalIPs,
                                               KeepUserLoggedIn = a.KeepUserLoggedIn,
                                               ExcludeInternalIPs = a.ExcludeInternalIPs,
                                               AllowedIPs = a.AllowedIPs,
                                               SessionTimeout = a.SessionTimeout,
                                           }).FirstOrDefault();
            return entity;
        }

        public TenantLoginPolicyPM GetSinglePM(int tenant , int id)
        {
            TenantLoginPolicyPM entity = (from a in repository.context.TenantLoginPolicies
                                             where a.Tenant == tenant

                                             select new TenantLoginPolicyPM()
                                             {
                                                 Tenant = a.Tenant,
                                                 IsEnabledForSpecificUsers = a.IsEnabledForSpecificUsers,
                                                 LoginPolicyCode = a.LoginPolicyCode,
                                                 TwoFactorInternalIPs = a.TwoFactorInternalIPs,
                                                 KeepUserLoggedIn = a.KeepUserLoggedIn,
                                                 ExcludeInternalIPs = a.ExcludeInternalIPs,
                                                 AllowedIPs = a.AllowedIPs,
                                                 SessionTimeout = a.SessionTimeout,
                                             }).FirstOrDefault();
            return entity;
        }



        

        public IQueryable<TenantLoginPolicyPM> GetTenantLoginPolicyPMsByTenant(int tenant)
        {
            IQueryable<TenantLoginPolicyPM> TenantLoginPolicyPMs = from a in repository.context.TenantLoginPolicies
                                                                       where a.Tenant == tenant
                                                                       select new TenantLoginPolicyPM()
                                                                       {
                                                                           Tenant = a.Tenant,
                                                                           IsEnabledForSpecificUsers = a.IsEnabledForSpecificUsers,
                                                                           LoginPolicyCode = a.LoginPolicyCode,
                                                                           TwoFactorInternalIPs = a.TwoFactorInternalIPs,
                                                                           KeepUserLoggedIn = a.KeepUserLoggedIn,
                                                                           ExcludeInternalIPs = a.ExcludeInternalIPs,
                                                                           AllowedIPs = a.AllowedIPs,
                                                                           SessionTimeout = a.SessionTimeout,
                                                                       };
            return TenantLoginPolicyPMs;
        }

        public IQueryable<TenantLoginPolicyList> GetTenantLoginPolicyListsByTenant(int tenant)
        {
            IQueryable<TenantLoginPolicyList> TenantLoginPolicyLists = from a in repository.context.TenantLoginPolicies
                                                                          where a.Tenant == tenant
                                                                           select new TenantLoginPolicyList()
                                                                           {
                                                                               Tenant = a.Tenant,
                                                                               IsEnabledForSpecificUsers = a.IsEnabledForSpecificUsers,
                                                                               LoginPolicyCode = a.LoginPolicyCode,
                                                                               TwoFactorInternalIPs = a.TwoFactorInternalIPs,
                                                                               KeepUserLoggedIn = a.KeepUserLoggedIn,
                                                                               ExcludeInternalIPs = a.ExcludeInternalIPs,
                                                                               AllowedIPs = a.AllowedIPs,
                                                                               SessionTimeout = a.SessionTimeout,

                                                                           };
            return TenantLoginPolicyLists;
        }



    }
}