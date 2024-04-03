using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;


namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class UnassignedEntityService
    {
        bool isNewEntity;
        private int tenant;
        public UnassignedEntity Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private UnassignedEntityPM entityPM;
        private ICommonDataContext objectContext;
        private UnassignedEntityRepository entityRepository;
        public UnassignedEntityService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new UnassignedEntityRepository(objectContext);
        }

        public void Create(UnassignedEntityPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("UnassignedEntity", tenant).ToString();
            this.Poco = new UnassignedEntity();
            this.Poco.Id = this.entityPM.Id;

            UnassignedEntityMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(UnassignedEntityPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleUnassignedEntity(entityPM.Id, entityPM.Tenant);

            UnassignedEntityMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
