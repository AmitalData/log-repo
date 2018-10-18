using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class AccountingSystemsSettingService
    {

        bool isNewEntity;
        private int tenant;
        public AccountingSystemsSetting Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AccountingSystemsSettingPM entityPM;
        private IInvoiceContext objectContext;
        private AccountingSystemsSettingRepository entityRepository;
        public AccountingSystemsSettingService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AccountingSystemsSettingRepository(objectContext);
        }

        public void Create(AccountingSystemsSettingPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("AccountingSystemsSetting", tenant).ToString();
            this.Poco = new AccountingSystemsSetting();
            this.Poco.Id = this.entityPM.Id;

            AccountingSystemsSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(AccountingSystemsSettingPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleAccountingSystemsSetting(theEntityPm.Id, entityPM.Tenant);
            AccountingSystemsSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
        }

    }
}