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
    public class ContainerTrackingProviderService
    {
        bool isNewEntity;
        private int tenant;
        public ContainerTrackingProvider Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ContainerTrackingProviderPM entityPM;
        private IShipmentsContext objectContext;
        private ContainerTrackingProviderRepository entityRepository;
        public ContainerTrackingProviderService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ContainerTrackingProviderRepository(objectContext);
        }

        public void Create(ContainerTrackingProviderPM theEntityPm)
        {
            Validate(theEntityPm);
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ContainerTrackingProvider", tenant).ToString();
            this.Poco = new ContainerTrackingProvider();
            this.Poco.Id = this.entityPM.Id;
            ContainerTrackingProviderMapping.Map(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        private void Validate(ContainerTrackingProviderPM theEntityPm)
        {
            var isExist = entityRepository.GetBySourceCode(theEntityPm.SourceCode) != null;
            if (isExist)
            {
                throw new Exception($"This source {theEntityPm.SourceCode} has already been added");
            }
        }

        public void Update(ContainerTrackingProviderPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleContainerTrackingProvider(theEntityPm.Id, entityPM.Tenant);
            ContainerTrackingProviderMapping.Map(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
