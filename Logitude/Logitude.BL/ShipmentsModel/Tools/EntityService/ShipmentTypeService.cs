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
    public class ShipmentTypeService
    {
          bool isNewEntity;
        private int tenant;
        public ShipmentType Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ShipmentTypePM entityPm;
        private IShipmentsContext objectContext;
        private ShipmentTypeRepository entityRepository;
        public ShipmentTypeService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ShipmentTypeRepository(objectContext);
        }

        public void Create(ShipmentTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM; 
            this.Poco = new ShipmentType();
            this.entityPm.Id = IdCounter.GetNumber("ShipmentType", tenant).ToString();
            ShipmentTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ShipmentTypePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleShipmentType(entityPM.Id);
             
            ShipmentTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
