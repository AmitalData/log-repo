using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DefaultAndConfigurationKeyRepository : IRepository<DefaultAndConfigurationKey>
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
        public DefaultAndConfigurationKey GetSingleDefaultAndConfigurationKey(string SetKey, int? tenant = null)
        {
            return (from a in context.DefaultAndConfigurationKey
                    where a.SetKey == SetKey
                    select a).FirstOrDefault();
        }

        public void Add(DefaultAndConfigurationKey entity)
        {
            context.DefaultAndConfigurationKey.Add(entity);
        }

        public void Remove(DefaultAndConfigurationKey entity)
        {
            context.DefaultAndConfigurationKey.Attach(entity);
            context.DefaultAndConfigurationKey.Remove(entity);
        }

        public void Update(DefaultAndConfigurationKey entity)
        {
            context.DefaultAndConfigurationKey.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DefaultAndConfigurationKey> All()
        {
            return context.DefaultAndConfigurationKey.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
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
            context.DefaultAndConfigurationKey.AsQueryable();

        public DefaultAndConfigurationKey GetSingleDefaultAndConfigurationKey(int tenant1, string setkey, int tenant)
        {
            throw new NotImplementedException();
        }
    }
}