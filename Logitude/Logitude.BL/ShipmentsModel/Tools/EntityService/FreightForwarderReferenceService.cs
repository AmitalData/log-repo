using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class FreightForwarderReferenceService
    {
        bool isNewEntity;
        private int tenant;
        public FreightForwarderReference Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private FreightForwarderReferencePM entityPM;
        private IShipmentsContext objectContext;
        private FreightForwarderReferenceRepository entityRepository;
        public FreightForwarderReferenceService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new FreightForwarderReferenceRepository(objectContext);
        }

        public void Create(FreightForwarderReferencePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.Poco = new FreightForwarderReference();
            this.Poco.ShipmentId = this.entityPM.ShipmentId;
            ShipmentMapping.MapFreightForwarderReference(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(FreightForwarderReferencePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleFreightForwarderReference(entityPM.Tenant, theEntityPm.ShipmentId,theEntityPm.LineNumber);
            ShipmentMapping.MapFreightForwarderReference(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
