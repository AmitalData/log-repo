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
    public class SpecialServicesTypeService
    {
        bool isNewEntity;
        private int tenant;
        public SpecialServicesType Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private SpecialServicesTypePM entityPM;
        private IShipmentsContext objectContext;
        private SpecialServicesTypeRepository entityRepository;
        public SpecialServicesTypeService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new SpecialServicesTypeRepository(objectContext);
        }

        public void Create(SpecialServicesTypePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("SpecialServicesType", tenant).ToString();
            this.Poco = new SpecialServicesType();
            this.Poco.Id = this.entityPM.Id;
            if (!theEntityPm.IsHybrid)
            {
                SpecialServicesTypeTracing.Trace(entityPM, Poco, isNewEntity);
            }
            SpecialServicesTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(SpecialServicesTypePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleSpecialServicesType(theEntityPm.Id, entityPM.Tenant);
            if (!theEntityPm.IsHybrid)
            {
                SpecialServicesTypeTracing.Trace(entityPM, Poco, isNewEntity);
            }
            SpecialServicesTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();            
        }
    }
}