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
    public class MeasurementService
    {
        bool isNewEntity;
        private int tenant;
        public Measurement Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MeasurementPM entityPM;
        private ICommonDataContext objectContext;
        private MeasurementRepository entityRepository;
        public MeasurementService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MeasurementRepository(objectContext);
        }

        public void Create(MeasurementPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Measurement", tenant).ToString();
            this.Poco = new Measurement();
            this.Poco.Id = this.entityPM.Id;

            MeasurementValidating.Validate(theEntityPm);
            MeasurementTracing.Trace(theEntityPm, Poco, isNewEntity);
            MeasurementMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(MeasurementPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleMeasurement(theEntityPm.Id, theEntityPm.Tenant);

            MeasurementValidating.Validate(theEntityPm);
            MeasurementTracing.Trace(theEntityPm, Poco, isNewEntity);
            MeasurementMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
