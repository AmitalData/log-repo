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
    public class OceanInsightsRequestService
    {
        bool isNewEntity;
        private int tenant;
        public OceanInsightsRequest Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private OceanInsightsRequestPM entityPM;
        private IShipmentsContext objectContext;
        private OceanInsightsRequestRepository entityRepository;
        public OceanInsightsRequestService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new OceanInsightsRequestRepository(objectContext);
        }

        public void Create(OceanInsightsRequestPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("OceanInsightsRequest", tenant).ToString();
            this.Poco = new OceanInsightsRequest();
            this.Poco.Id = this.entityPM.Id; 
            OceanInsightsRequestMapping.MapOceanInsightsRequest(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(OceanInsightsRequestPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleOceanInsightsRequest(theEntityPm.Id, entityPM.Tenant); 
            OceanInsightsRequestMapping.MapOceanInsightsRequest(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();            
        }
    }
}