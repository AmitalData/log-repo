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
    public class OceanInsightsStatusesService
    {
        bool isNewEntity;
        private int tenant;
        public OceanInsightsStatuses Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private OceanInsightsStatusesPM entityPM;
        private IShipmentsContext objectContext;
        private OceanInsightsStatusesRepository entityRepository;
        public OceanInsightsStatusesService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new OceanInsightsStatusesRepository(objectContext);
        }

        public void Create(OceanInsightsStatusesPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("OceanInsightsStatuses", tenant).ToString();
            this.Poco = new OceanInsightsStatuses();
            this.Poco.Id = this.entityPM.Id; 
            OceanInsightsStatusesMapping.MapOceanInsightsStatuses(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(OceanInsightsStatusesPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleOceanInsightsStatus(theEntityPm.Id, entityPM.Tenant); 
            OceanInsightsStatusesMapping.MapOceanInsightsStatuses(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();            
        }
    }
}