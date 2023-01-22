using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class PortGroupService
    {
        bool isNewEntity;
        private int tenant;
        private PortGroup entityPOCO;
        private PortGroupPM entityPM;
        private PortGroupRepository entityRepository;
        public PortGroupService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.entityRepository = new PortGroupRepository(objectContext);
        }

        public void Create(PortGroupPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("PortGroup", tenant).ToString();
            this.entityPOCO = new PortGroup();
            this.entityPOCO.Id = this.entityPM.Id;

            PortGroupValidating.Validate(entityPM);
            PortGroupTracing.Trace(entityPM, entityPOCO, isNewEntity);
            PortGroupMapping.MapEntity(entityPM, entityPOCO, isNewEntity);
            entityRepository.Add(entityPOCO);
            entityRepository.SubmitChanges();
        }

        public void Update(PortGroupPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPOCO = entityRepository.GetSinglePortGroup(entityPM.Id, entityPM.Tenant);

            PortGroupValidating.Validate(entityPM);
            PortGroupTracing.Trace(entityPM, entityPOCO, isNewEntity);            
            PortGroupMapping.MapEntity(entityPM, entityPOCO, isNewEntity);
            entityRepository.Update(entityPOCO);
            entityRepository.SubmitChanges();
        }
    }
}
