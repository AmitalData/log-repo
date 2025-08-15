using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DWHSettingRepository : IRepository<DWHSetting>
    {
        ICommonDataContext commonDataContext;



        public DWHSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public DWHSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DWHSetting GetSingleDWHSetting(int tenant)
        {
            return (from a in this.context.DWHSettings
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(DWHSetting entity)
        {
            this.context.DWHSettings.Add(entity);
        }

        public void Remove(DWHSetting entity)
        {

            this.context.DWHSettings.Attach(entity);

            this.context.DWHSettings.Remove(entity);
        }

        public void Update(DWHSetting entity)
        {
            this.context.DWHSettings.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<DWHSetting> All()
        {
            return this.context.DWHSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public bool IsParentTenant(int tenant)
        {
            return (from a in this.context.DWHSettings
                    where a.Tenant == tenant
                    select a.IsParentTenant).FirstOrDefault();
        }

        public List<int> GetTenantNumbersByParentTenant(int tenant)
        {
            return (from a in this.context.DWHSettings
                    where a.ParentTenant == tenant
                    select a.Tenant).ToList();
        }


        public List<DWHSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DWHSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}