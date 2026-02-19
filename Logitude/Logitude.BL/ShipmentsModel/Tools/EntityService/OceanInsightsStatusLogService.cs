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
    public class OceanInsightsStatusLogService
    {
        bool isNewEntity;
        private int tenant;
        public OceanInsightsStatusLog Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private OceanInsightsStatusLogPM entityPM;
        private IShipmentsContext objectContext;
        private OceanInsightsStatusLogRepository entityRepository;
        public OceanInsightsStatusLogService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new OceanInsightsStatusLogRepository(objectContext);
        }

        public void Create(OceanInsightsStatusLogPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("OceanInsightsStatusLog", tenant).ToString();
            this.Poco = new OceanInsightsStatusLog();
            this.Poco.Id = this.entityPM.Id;
			OceanInsightsStatusLogMapping.MapOceanInsightsStatusLog(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(OceanInsightsStatusLogPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleOceanInsightsStatusLog(theEntityPm.Id, entityPM.Tenant);
			OceanInsightsStatusLogMapping.MapOceanInsightsStatusLog(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();            
        }
    }
}