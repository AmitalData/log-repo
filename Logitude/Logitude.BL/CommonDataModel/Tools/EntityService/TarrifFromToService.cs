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
    public class TarrifFromToService
    {
         bool isNewEntity;
        private int tenant;
        public TarrifFromTo Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TarrifFromToPM entityPm;
        private ICommonDataContext objectContext;
        private TarrifFromToRepository entityRepository;
        public TarrifFromToService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TarrifFromToRepository(objectContext);
        }

        public void Create(TarrifFromToPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("TarrifFromTo", tenant).ToString();
            this.Poco = new TarrifFromTo();
            this.Poco.Id = this.entityPm.Id;

            TarrifFromToValidating.Validate(entityPM);
            TarrifFromToTracing.Trace(entityPM, Poco, isNewEntity);
            TarrifFromToMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TarrifFromToPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTarrifFromTo(entityPM.Id);

            TarrifFromToValidating.Validate(entityPM);
            TarrifFromToTracing.Trace(entityPM, Poco, isNewEntity);
            TarrifFromToMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
