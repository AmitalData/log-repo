using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ContainerTrackingRequestService
    {
        bool isNewEntity;
        private int tenant;
        public ContainerTrackingRequest Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ContainerTrackingRequestPM entityPM;
        private IShipmentsContext objectContext;
        private ContainerTrackingRequestRepository entityRepository;
        public ContainerTrackingRequestService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ContainerTrackingRequestRepository(objectContext);
        }

        public void Create(ContainerTrackingRequestPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ContainerTrackingRequest", tenant).ToString();
            this.Poco = new ContainerTrackingRequest();
            this.Poco.Id = this.entityPM.Id;
            ContainerTrackingRequestMapping.MapFields(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ContainerTrackingRequestPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleContainerTrackingRequest(theEntityPm.Id, entityPM.Tenant);
            ContainerTrackingRequestMapping.MapFields(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
