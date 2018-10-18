using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerFieldsUpdateSettingRepository : IRepository<CustomerFieldsUpdateSetting>
    {
        ICommonDataContext commonDataContext;

        public CustomerFieldsUpdateSettingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerFieldsUpdateSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public CustomerFieldsUpdateSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerFieldsUpdateSetting GetSingleCustomerFieldsUpdateSetting(string id, int tenant)
        {
            return (from a in this.context.CustomerFieldsUpdateSettings.Include("ObjectField")
                    where a.Id == id
                    select a).FirstOrDefault();
        }


        public IQueryable<CustomerFieldsUpdateSetting> GetCustomerFieldsUpdateSettings( int tenant)
        {
            return (from a in this.context.CustomerFieldsUpdateSettings
                    where a.Tenant == tenant
                    select a);
        }






        public void Add(CustomerFieldsUpdateSetting entity)
        {
            this.context.CustomerFieldsUpdateSettings.Add(entity);
        }

        public void Remove(CustomerFieldsUpdateSetting entity)
        {

            this.context.CustomerFieldsUpdateSettings.Attach(entity);

            this.context.CustomerFieldsUpdateSettings.Remove(entity);
        }

        public void Update(CustomerFieldsUpdateSetting entity)
        {
            this.context.CustomerFieldsUpdateSettings.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<CustomerFieldsUpdateSetting> All()
        {
            return this.context.CustomerFieldsUpdateSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<CustomerFieldsUpdateSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerFieldsUpdateSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}