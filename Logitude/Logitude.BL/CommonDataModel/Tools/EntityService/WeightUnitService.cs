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
    public class WeightUnitService
    {
          bool isNewEntity;
        private int tenant;
        public WeightUnit Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private WeightUnitPM entityPm;
        private ICommonDataContext objectContext;
        private WeightUnitRepository entityRepository;
        public WeightUnitService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new WeightUnitRepository(objectContext);
        }

        public void Create(WeightUnitPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            //this.entityPm.Id = IdCounter.GetNumber("WeightUnit", tenant).ToString();
            this.Poco = new WeightUnit();
            //this.Poco.Id = this.entityPm.Id;

            //WeightUnitValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    WeightUnitTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            WeightUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(WeightUnitPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleWeightUnit(entityPM.Code);

            string entityName = "WeightUnit" + entityPM.Code;
            string entityPmName = "WeightUnitPM" + entityPM.Code;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            
            WeightUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
