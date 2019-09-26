using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class LogBoxTenantSettingRepository : IRepository<LogBoxTenantSetting>
    {

        ICommonDataContext commonDataContext;
        public LogBoxTenantSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);  
        }
        

        public LogBoxTenantSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

       

        public IQueryable<LogBoxTenantSetting> GetLogBoxTenantSettings()
        {
            return this.context.LogBoxTenantSettings;
        }

        public LogBoxTenantSetting GetLogBoxTenantSettingId(int id)
        {
            var logBoxTenantSetting = (from a in context.LogBoxTenantSettings
                                       where a.Id == id 
                                 select a).FirstOrDefault(); ;
            return logBoxTenantSetting;
        }


        public LogBoxTenantSetting GetSingleLBTenant(int id)
        {
            return (from record in context.LogBoxTenantSettings where record.Id == id select record).FirstOrDefault();
        }

        public static LogBoxTenantSetting GetSingleLBTenantSetting(int id)
        {

            LogBoxTenantSetting entity;

            ICommonDataContext context = CommonDataContext.GetContext(id);
            entity = (from a in context.LogBoxTenantSettings where a.Id == id select a).FirstOrDefault();

            return entity;
        }

        public void Add(LogBoxTenantSetting entity)
        {
            this.context.LogBoxTenantSettings.Add(entity);
        }

        public void Remove(LogBoxTenantSetting entity)
        {
            this.context.LogBoxTenantSettings.Attach(entity);
            this.context.LogBoxTenantSettings.Remove(entity);
        }

        public void Update(LogBoxTenantSetting entity)
        {
            this.context.LogBoxTenantSettings.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<LogBoxTenantSetting> All()
        {
            return this.context.LogBoxTenantSettings.ToList<LogBoxTenantSetting>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public LogBoxTenantSetting GetSingleLogBoxTenantSetting(int id)
        {
            LogBoxTenantSetting entity;

            ICommonDataContext context = CommonDataContext.GetContext(id);
            entity = (from a in context.LogBoxTenantSettings where a.Id == id select a).FirstOrDefault();

            return entity;
        }

        public List<LogBoxTenantSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public LogBoxTenantSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}