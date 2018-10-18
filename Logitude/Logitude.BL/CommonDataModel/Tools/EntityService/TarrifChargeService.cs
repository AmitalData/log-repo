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
    public class TarrifChargeService
    {
        bool isNewEntity;
        private int tenant;
        public TarrifCharge Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TarrifChargePM entityPm;
        private ICommonDataContext objectContext;
        private TarrifChargeRepository entityRepository;
        public TarrifChargeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TarrifChargeRepository(objectContext);
        }

        public void Create(TarrifChargePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("TarrifCharge", tenant).ToString();
            this.Poco = new TarrifCharge();
            this.Poco.Id = this.entityPm.Id;

            TarrifChargeValidating.Validate(entityPM);
            TarrifChargeTracing.Trace(entityPM, Poco, isNewEntity);
            TarrifChargeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TarrifChargePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTarrifCharge(entityPM.Id );

            TarrifChargeValidating.Validate(entityPM);
            TarrifChargeTracing.Trace(entityPM, Poco, isNewEntity);
            TarrifChargeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
