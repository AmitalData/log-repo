using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AccountingSettingService
    {
        bool isNewEntity;
        private int tenant;
        public AccountingSetting Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AccountingSettingPM entityPm;
        private ICommonDataContext objectContext;
        private AccountingSettingRepository entityRepository;

        public AccountingSettingService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AccountingSettingRepository(objectContext);
        }

        public void Create(AccountingSettingPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = this.tenant; 
            this.Poco = new AccountingSetting();
            this.Poco.Id = this.entityPm.Id;

            //AccountingSettingTracing.Trace(entityPM, Poco, isNewEntity);
            AccountingSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            TenantRepository tenantRepository = new TenantRepository(entityPM.Id);
            Tenant tenant = tenantRepository.GetSingleTenant(entityPM.Id);
            tenant.VatNumber = entityPM.VatNumber;
            tenant.PaymentTermId = entityPM.PaymentTermId;
            //tenant.AccountingActivationDate = entityPM.AccountingActivationDate;
            //tenant.AccountingActivated = entityPM.AccountingActivated;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();

            

        }

        public void Update(AccountingSettingPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleAccountingSetting(entityPM.Id);

            //AccountingSettingTracing.Trace(entityPM, Poco, isNewEntity);
            AccountingSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            TenantRepository tenantRepository = new TenantRepository(entityPM.Id);
            Tenant tenant = tenantRepository.GetSingleTenant(entityPM.Id);
            tenant.VatNumber = entityPM.VatNumber;
            tenant.PaymentTermId = entityPM.PaymentTermId;
            //tenant.AccountingActivationDate = entityPM.AccountingActivationDate;
            //tenant.AccountingActivated = entityPM.AccountingActivated;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();
        }   
    }
}
