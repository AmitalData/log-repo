
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
    public class TwoFactorAuthenticationDeviceQuery
    {
        
           TwoFactorAuthenticationDeviceRepository repository;



        public TwoFactorAuthenticationDeviceQuery(int tenant)
        {
            repository = new TwoFactorAuthenticationDeviceRepository(tenant);
        }

        public TwoFactorAuthenticationDeviceQuery(TwoFactorAuthenticationDeviceRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TwoFactorAuthenticationDeviceList> GetIQueryableEntityList(IQueryable<TwoFactorAuthenticationDevice> iQueryable)
        {
            IQueryable<TwoFactorAuthenticationDeviceList> result = from a in iQueryable
                                                         select new TwoFactorAuthenticationDeviceList()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             //AuthenticationCode = a.AuthenticationCode,
                                                             CodeExpirationDate = a.CodeExpirationDate,
                                                             DeviceDescription = a.DeviceDescription,
                                                             InActive = a.InActive,
                                                             UpdateDate = a.UpdateDate,
                                                             LastLoginDate = a.LastLoginDate,
                                                             CreateDate = a.CreateDate,
                                                             LastLoginIP = a.LastLoginIP,
                                                             //TwoFactorkey = a.TwoFactorkey,
                                                             UserId = a.UserId,
                        
                                                         };


            return result;
        }

        public TwoFactorAuthenticationDevicePM GetSinglePM(string twoFactorkey, int tenant)
        {
            TwoFactorAuthenticationDevicePM entity = (from a in repository.context.TwoFactorAuthenticationDevices
                                            where a.Tenant == tenant
                                            && a.TwoFactorkey == twoFactorkey
                                           select new TwoFactorAuthenticationDevicePM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                //AuthenticationCode = a.AuthenticationCode,
                                                CodeExpirationDate = a.CodeExpirationDate,
                                                DeviceDescription = a.DeviceDescription,
                                                InActive = a.InActive,
                                                UpdateDate = a.UpdateDate,
                                                LastLoginDate = a.LastLoginDate,
                                                CreateDate = a.CreateDate,
                                                LastLoginIP = a.LastLoginIP,
                                                //TwoFactorkey = a.TwoFactorkey,
                                                UserId = a.UserId,
                                            }).FirstOrDefault();
            return entity;
        }

        public IQueryable<TwoFactorAuthenticationDevicePM> GetTwoFactorAuthenticationDevicePMsByTenant(int tenant)
        {
            IQueryable<TwoFactorAuthenticationDevicePM> TwoFactorAuthenticationDevicePMs = from a in repository.context.TwoFactorAuthenticationDevices
                                                                       where a.Tenant == tenant
                                                                       select new TwoFactorAuthenticationDevicePM()
                                                                       {   Id = a.Id,
                                                                           Tenant = a.Tenant,
                                                                           //AuthenticationCode = a.AuthenticationCode,
                                                                           CodeExpirationDate = a.CodeExpirationDate,
                                                                           DeviceDescription = a.DeviceDescription,
                                                                           InActive = a.InActive,
                                                                           UpdateDate = a.UpdateDate,
                                                                           LastLoginDate = a.LastLoginDate,
                                                                           CreateDate = a.CreateDate,
                                                                           LastLoginIP = a.LastLoginIP,
                                                                           //TwoFactorkey = a.TwoFactorkey,
                                                                           UserId = a.UserId,
                                                                       };
            return TwoFactorAuthenticationDevicePMs;
        }


        public IQueryable<TwoFactorAuthenticationDevicePM> GetVerifiedTwoFactorAuthenticationDevicePMsByUserId(string userId, int tenant)
        {
            IQueryable<TwoFactorAuthenticationDevicePM> TwoFactorAuthenticationDevicePMs = from a in repository.context.TwoFactorAuthenticationDevices
                                                                                           where a.Tenant == tenant && a.UserId == userId && a.IsVerified
                                                                                           select new TwoFactorAuthenticationDevicePM()
                                                                                           {
                                                                                               Id = a.Id,
                                                                                               Tenant = a.Tenant,
                                                                                               //AuthenticationCode = a.AuthenticationCode,
                                                                                               CodeExpirationDate = a.CodeExpirationDate,
                                                                                               DeviceDescription = a.DeviceDescription,
                                                                                               InActive = a.InActive,
                                                                                               UpdateDate = a.UpdateDate,
                                                                                               LastLoginDate = a.LastLoginDate,
                                                                                               CreateDate = a.CreateDate,
                                                                                               LastLoginIP = a.LastLoginIP,
                                                                                               //TwoFactorkey = a.TwoFactorkey,
                                                                                               UserId = a.UserId,
                                                                                           };
            return TwoFactorAuthenticationDevicePMs;
        }

        public IQueryable<TwoFactorAuthenticationDeviceList> GetTwoFactorAuthenticationDeviceListsByTenant(int tenant)
        {
            IQueryable<TwoFactorAuthenticationDeviceList> TwoFactorAuthenticationDeviceLists = from a in repository.context.TwoFactorAuthenticationDevices
                                                                                               where a.Tenant == tenant
                                                                                               select new TwoFactorAuthenticationDeviceList()
                                                                                               {
                                                                                                   Id = a.Id,
                                                                                                   Tenant = a.Tenant,
                                                                                                   //AuthenticationCode = a.AuthenticationCode,
                                                                                                   CodeExpirationDate = a.CodeExpirationDate,
                                                                                                   DeviceDescription = a.DeviceDescription,
                                                                                                   InActive = a.InActive,
                                                                                                   UpdateDate = a.UpdateDate,
                                                                                                   LastLoginDate = a.LastLoginDate,
                                                                                                   CreateDate = a.CreateDate,
                                                                                                   LastLoginIP = a.LastLoginIP,
                                                                                                   //TwoFactorkey = a.TwoFactorkey,
                                                                                                   UserId = a.UserId,

                                                                                               };
            return TwoFactorAuthenticationDeviceLists;
        }



    }
}