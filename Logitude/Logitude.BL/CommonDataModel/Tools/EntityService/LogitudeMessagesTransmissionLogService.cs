using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class LogitudeMessagesTransmissionLogService
    {
        bool isNewEntity;
        private int tenant;
        public LogitudeMessagesTransmissionLog Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private LogitudeMessagesTransmissionLogPM entityPm;
        private ICommonDataContext objectContext;
        private LogitudeMessagesTransmissionLogRepository entityRepository;
        public LogitudeMessagesTransmissionLogService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new LogitudeMessagesTransmissionLogRepository(objectContext);
        }

        public void Create(LogitudeMessagesTransmissionLogPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("LogitudeMessagesTransmissionLog", tenant).ToString();
            this.Poco = new LogitudeMessagesTransmissionLog();
            this.Poco.Id = this.entityPm.Id;            
           // LogitudeMessagesTransmissionLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(LogitudeMessagesTransmissionLogPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleLogitudeMessagesTransmissionLog(entityPm.Tenant, entityPM.Id);



           // CardExternalCodeByCurrencyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

    }
}
