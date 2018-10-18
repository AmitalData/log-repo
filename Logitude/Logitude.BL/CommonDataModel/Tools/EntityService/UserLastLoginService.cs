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
    public class UserLastLoginService
    {        
        bool isNewEntity;
        private int tenant;
        public UserLastLogin Poco { get; set; }

        public CommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private UserLastLoginPM entityPm;
        private CommonDataContext objectContext;
        private UserLastLoginRepository entityRepository;
        public UserLastLoginService(CommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new UserLastLoginRepository(objectContext);
        }

        public void Create(UserLastLoginPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("UserLastLogin", tenant).ToString();
            this.Poco = new UserLastLogin();
            this.Poco.Id = this.entityPm.Id;

            UserLastLoginValidating.Validate(entityPM);
            UserLastLoginTracing.Trace(entityPM, Poco, isNewEntity);
            UserLastLoginMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(UserLastLoginPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleUserLastLogin(entityPM.Id , entityPm.Tenant , true);

            UserLastLoginValidating.Validate(entityPM);
            UserLastLoginTracing.Trace(entityPM, Poco, isNewEntity);
            UserLastLoginMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
