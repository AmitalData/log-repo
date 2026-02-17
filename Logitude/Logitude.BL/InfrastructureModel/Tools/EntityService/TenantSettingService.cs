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
    public class TenantSettingService
    {
        bool isNewEntity;
        private int tenant;
        public TenantSetting Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TenantSettingPM entityPM;
        private IWebFreightContext objectContext;
        private TenantSettingRepository entityRepository;
        public TenantSettingService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TenantSettingRepository(objectContext);
        }

        public void Create(TenantSettingPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("TenantSetting", tenant).ToString();
            this.Poco = new TenantSetting();
            this.Poco.Id = this.entityPM.Id;

            TenantSettingValidating.Validate(theEntityPm);
            TenantSettingTracing.Trace(theEntityPm, Poco, isNewEntity);
            TenantSettingMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(TenantSettingPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTenantSetting(theEntityPm.Id, theEntityPm.Tenant);

            TenantSettingValidating.Validate(theEntityPm);
            TenantSettingTracing.Trace(theEntityPm, Poco, isNewEntity);
            TenantSettingMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}