using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DistributorService
    {
         bool isNewEntity;
        private int tenant;
        private DistributorPM entityPM;
        private ICommonDataContext objectContext;
        private DistributorRepository entityRepository;
        public Distributor Poco { get; set; }
        public DistributorService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new DistributorRepository(objectContext);
        }

        public void Create(DistributorPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.Poco = new Distributor();
            DistributorMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DistributorPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleDistributor(entityPM.Code, tenant);
            DistributorMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
