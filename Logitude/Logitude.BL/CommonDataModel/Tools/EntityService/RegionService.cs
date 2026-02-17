using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class RegionService
    {

          bool isNewEntity;
        private int tenant;
        public Region Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private RegionPM entityPm;
        private ICommonDataContext objectContext;
        private RegionRepository entityRepository;

        public RegionService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new RegionRepository(objectContext);
        }

        public void Create(RegionPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.Poco = new Region();

            this.Poco.Id = IdCounter.GetNumber("Region", entityPm.Tenant).ToString();
            entityPm.Id = this.Poco.Id;

            RegionTracing.Trace(entityPM, Poco, isNewEntity); 
            RegionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();                      
        }

        public void Update(RegionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleRegion(entityPM.Id, entityPm.Tenant);

            RegionTracing.Trace(entityPM, Poco, isNewEntity);    
            RegionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();                    
        }
    }
}
