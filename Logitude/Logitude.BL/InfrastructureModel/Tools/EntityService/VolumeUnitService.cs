using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class VolumeUnitService
    {
          bool isNewEntity;
        private int tenant;
        public  VolumeUnit Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private  VolumeUnitPM entityPm;
        private IWebFreightContext objectContext;
        private  VolumeUnitRepository entityRepository;
        public  VolumeUnitService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new  VolumeUnitRepository(objectContext);
        }

        public void Create( VolumeUnitPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM; 
            this.Poco = new  VolumeUnit();
            
             VolumeUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update( VolumeUnitPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleVolumeUnit(entityPM.Code);
             
             VolumeUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
