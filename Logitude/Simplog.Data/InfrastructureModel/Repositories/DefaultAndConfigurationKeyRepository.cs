using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DefaultAndConfigurationKeyRepository : IRepository<DefaultAndConfigurationKeys>
    {

        IWebFreightContext webFreightContext;
        public DefaultAndConfigurationKeyRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public DefaultAndConfigurationKeyRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DefaultAndConfigurationKeyRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public DefaultAndConfigurationKeys GetSingleDefaultAndConfigurationKey(string id)
        {
            return (from a in context.DefaultAndConfigurationKeys
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(DefaultAndConfigurationKeys entity)
        {
            context.DefaultAndConfigurationKeys.Add(entity);
        }

        public void Remove(DefaultAndConfigurationKeys entity)
        {
            context.DefaultAndConfigurationKeys.Attach(entity);
            context.DefaultAndConfigurationKeys.Remove(entity);
        }

        public void Update(DefaultAndConfigurationKeys entity)
        {
            context.DefaultAndConfigurationKeys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DefaultAndConfigurationKeys> All()
        {
            return context.DefaultAndConfigurationKeys.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DefaultAndConfigurationKeys> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DefaultAndConfigurationKeys GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}