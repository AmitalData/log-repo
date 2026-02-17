using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class RoleFeatureService
    {

        bool isNewEntity;
        private int tenant;
        public RoleFeature Poco { get; set; }

        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private RoleFeaturePM entityPm;
        private IGlobalContext objectContext;
        private RoleFeatureRepository entityRepository;
        public RoleFeatureService(IGlobalContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new RoleFeatureRepository(objectContext);
        }

        public void Create(RoleFeaturePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("RoleFeature", tenant).ToString();
            this.Poco = new RoleFeature();
            this.Poco.Id = this.entityPm.Id;

            RoleFeatureValidating.Validate(entityPM);
            RoleFeatureTracing.Trace(entityPM, Poco, isNewEntity);
            RoleFeatureMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(RoleFeaturePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleRoleFeature(entityPM.Id );

            RoleFeatureValidating.Validate(entityPM);
            RoleFeatureTracing.Trace(entityPM, Poco, isNewEntity);
            RoleFeatureMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
