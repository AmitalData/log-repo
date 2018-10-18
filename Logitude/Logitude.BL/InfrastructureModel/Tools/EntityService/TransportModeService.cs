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
    public class TransportModeService
    {
          bool isNewEntity;
        private int tenant;
        public TransportMode Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TransportModePM entityPm;
        private IWebFreightContext objectContext;
        private TransportModeRepository entityRepository;
        public TransportModeService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TransportModeRepository(objectContext);
        }

        public void Create(TransportModePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM; 
            this.Poco = new TransportMode();
            
            TransportModeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TransportModePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTransportMode(entityPM.Id);
             
            TransportModeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
