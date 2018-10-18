using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
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
    public class CustomerFieldsUpdateSettingService
    {
        bool isNewEntity;
        private int tenant;
        public CustomerFieldsUpdateSetting Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomerFieldsUpdateSettingPM entityPm;
        private ICommonDataContext objectContext;
        private CustomerFieldsUpdateSettingRepository entityRepository;
        public CustomerFieldsUpdateSettingService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerFieldsUpdateSettingRepository(objectContext);
        }


        public void Create(CustomerFieldsUpdateSettingPM entityPM)
        {
            CustomerFieldsUpdateSettingQuery customerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(tenant);
            bool result = customerFieldsUpdateSettingQuery.CheckIfExistCustomerFieldsUpdateSetting(entityPM.ObjectFieldId, tenant);
            if (result)
            {
                throw new Exception("An update setting already exists for the field");
            }

            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("CustomerFieldsUpdateSetting", tenant).ToString();
            this.Poco = new CustomerFieldsUpdateSetting();
            CustomerFieldsUpdateSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomerFieldsUpdateSettingPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCustomerFieldsUpdateSetting(entityPM.Id, entityPm.Tenant);
            CustomerFieldsUpdateSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

    }
}
