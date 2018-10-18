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
    public class OceanInsightsRequestsCountService
    {
        bool isNewEntity;
        private int tenant;
        public OceanInsightsRequestsCount Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private OceanInsightsRequestsCountPM entityPM;
        private IShipmentsContext objectContext;
        private OceanInsightsRequestsCountRepository entityRepository;
        public OceanInsightsRequestsCountService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new OceanInsightsRequestsCountRepository(objectContext);
        }

        public void Create(OceanInsightsRequestsCountPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("OceanInsightsRequestsCount", tenant).ToString();
            this.Poco = new OceanInsightsRequestsCount();
            this.Poco.Id = this.entityPM.Id; 
            OceanInsightsRequestsCountMapping.MapOceanInsightsRequestsCount(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(OceanInsightsRequestsCountPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleOceanInsightsRequestsCount(theEntityPm.Id, entityPM.Tenant); 
            OceanInsightsRequestsCountMapping.MapOceanInsightsRequestsCount(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();            
        }
    }
}