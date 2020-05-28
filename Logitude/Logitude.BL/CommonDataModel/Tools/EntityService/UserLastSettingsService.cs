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
    public class UserLastSettingsService
    {        
        bool isNewEntity;
        private int tenant;
        public UserLastSettings Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private UserLastSettingsPM entityPm;
        private ICommonDataContext objectContext;
        private UserLastSettingsRepository entityRepository;
        public UserLastSettingsService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new UserLastSettingsRepository(objectContext);
        }

        public void Create(UserLastSettingsPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("UserLastSettings", tenant).ToString();
            this.Poco = new UserLastSettings();
            this.Poco.Id = this.entityPm.Id;

            //UserLastSettingsValidating.Validate(entityPM);
            //UserLastSettingsTracing.Trace(entityPM, Poco, isNewEntity);
            UserLastSettingsMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(UserLastSettingsPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleUserLastSettings(entityPM.Id , entityPm.Tenant);

            //UserLastSettingsValidating.Validate(entityPM);
            //UserLastSettingsTracing.Trace(entityPM, Poco, isNewEntity);
            UserLastSettingsMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
