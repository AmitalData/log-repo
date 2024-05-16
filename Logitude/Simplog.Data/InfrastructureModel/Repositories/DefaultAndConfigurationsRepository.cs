using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DefaultAndConfigurationsRepository : IRepository<DefaultAndConfiguration>
    {

        IWebFreightContext webFreightContext;
        public DefaultAndConfigurationsRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public DefaultAndConfigurationsRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DefaultAndConfigurationsRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public DefaultAndConfiguration GetSingleQueueMessage(long id)
        {
            return (from a in context.DefaultAndConfigurations
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(DefaultAndConfiguration entity)
        {
            context.DefaultAndConfigurations.Add(entity);
        }


        public DefaultAndConfiguration GetSingleDefaultAndConfigurations(string id)
        {
            long? longId = null;
            if (id != null)
            {
                longId = long.Parse(id);
            }

            return (from a in context.DefaultAndConfigurations
                    where a.Id == longId
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

        public IWebFreightContext context
        {
            get { return webFreightContext; }
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
    }
}