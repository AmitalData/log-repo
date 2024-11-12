using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class UserPermittedProductQuery
    {
        UserPermittedProductRepository repository;

        public UserPermittedProductQuery()
        {
            repository = new UserPermittedProductRepository(); 
        }

        public UserPermittedProductQuery(int tenant)
        {
            repository = new UserPermittedProductRepository(tenant);
        }

        public UserPermittedProductQuery(UserPermittedProductRepository UserPermittedProductRepository)
        {
            repository = UserPermittedProductRepository;
        }

        public UserPermittedProductPM GetSinglePM(string id, int tenant)
        {
            UserPermittedProductPM UserPermittedProduct = (from a in repository.context.UserPermittedProducts
                                                         where a.Id == id && a.Tenant == tenant
                                                         select new UserPermittedProductPM()
                                                         {
                                                             Id = a.Id,
                                                             Tenant = a.Tenant,
                                                             ProductTypeCode = a.ProductTypeCode,
                                                             UserId = a.UserId,
                                                         }).FirstOrDefault();
            return UserPermittedProduct;
        }

        public IQueryable<UserPermittedProductPM> GetUserPermittedProductPMsByTenant(int tenant)
        {
            IQueryable<UserPermittedProductPM> UserPermittedProducts = from a in repository.context.UserPermittedProducts
                                                     where a.Tenant == tenant
                                                     select new UserPermittedProductPM()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         ProductTypeCode = a.ProductTypeCode,
                                                         UserId = a.UserId,
                                                     };
            return UserPermittedProducts;
        }

        public IQueryable<UserPermittedProductPM> GetContactFromUserPermittedProductPMsByUserId(string id, int tenant)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" before GetContactFromUserPermittedProductPMsByUserId  repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" GetContactFromUserPermittedProductPMsByUserId  id:" + id + ",tenant:" + tenant);

            IQueryable<UserPermittedProductPM> UserPermittedProducts = (from a in repository.context.UserPermittedProducts.Include("Contact")
                                      where a.UserId == id
                                      select new UserPermittedProductPM()
                                      {
                                          Id = a.Id,
                                          Tenant = a.Tenant,
                                          ProductTypeCode = a.ProductTypeCode,
                                          UserId = a.UserId,
                                      });
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" after GetContactFromUserPermittedProductPMsByUserId  repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("  GetContactFromUserPermittedProductPMsByUserId UserPermittedProducts.count()" + UserPermittedProducts?.Count());

            return UserPermittedProducts;
        }
    }
}