using Logitude.Server.Tools.Counters;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class LogitudeOceanInsightsRequestService
    {
        bool isNewEntity;
        private int tenant;
        public LogitudeOceanInsightsRequest Poco { get; set; }
        private IShipmentsContext objectContext;
        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private LogitudeOceanInsightsRequestPM entityPM;
        private LogitudeOceanInsightsRequestRepository entityRepository;
        public LogitudeOceanInsightsRequestService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.entityRepository = new LogitudeOceanInsightsRequestRepository(objectContext);
        }

        public void Create(LogitudeOceanInsightsRequestPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("LogitudeOceanInsightsRequest", tenant).ToString();
            this.Poco = new LogitudeOceanInsightsRequest();
            this.Poco.Id = this.entityPM.Id;
            LogitudeOceanInsightsRequestMapping.MapLogitudeOceanInsightsRequest(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(LogitudeOceanInsightsRequestPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleLogitudeOceanInsightsRequest(theEntityPm.Id, entityPM.Tenant);
            LogitudeOceanInsightsRequestMapping.MapLogitudeOceanInsightsRequest(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
