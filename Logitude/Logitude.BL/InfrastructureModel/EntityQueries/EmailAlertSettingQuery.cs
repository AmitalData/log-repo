using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class EmailAlertSettingQuery
    {
        EmailAlertSettingRepository repository;

        public EmailAlertSettingQuery()
        {
            repository = new EmailAlertSettingRepository();
        }

        public EmailAlertSettingQuery(int tenant)
        {
            repository = new EmailAlertSettingRepository(tenant);
        }

        public EmailAlertSettingQuery(EmailAlertSettingRepository emailAlertSettingRepository)
        {
            repository = emailAlertSettingRepository;
        }

        public EmailAlertSettingPM GetSinglePM(string id, int tenant)
        {
            EmailAlertSettingPM entity;
            entity = (from a in repository.webFreightContext.EmailAlertSettings
                      where a.Tenant == tenant && a.Id == id
                      select new EmailAlertSettingPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          Code = a.Code,
                          Description = a.Description,
                          ObjectTableId = a.ObjectTableId,
                          InActive = a.InActive,
                          SettingLevelCode = a.SettingLevelCode,
                          To = a.To,
                          IndexOrder = a.IndexOrder,
                      }).FirstOrDefault();

            return entity;

        }

      

        public IQueryable<EmailAlertSettingList> GetIQueryableEntityList(IQueryable<EmailAlertSetting> iQueryable)
        {
            IQueryable<EmailAlertSettingList> result = from emailAlertSetting in iQueryable
                                                       select new EmailAlertSettingList()
                                                       {
                                                           Id = emailAlertSetting.Id,


                                                           Tenant = emailAlertSetting.Tenant,
                                                           Code = emailAlertSetting.Code,
                                                           Description = emailAlertSetting.Description,
                                                           ObjectTableId = emailAlertSetting.ObjectTableId,
                                                           InActive = emailAlertSetting.InActive,
                                                           SettingLevelCode = emailAlertSetting.SettingLevelCode,
                                                           To = emailAlertSetting.To,
                                                           IndexOrder = emailAlertSetting.IndexOrder,
                                                       };
            return result;
        }

        public EmailAlertSetting GetFirstEmailAlertSettingForTenant(int tenant)
        {
            return (from a in repository.webFreightContext.EmailAlertSettings
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        //  EmailAlertSettingService

        public IQueryable<EmailAlertSettingPM> GetEmailAlertSettingPMsByTenant(int tenant)
        {
            IQueryable<EmailAlertSettingPM> EmailAlertSetting = from a in repository.webFreightContext.EmailAlertSettings
                                                                where a.Tenant == tenant
                                                                select new EmailAlertSettingPM()
                                                                {
                                                                    Id = a.Id,
                                                                    Tenant = a.Tenant,
                                                                    Code = a.Code,
                                                                    Description = a.Description,
                                                                    ObjectTableId = a.ObjectTableId,
                                                                    InActive = a.InActive,
                                                                    SettingLevelCode = a.SettingLevelCode,
                                                                    To = a.To,
                                                                    IndexOrder = a.IndexOrder,
                                                                };
            return EmailAlertSetting;
        }
    }
}