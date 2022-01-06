using System;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class ApiCredintialsQuery
    {
        private ApiCredintialsRepository repository;
        public ApiCredintialsQuery()
        {
            repository = new ApiCredintialsRepository();
        }
        public ApiCredintialsQuery(int tenant)
        {
            repository = new ApiCredintialsRepository();
        }
        public ApiCredintialsQuery(ApiCredintialsRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<ApiCredintialsPM> GetApiCredintialsPMs(int tenant)
        {
            return (from a in repository.context.ApiCredintials
                    where a.Tenant == tenant
                    select new ApiCredintialsPM()
                    {
                        Id = a.Id,
                        AllowedIPs = a.AllowedIPs,
                        CreateDate = a.CreateDate,
                        CreatedBy = a.CreatedBy,
                        HashedPrimaryAccessKey = a.HashedPrimaryAccessKey,
                        HashedSeconderyAccessKey = a.HashedSeconderyAccessKey,
                        maskedPrimaryAccessKey = a.maskedPrimaryAccessKey,
                        maskedSeconderyAccessKey = a.maskedSeconderyAccessKey,
                        UpdateDate = a.UpdateDate,
                        UpdatedBy = a.UpdatedBy,
                        UsedFor = a.UsedFor,
                        Tenant = a.Tenant,
                        TokenExpirationTime = a.TokenExpirationTime,
                        //ComputingPartnerId = a.ComputingPartnerId,
                    });
        }

        public ApiCredintialsPM GetSinglePM(string id, int tenant)
        {
            ApiCredintialsPM entity = (from a in repository.context.ApiCredintials
                                       where a.Id == id && a.Tenant == tenant
                                       select new ApiCredintialsPM()
                                       {
                                           Id = a.Id,
                                           AllowedIPs = a.AllowedIPs,
                                           CreateDate = a.CreateDate,
                                           CreatedBy = a.CreatedBy,
                                           HashedPrimaryAccessKey = a.HashedPrimaryAccessKey,
                                           HashedSeconderyAccessKey = a.HashedSeconderyAccessKey,
                                           maskedPrimaryAccessKey = a.maskedPrimaryAccessKey,
                                           maskedSeconderyAccessKey = a.maskedSeconderyAccessKey,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedBy = a.UpdatedBy,
                                           UsedFor = a.UsedFor,
                                           Tenant = a.Tenant,
                                           TokenExpirationTime = a.TokenExpirationTime,
                                           //ComputingPartnerId = a.ComputingPartnerId,
                                       }).FirstOrDefault();



            return entity;
        }

        public ApiCredintialsList GetSingleList(int tenant, string id)
        {
            ApiCredintialsList entity = (from a in repository.context.ApiCredintials
                                         where a.Id == id && a.Tenant == tenant
                                         select new ApiCredintialsList()
                                         {
                                             Id = a.Id,
                                             AllowedIPs = a.AllowedIPs,
                                             CreateDate = a.CreateDate,
                                             CreatedBy = a.CreatedBy,
                                             HashedPrimaryAccessKey = a.HashedPrimaryAccessKey,
                                             HashedSeconderyAccessKey = a.HashedSeconderyAccessKey,
                                             maskedPrimaryAccessKey = a.maskedPrimaryAccessKey,
                                             maskedSeconderyAccessKey = a.maskedSeconderyAccessKey,
                                             UpdateDate = a.UpdateDate,
                                             UpdatedBy = a.UpdatedBy,
                                             UsedFor = a.UsedFor,
                                             Tenant = a.Tenant,
                                             TokenExpirationTime = a.TokenExpirationTime,
                                             //ComputingPartnerId = a.ComputingPartnerId,
                                         }).FirstOrDefault();

            return entity;
        }

        public IQueryable<ApiCredintialsList> GetApiCredintialsLists(int tenant)
        {
            return (from a in repository.context.ApiCredintials
                    where a.Tenant == tenant
                    select new ApiCredintialsList()
                    {
                        Id = a.Id,
                        AllowedIPs = a.AllowedIPs,
                        CreateDate = a.CreateDate,
                        CreatedBy = a.CreatedBy,
                        HashedPrimaryAccessKey = a.HashedPrimaryAccessKey,
                        HashedSeconderyAccessKey = a.HashedSeconderyAccessKey,
                        maskedPrimaryAccessKey = a.maskedPrimaryAccessKey,
                        maskedSeconderyAccessKey = a.maskedSeconderyAccessKey,
                        UpdateDate = a.UpdateDate,
                        UpdatedBy = a.UpdatedBy,
                        UsedFor = a.UsedFor,
                        Tenant = a.Tenant,
                        TokenExpirationTime = a.TokenExpirationTime,
                        //ComputingPartnerId = a.ComputingPartnerId,
                    });
        }

        public IQueryable<ApiCredintialsList> GetIQueryableEntityList(IQueryable<ApiCredintials> iQueryable)
        {
            return from a in iQueryable
                   select new ApiCredintialsList()
                   {
                       Id = a.Id,
                       AllowedIPs = a.AllowedIPs,
                       CreateDate = a.CreateDate,
                       CreatedBy = a.CreatedBy,
                       HashedPrimaryAccessKey = a.HashedPrimaryAccessKey,
                       HashedSeconderyAccessKey = a.HashedSeconderyAccessKey,
                       maskedPrimaryAccessKey = a.maskedPrimaryAccessKey,
                       maskedSeconderyAccessKey = a.maskedSeconderyAccessKey,
                       UpdateDate = a.UpdateDate,
                       UpdatedBy = a.UpdatedBy,
                       UsedFor = a.UsedFor,
                       Tenant = a.Tenant,
                       TokenExpirationTime = a.TokenExpirationTime,
                       //ComputingPartnerId = a.ComputingPartnerId,
                   };
        }
    }
}