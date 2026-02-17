using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DimensionsUnitService
    {
          bool isNewEntity;
        private int tenant;
        public DimensionsUnit Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DimensionsUnitPM entityPm;
        private ICommonDataContext objectContext;
        private DimensionsUnitRepository entityRepository;
        public DimensionsUnitService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DimensionsUnitRepository(objectContext);
        }

        public void Create(DimensionsUnitPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            //this.entityPm.Id = IdCounter.GetNumber("DimensionsUnit", tenant).ToString();
            this.Poco = new DimensionsUnit();
            //this.Poco.Id = this.entityPm.Id;

            //DimensionsUnitValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    DimensionsUnitTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            DimensionsUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DimensionsUnitPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleDimensionsUnit(entityPM.Code);

            string entityName = "DimensionsUnit" + entityPM.Code;
            string entityPmName = "DimensionsUnitPM" + entityPM.Code;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            
            DimensionsUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
