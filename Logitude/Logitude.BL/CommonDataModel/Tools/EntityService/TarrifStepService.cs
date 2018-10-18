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
    public class TarrifStepService
    {
        bool isNewEntity;
        private int tenant;
        public TarrifStep Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TarrifStepPM entityPm;
        private ICommonDataContext objectContext;
        private TarrifStepRepository entityRepository;
        public TarrifStepService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TarrifStepRepository(objectContext);
        }

        public void Create(TarrifStepPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("TarrifStep", tenant).ToString();
            this.Poco = new TarrifStep();
            this.Poco.Id = this.entityPm.Id;

            TarrifStepValidating.Validate(entityPM);
            TarrifStepTracing.Trace(entityPM, Poco, isNewEntity);
            TarrifStepMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TarrifStepPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTarrifStep(entityPM.Id);

            TarrifStepValidating.Validate(entityPM);
            TarrifStepTracing.Trace(entityPM, Poco, isNewEntity);
            TarrifStepMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
