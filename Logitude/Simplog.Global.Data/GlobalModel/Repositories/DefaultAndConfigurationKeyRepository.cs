using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class DefaultAndConfigurationKeyRepository : IRepository<DefaultAndConfigurationKey>
    {

        IGlobalContext globalContext;
        public DefaultAndConfigurationKeyRepository(IGlobalContext context)
        {
            globalContext = context;

        }
        public DefaultAndConfigurationKeyRepository()
        {
            globalContext = new GlobalContext();
        }
        public DefaultAndConfigurationKeyRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext(tenant);
        }
        public DefaultAndConfigurationKey GetSingleDefaultAndConfigurationKey(string SetKey, int? tenant = null)
        {
            return (from a in context.DefaultAndConfigurationKeys
                    where a.SetKey == SetKey
                    select a).FirstOrDefault();
        }

        public void Add(DefaultAndConfigurationKey entity)
        {
            context.DefaultAndConfigurationKeys.Add(entity);
        }

        public void Remove(DefaultAndConfigurationKey entity)
        {
            context.DefaultAndConfigurationKeys.Attach(entity);
            context.DefaultAndConfigurationKeys.Remove(entity);
        }

        public void Update(DefaultAndConfigurationKey entity)
        {
            context.DefaultAndConfigurationKeys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DefaultAndConfigurationKey> All()
        {
            return context.DefaultAndConfigurationKeys.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DefaultAndConfigurationKey> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DefaultAndConfigurationKey GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<DefaultAndConfigurationKey> GetDefaultAndConfigurationKeys(int tenant) =>
            context.DefaultAndConfigurationKeys.AsQueryable();

        public DefaultAndConfigurationKey GetSingleDefaultAndConfigurationKey(int tenant1, string setkey, int tenant)
        {
            throw new NotImplementedException();
        }
    }
}