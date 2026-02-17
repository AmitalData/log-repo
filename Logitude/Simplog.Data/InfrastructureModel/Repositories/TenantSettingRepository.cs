using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TenantSettingRepository : IRepository<TenantSetting>
    {
        IWebFreightContext webFreightContext;
        public TenantSettingRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public TenantSettingRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public TenantSettingRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

      
        public TenantSetting GetSingleTenantSetting(string id, int tenant)
        {
            return context.TenantSettings.Where(d => d.Tenant == tenant && d.Id == id).FirstOrDefault();
        }





        public void Add(TenantSetting entity)
        {
            this.context.TenantSettings.Add(entity);
        }

        public void Remove(TenantSetting entity)
        {
            this.context.TenantSettings.Remove(entity);
        }

        public void Update(TenantSetting entity)
        {
            this.context.TenantSettings.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<TenantSetting> All()
        {
            return this.context.TenantSettings.ToList();
        }

        public IWebFreightContext context
        {
            get { return this.webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<TenantSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<TenantSetting> GetTenantSettingsByTenant(int tenant)
        {
            return context.TenantSettings.Where(d => d.Tenant == tenant).ToList();
        }

    }
}