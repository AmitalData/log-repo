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
    public class ExpenseAllocationSettingService
    {
         bool isNewEntity;
        private int tenant;
        public ExpenseAllocationSetting Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ExpenseAllocationSettingPM entityPM;
        private IInvoiceContext objectContext;
        private ExpenseAllocationSettingRepository entityRepository;
        public ExpenseAllocationSettingService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExpenseAllocationSettingRepository(objectContext);
        }

        public void Create(ExpenseAllocationSettingPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ExpenseAllocationSetting", tenant).ToString();
            this.Poco = new ExpenseAllocationSetting();
            this.Poco.Id = this.entityPM.Id;

            ExpenseAllocationSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ExpenseAllocationSettingPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleById(theEntityPm.Id, entityPM.Tenant);
            ExpenseAllocationSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
        }

    }
}