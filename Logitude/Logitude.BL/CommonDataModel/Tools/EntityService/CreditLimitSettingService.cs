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
    public class CreditLimitSettingService
    {
        bool isNewEntity;
        private int tenant;
        public CreditLimitSetting Poco { get; set; }
        private CreditLimitSettingPM entityPM;
        private ICommonDataContext objectContext;
        private CreditLimitSettingRepository entityRepository;
        public CreditLimitSettingService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CreditLimitSettingRepository(objectContext);
        }

        public void Create(CreditLimitSettingPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            //this.entityPM.Id = IdCounter.GetNumber("CreditLimitSetting", tenant).ToString();
            this.entityPM.Id = this.tenant.ToString();

            this.Poco = new CreditLimitSetting()
            {
                Id = this.entityPM.Id,
                Tenant = tenant
            };

            CreditLimitSettingValidating.Validate(entityPM);
            CreditLimitSettingTracing.Trace(entityPM, Poco, isNewEntity);
            CreditLimitSettingMpapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CreditLimitSettingPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCreditLimitSetting(entityPM.Id, tenant);

            CreditLimitSettingValidating.Validate(entityPM);
            CreditLimitSettingTracing.Trace(entityPM, Poco, isNewEntity);
            CreditLimitSettingMpapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
