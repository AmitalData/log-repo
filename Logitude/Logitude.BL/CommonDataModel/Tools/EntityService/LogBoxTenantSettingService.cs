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
    public class LogBoxTenantSettingService
    {
        bool isNewEntity;
        private int tenant;
        public LogBoxTenantSetting Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private LogBoxTenantSettingPM entityPm;
        private ICommonDataContext objectContext;
        private LogBoxTenantSettingRepository entityRepository;
        public LogBoxTenantSettingService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new LogBoxTenantSettingRepository(objectContext);
        }

        public void Create(LogBoxTenantSettingPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
           // this.entityPm.Id = IdCounter.GetNumber("ContactTenant", tenant).ToString();
            this.Poco = new LogBoxTenantSetting();
            this.Poco.Id = this.entityPm.Id;


            LogBoxTenantSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(LogBoxTenantSettingPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleLBTenant(entityPM.Id);


            LogBoxTenantSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
