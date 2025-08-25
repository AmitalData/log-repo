using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UserPermittedProductRepository:IRepository<UserPermittedProduct>
    {
        ICommonDataContext commonDataContext;

        public UserPermittedProductRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public UserPermittedProductRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<UserPermittedProduct> GetUserPermittedProducts(int tenant)
        {
            return (from record in context.UserPermittedProducts.Include("User") where record.Tenant == tenant select record);
        }

        public IQueryable<UserPermittedProduct> GetUserPermittedProductesByUserId(string userId, int tenant)
        {
            return (from record in context.UserPermittedProducts.Include("User") where record.UserId == userId && record.Tenant == tenant select record);
        }

        public UserPermittedProduct GetSingleUserPermittedProduct(string id, int tenant)
        {
            return (from record in context.UserPermittedProducts.Include("User") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public UserPermittedProduct GetUserPermittedProductByUserAndProduct(string userId, string productTypeCode, int tenant)
        {
            return (from record in context.UserPermittedProducts.Include("User") where record.UserId == userId && record.Tenant == tenant && record.ProductTypeCode == productTypeCode select record).FirstOrDefault();
        }

        public void Add(UserPermittedProduct entity)
        {
            context.UserPermittedProducts.Add(entity);
        }

        public void Remove(UserPermittedProduct entity)
        {
            try
            {
                context.UserPermittedProducts.Attach(entity);
            }
            catch { }
            context.UserPermittedProducts.Remove(entity);
        }

        public void Update(UserPermittedProduct entity)
        {
            context.UserPermittedProducts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UserPermittedProduct> All()
        {
            return context.UserPermittedProducts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<UserPermittedProduct> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public UserPermittedProduct GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}