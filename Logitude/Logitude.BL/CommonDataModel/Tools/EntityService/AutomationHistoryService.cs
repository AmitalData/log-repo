using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AutomationHistoryService
    {
        bool isNewEntity;
        private int tenant;
        public AutomationHistory Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AutomationHistoryPM entityPm;
        private ICommonDataContext objectContext;
        private AutomationHistoryRepository entityRepository;
        public AutomationHistoryService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AutomationHistoryRepository(objectContext);
        }

        public void Create(AutomationHistoryPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
        
            this.Poco = new AutomationHistory();
            this.entityPm.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
         
            AutomationHistoryMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(AutomationHistoryPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleAutomationHistory(entityPM.Version, entityPM.AutomationsId, entityPm.Tenant);


            AutomationHistoryMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

    }
}