using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ContainerService
    {
        bool isNewEntity;
        private int tenant;  
        private ContainerPM entityPm;
        private IShipmentsContext objectContext;
        private ContainerRepository entityRepository;
        public Container containerPoco { get; set; }
        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        public ContainerService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ContainerRepository(objectContext);
        }

        public void Create(ContainerPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.containerPoco = new Container();
            this.entityPm.Id = IdCounter.GetNumber("Container", tenant).ToString();
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            entityRepository.Add(containerPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(ContainerPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id,tenant);

            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            entityRepository.Update(containerPoco);
            entityRepository.SubmitChanges();
        }
    }
}
