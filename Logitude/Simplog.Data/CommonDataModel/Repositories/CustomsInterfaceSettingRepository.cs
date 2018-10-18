using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomsInterfaceSettingRepository : IRepository<CustomsInterfaceSetting>
    {
        ICommonDataContext commonDataContext;

        public CustomsInterfaceSettingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomsInterfaceSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomsInterfaceSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomsInterfaceSetting> GetAllArtimusCustomsInterfaceSettings()
        {
            return (from a in this.context.CustomsInterfaceSettings
                    where a.ImportToUSAInterfaceCode == "ART" && !string.IsNullOrEmpty(a.ArtemusInSettingsId)
                    select a);
        }


        public CustomsInterfaceSetting GetSingleCustomsInterfaceSetting(int id, int otherTenant)
        {
            return (from a in this.context.CustomsInterfaceSettings
                    where a.Tenant == id
                    select a).FirstOrDefault();
        }

        public void Add(CustomsInterfaceSetting entity)
        {
            this.context.CustomsInterfaceSettings.Add(entity);
        }

        public void Remove(CustomsInterfaceSetting entity)
        {
            this.context.CustomsInterfaceSettings.Attach(entity);
            this.context.CustomsInterfaceSettings.Remove(entity);
        }

        public void Update(CustomsInterfaceSetting entity)
        {
            this.context.CustomsInterfaceSettings.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<CustomsInterfaceSetting> All()
        {
            return this.context.CustomsInterfaceSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<CustomsInterfaceSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomsInterfaceSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
        
        public IQueryable<CustomsInterfaceSetting> GetCustomsInterfaceSettings(int tenant)
        {
            return (from a in context.CustomsInterfaceSettings select a);
        }
    }
}