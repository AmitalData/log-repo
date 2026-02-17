using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AccountingSettingRepository : IRepository<AccountingSetting>
    {
        ICommonDataContext commonDataContext;

        public AccountingSettingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AccountingSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public AccountingSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public AccountingSetting GetSingleAccountSetting(int id)
        {
            return (from a in this.context.AccountingSettings
                   where a.Id == id
                   select a).FirstOrDefault();
        }

        public void Add(AccountingSetting entity)
        {
            this.context.AccountingSettings.Add(entity);
        }

        public void Remove(AccountingSetting entity)
        {

            this.context.AccountingSettings.Attach(entity);

            this.context.AccountingSettings.Remove(entity);
        }

        public void Update(AccountingSetting entity)
        {
            this.context.AccountingSettings.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<AccountingSetting> All()
        {
            return this.context.AccountingSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<AccountingSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AccountingSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AccountingSetting GetSingleAccountingSetting(int id)
        {
            return (from a in context.AccountingSettings where a.Id == id select a).FirstOrDefault();   
        }

        public IQueryable<AccountingSetting> GetAccountingSettings()
        {
            return (from a in context.AccountingSettings select a);
        }
    }
}