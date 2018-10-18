using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class HybridTenantThresholdService
    {
        bool isNewEntity;
        private int tenant;
        public HybridTenantThreshold Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private HybridTenantThresholdPM entityPm;
        private ICommonDataContext objectContext;
        private HybridTenantThresholdRepository entityRepository;

        public HybridTenantThresholdService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new HybridTenantThresholdRepository(objectContext);
        }

        public void Create(HybridTenantThresholdPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
           
            this.Poco = new HybridTenantThreshold();
            

          //  HybridTenantThresholdValidating.Validate(entityPM);
           // HybridTenantThresholdTracing.Trace(entityPM, Poco, isNewEntity);
            HybridTenantThresholdMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(HybridTenantThresholdPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleHybridTenantThreshold(entityPm.Tenant);

            // HybridTenantThresholdValidating.Validate(entityPM);
             //HybridTenantThresholdTracing.Trace(entityPM, Poco, isNewEntity);
             HybridTenantThresholdMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
