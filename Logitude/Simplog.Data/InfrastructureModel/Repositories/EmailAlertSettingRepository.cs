using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
namespace Simplog.Data.InfrastructureModel.Repositories
{
 public   class EmailAlertSettingRepository : IRepository<EmailAlertSetting>
    {

    public    IWebFreightContext webFreightContext;

        public EmailAlertSettingRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public EmailAlertSettingRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public EmailAlertSettingRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }




        public EmailAlertSetting GetSingleEmailAlertSettingByCodeAndTenant(string code, int tenant)
        {
            EmailAlertSetting entity = this.webFreightContext.EmailAlertSettings.Where(d => d.Code == code && d.Tenant == tenant).FirstOrDefault();
            return entity;
        }


        public EmailAlertSetting GetSingleEmailAlertSetting(string id, int tenant)
        {

            EmailAlertSetting entity = this.webFreightContext.EmailAlertSettings.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


            return entity;
  
        }
        public EmailAlertSetting GetSingleEmailAlertSettingByCodeObjectTableName(string code, string objectTableName, int tenant)
        {

            var emailAlertSetting = (from a in webFreightContext.EmailAlertSettings.Include("ObjectTable")
                                     where a.Tenant == tenant && a.Code == code && a.ObjectTable.Name == objectTableName && a.InActive == false
                                     select a).FirstOrDefault();
            return emailAlertSetting;

        }

        public IQueryable<EmailAlertSetting> GetEmailAlertSettingsByTenant(int tenant)
        {
            IQueryable<EmailAlertSetting> emailAlertSettings = from a in webFreightContext.EmailAlertSettings
                                                               where a.Tenant == tenant
                                                               select a;
            return emailAlertSettings;

        }
        public IQueryable<EmailAlertSetting>GetEmailAlertSettingsByObjectTable(string objectTableName,int tenant)
        {
            IQueryable<EmailAlertSetting> emailAlertSettings = from a in webFreightContext.EmailAlertSettings.Include("ObjectTable")
                                                               where a.Tenant == tenant && a.ObjectTable.Name == objectTableName && a.InActive == false
                                                               select a;
            return emailAlertSettings;

        }


        public IQueryable<EmailAlertSetting> GetEmailAlertSettings(int tenant)
        {
            return (from record in webFreightContext.EmailAlertSettings where record.Tenant == tenant select record);
        }


        public void Add(EmailAlertSetting entity)
        {
            webFreightContext.EmailAlertSettings.Add(entity);
        }

        public void Remove(EmailAlertSetting entity)
        {
            webFreightContext.EmailAlertSettings.Attach(entity);
            webFreightContext.EmailAlertSettings.Remove(entity);
        }


        public void Update(EmailAlertSetting entity)
        {
            webFreightContext.EmailAlertSettings.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<EmailAlertSetting> All()
        {
            return webFreightContext.EmailAlertSettings.ToList();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<EmailAlertSetting> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EmailAlertSetting GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
