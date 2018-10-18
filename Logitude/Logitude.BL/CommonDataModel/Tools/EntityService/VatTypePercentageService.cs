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
    public class VatTypePercentageService
    {
        bool isNewEntity;
        private int tenant;
        public VatTypePercentage Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private VatTypePercentagePM entityPm;
        private ICommonDataContext objectContext;
        private VatTypePercentageRepository entityRepository;
        public VatTypePercentageService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new VatTypePercentageRepository(objectContext);
        }

        public void Create(VatTypePercentagePM entityPM , VatTypePM vatTypePm)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            this.entityPm.Id = IdCounter.GetNumber("VatTypePercentage", entityPm.Tenant);
            this.Poco = new VatTypePercentage();
            this.Poco.Id = this.entityPm.Id;
            entityPM.VatTypeId = vatTypePm.Id;
            this.Poco.VatTypeId = vatTypePm.Id;
    
            VatTypePercentageValidating.Validate(entityPM);
            VatTypePercentageTracing.Trace(entityPM, Poco, isNewEntity);
            VatTypePercentageMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(VatTypePercentagePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleVatTypePercentage(entityPM.Id);

            VatTypePercentageValidating.Validate(entityPM);
            VatTypePercentageTracing.Trace(entityPM, Poco, isNewEntity);
            VatTypePercentageMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
