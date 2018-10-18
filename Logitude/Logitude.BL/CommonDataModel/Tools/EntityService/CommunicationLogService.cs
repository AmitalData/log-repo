using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CommunicationLogService
    {
        bool isNewEntity;
        private int tenant;
        public CommunicationLog Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CommunicationLogPM entityPM;
        private ICommonDataContext objectContext;
        private CommunicationLogRepository entityRepository;
        public CommunicationLogService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CommunicationLogRepository(objectContext);
        }

        public void Create(CommunicationLogPM theEntityPm)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.isNewEntity = true;
                this.entityPM = theEntityPm;
                this.entityPM.Id = IdCounter.GetNumber("CommunicationLog", tenant).ToString();
                this.Poco = new CommunicationLog();
                this.Poco.Id = this.entityPM.Id;

                CommunicationLogValidating.Validate(theEntityPm);
                CommunicationLogTracing.Trace(theEntityPm, Poco, isNewEntity);
                CommunicationLogMapping.MapEntity(theEntityPm, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                scope.Complete();
            }
        }

        public void Update(CommunicationLogPM theEntityPm)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.isNewEntity = false;
                this.entityPM = theEntityPm;
                this.Poco = entityRepository.GetSingleCommunicationLog(theEntityPm.Id, theEntityPm.Tenant);

                CommunicationLogValidating.Validate(theEntityPm);
                CommunicationLogTracing.Trace(theEntityPm, Poco, isNewEntity);
                CommunicationLogMapping.MapEntity(theEntityPm, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
                scope.Complete();
            }
        }
    }
}
