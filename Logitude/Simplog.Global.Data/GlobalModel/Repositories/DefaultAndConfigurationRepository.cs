using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class DefaultAndConfigurationRepository : IRepository<DefaultAndConfiguration>
    {

        IGlobalContext globalContext;
        public DefaultAndConfigurationRepository(IGlobalContext context)
        {
            globalContext = context;

        }
        public DefaultAndConfigurationRepository()
        {
            globalContext = new GlobalContext();
        }
        public DefaultAndConfigurationRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext(tenant);
        }
        public DefaultAndConfiguration GetSingleQueueMessage(string id)
        {
            return (from a in context.DefaultAndConfigurations
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(DefaultAndConfiguration entity)
        {
            context.DefaultAndConfigurations.Add(entity);
        }


        public DefaultAndConfiguration GetSingleDefaultAndConfiguration(string id)
        {
            return (from a in context.DefaultAndConfigurations
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Remove(DefaultAndConfiguration entity)
        {
            context.DefaultAndConfigurations.Attach(entity);
            context.DefaultAndConfigurations.Remove(entity);
        }

        public void Update(DefaultAndConfiguration entity)
        {
            context.DefaultAndConfigurations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DefaultAndConfiguration> All()
        {
            return context.DefaultAndConfigurations.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DefaultAndConfiguration> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DefaultAndConfiguration GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        List<DefaultAndConfiguration> IRepository<DefaultAndConfiguration>.All()
        {
            throw new NotImplementedException();
        }

        List<DefaultAndConfiguration> IRepository<DefaultAndConfiguration>.GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        DefaultAndConfiguration IRepository<DefaultAndConfiguration>.GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DefaultAndConfiguration GetSingleDefaultAndConfiguration(string id, int tenant)
        {

            return (from a in context.DefaultAndConfigurations
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<DefaultAndConfiguration> GetDefaultAndConfigurations(int tenant) =>
            context.DefaultAndConfigurations.Where(x => x.Tenant == tenant && x.Is_Active == true);

        public IQueryable<DefaultAndConfiguration> GetWithInheritance(int tenant) =>
            context.DefaultAndConfigurations.Where(x => x.Is_Active == true && (x.Tenant == tenant || (x.Tenant == 0 && x.AllowInheritance == true)));
    }
}