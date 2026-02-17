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
    public class PaymentTermService
    {
        bool isNewEntity;
        private int tenant;
        public PaymentTerm Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private PaymentTermPM entityPM;
        private ICommonDataContext objectContext;
        private PaymentTermRepository entityRepository;
        public PaymentTermService(ICommonDataContext commonDataObjectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = commonDataObjectContext;
            this.entityRepository = new PaymentTermRepository(commonDataObjectContext);
        }

        public void Create(PaymentTermPM theEntityPm)
        {
            this.isNewEntity = true;
            if(theEntityPm.IsManuallySet==false)
            theEntityPm.DisplayInLOV = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("PaymentTerm", tenant).ToString();
            this.Poco = new PaymentTerm();
            this.Poco.Id = this.entityPM.Id;

            PaymentTermValidating.Validate(theEntityPm);
            PaymentTermTracing.Trace(theEntityPm, Poco, isNewEntity);
            PaymentTermMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(PaymentTermPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSinglePaymentTerm(theEntityPm.Id, theEntityPm.Tenant);

            PaymentTermValidating.Validate(theEntityPm);
            PaymentTermTracing.Trace(theEntityPm, Poco, isNewEntity);
            PaymentTermMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
