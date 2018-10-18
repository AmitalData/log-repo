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

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AutomationResultEmailRecipientService
    {
        bool isNewEntity;
        private int tenant;
        public AutomationResultEmailRecipient Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AutomationResultEmailRecipientPM entityPm;
        private ICommonDataContext objectContext;
        private AutomationResultEmailRecipientRepository entityRepository;
        public AutomationResultEmailRecipientService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AutomationResultEmailRecipientRepository(objectContext);
        }

        public void Create(AutomationResultEmailRecipientPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("AutomationResultEmailRecipient", tenant).ToString();
            this.Poco = new AutomationResultEmailRecipient();
            this.Poco.Id = this.entityPm.Id;

            AutomationResultEmailRecipientMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(AutomationResultEmailRecipientPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleAutomationResultEmailRecipient(entityPM.Id, entityPm.Tenant);


            AutomationResultEmailRecipientMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

    }
}