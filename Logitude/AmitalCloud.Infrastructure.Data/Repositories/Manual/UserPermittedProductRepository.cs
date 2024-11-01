using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class UserPermittedProductRepository:Repository<UserPermittedProduct>
    {
        IAmitalCloudContext currentContext;
        public UserPermittedProductRepository(IAmitalCloudContext context):base(context)
        {
            currentContext = context;
        }
        public UserPermittedProductRepository() :this(AmitalCloudContext.GetContext(0))
        {
        }
        public UserPermittedProductRepository(int tenant) : this(AmitalCloudContext.GetContext(tenant))
        {
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
        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }
    }
}