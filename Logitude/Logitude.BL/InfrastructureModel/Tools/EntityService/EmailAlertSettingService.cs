using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class EmailAlertSettingService
    {

        bool isNewEntity;
        private int tenant;
        public EmailAlertSetting Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
    
        private EmailAlertSettingPM entityPm;
        private IWebFreightContext objectContext;
        private EmailAlertSettingRepository entityRepository;

        public EmailAlertSettingService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new EmailAlertSettingRepository(objectContext);
        }

        public void Create(EmailAlertSettingPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("EmailAlertSetting", tenant).ToString();
            this.Poco = new EmailAlertSetting();
            this.Poco.Id = this.entityPm.Id;

            //DepartmentValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    EmailAlertSettingtTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            EmailAlertSettingMapping.MappingEmailAlertSetting(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(EmailAlertSettingPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleEmailAlertSetting(entityPM.Id, entityPm.Tenant);

            string entityName = "EmailAlertSetting" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "EmailAlertSettingPM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //EmailAlertSettingValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    EmailAlertSettingTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            EmailAlertSettingMapping.MappingEmailAlertSetting(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}