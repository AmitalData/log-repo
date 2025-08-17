using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CreditLimitSettingRepository : IRepository<CreditLimitSetting>
    {
        ICommonDataContext commonDataContext;
 
        public CreditLimitSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }
        public CreditLimitSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CreditLimitSetting GetSingleCreditLimitSetting(string id, int tenant)
        {
            return (from a in this.context.CreditLimitSettings where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public void Add(CreditLimitSetting entity)
        {
            this.context.CreditLimitSettings.Add(entity);
        }

        public void Remove(CreditLimitSetting entity)
        {
            this.context.CreditLimitSettings.Attach(entity);
            this.context.CreditLimitSettings.Remove(entity);
        }

        public void Update(CreditLimitSetting entity)
        {
            this.context.CreditLimitSettings.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<CreditLimitSetting> All()
        {
            return this.context.CreditLimitSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<CreditLimitSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CreditLimitSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<CreditLimitSetting> GetCreditLimitSettings(int tenant)
        {
            return (from a in context.CreditLimitSettings where a.Tenant == tenant select a);
        }
    }
}
