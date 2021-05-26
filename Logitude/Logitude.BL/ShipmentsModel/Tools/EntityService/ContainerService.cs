using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ContainerService
    {
        bool isNewEntity;
        private int tenant;  
        private ContainerPM containerPm;
        private IShipmentsContext shipmentsContext;
        private ContainerRepository entityRepository;
        private Container containerPoco { get; set; }

        public ContainerService(IShipmentsContext shipmentsContext, int tenant)
        {
            this.tenant = tenant;
            this.shipmentsContext = shipmentsContext;
            this.entityRepository = new ContainerRepository(shipmentsContext);
        }

        public void Create(ContainerPM entityPM)
        {
            this.isNewEntity = true;
            this.containerPm = entityPM;
            this.containerPm.Id = IdCounter.GetNumber("Container", tenant).ToString();
            this.containerPoco = new Container { Id = this.containerPm.Id, Tenant = this.containerPm.Tenant };
            ContainerTracing.Trace(entityPM, containerPoco, isNewEntity);
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);
            entityRepository.Add(containerPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(ContainerPM entityPM, bool isDeleted = false)
        {
            this.isNewEntity = false;
            this.containerPm = entityPM;
            this.containerPoco = entityRepository.GetSingleContainer(entityPM.Id, tenant);
            ContainerTracing.Trace(entityPM, containerPoco, isNewEntity);
            ShipmentMapping.MapContainer(entityPM, containerPoco, isNewEntity);

            if (isDeleted == true)
                entityRepository.Remove(containerPoco);
            else
                entityRepository.Update(containerPoco);

            entityRepository.SubmitChanges();
        }
    }
}
