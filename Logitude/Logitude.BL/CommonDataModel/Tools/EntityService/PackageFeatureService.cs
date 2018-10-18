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
    public class PackageFeatureService
    {
        bool isNewEntity;
        private int tenant;
        public PackageFeature Poco { get; set; }

        public CommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private PackageFeaturePM entityPm;
        private CommonDataContext objectContext;
        private PackageFeatureRepository entityRepository;
        public PackageFeatureService(CommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new PackageFeatureRepository(objectContext);
        }

        public void Create(PackageFeaturePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("PackageFeature", tenant).ToString();
            this.Poco = new PackageFeature();
            this.Poco.Id = this.entityPm.Id;

            PackageFeatureValidating.Validate(entityPM);
            PackageFeatureTracing.Trace(entityPM, Poco, isNewEntity);
            PackageFeatureMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(PackageFeaturePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSinglePackageFeature(entityPM.Id );

            PackageFeatureValidating.Validate(entityPM);
            PackageFeatureTracing.Trace(entityPM, Poco, isNewEntity);
            PackageFeatureMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
