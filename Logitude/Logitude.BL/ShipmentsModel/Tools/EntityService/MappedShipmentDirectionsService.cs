using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class MappedShipmentDirectionsService
    {
        bool isNewEntity;
        private int tenant;
        public MappedShipmentDirections Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MappedShipmentDirectionsPM entityPM;
        private IShipmentsContext objectContext;
        private MappedShipmentDirectionsRepository entityRepository;
        public MappedShipmentDirectionsService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MappedShipmentDirectionsRepository(objectContext);
        }

        public void Create(MappedShipmentDirectionsPM theEntityPm)
        {

            MappedShipmentDirectionsMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(MappedShipmentDirectionsPM theEntityPm)
        {
            this.Poco = entityRepository.GetSingleMappedShipmentDirection(theEntityPm.Tenant, theEntityPm.ShipmentDirectionId);

            MappedShipmentDirectionsMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();            
        }
    }
}
