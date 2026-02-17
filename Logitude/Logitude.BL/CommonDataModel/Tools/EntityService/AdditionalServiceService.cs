using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AdditionalServiceService
    {
        bool isNewEntity;
        private int tenant;
        public AdditionalService Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AdditionalServicePM entityPm;
        private ICommonDataContext objectContext;
        private AdditionalServiceRepository entityRepository;

        public AdditionalServiceService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AdditionalServiceRepository(objectContext);
        }

        public void Create(AdditionalServicePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("AdditionalService", tenant).ToString();
            this.Poco = new AdditionalService();
            this.Poco.Id = this.entityPm.Id;

            AdditionalServiceTracing.Trace(entityPM, Poco, isNewEntity);
            AdditionalServiceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "AdditionalService");

        }

        public void Update(AdditionalServicePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleAdditionalService(entityPM.Id, entityPm.Tenant);

            AdditionalServiceTracing.Trace(entityPM, Poco, isNewEntity);
            AdditionalServiceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "AdditionalService");

        }
    }
}
