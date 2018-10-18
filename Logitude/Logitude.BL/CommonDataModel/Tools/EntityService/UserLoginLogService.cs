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
    public class UserLoginLogService
    {        
        bool isNewEntity;
        private int tenant;
        public UserLoginLog Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private UserLoginLogPM entityPm;
        private ICommonDataContext objectContext;
        private UserLoginLogRepository entityRepository;
        public UserLoginLogService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new UserLoginLogRepository(objectContext);
        }

        public void Create(UserLoginLogPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("UserLoginLog", tenant).ToString();
            this.Poco = new UserLoginLog();
            this.Poco.Id = this.entityPm.Id;

            UserLoginLogValidating.Validate(entityPM);
            UserLoginLogTracing.Trace(entityPM, Poco, isNewEntity);
            UserLoginLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(UserLoginLogPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleUserLoginLog(entityPM.Id , entityPm.Tenant, true);

            UserLoginLogValidating.Validate(entityPM);
            UserLoginLogTracing.Trace(entityPM, Poco, isNewEntity);
            UserLoginLogMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
