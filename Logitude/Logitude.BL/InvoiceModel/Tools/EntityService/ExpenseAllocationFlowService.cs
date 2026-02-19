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
    public class ExpenseAllocationFlowService
    {
         bool isNewEntity;
        private int tenant;
        public ExpenseAllocationFlow Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ExpenseAllocationFlowPM entityPM;
        private IInvoiceContext objectContext;
        private ExpenseAllocationFlowRepository entityRepository;
        public ExpenseAllocationFlowService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExpenseAllocationFlowRepository(objectContext);
        }

        public void Create(ExpenseAllocationFlowPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ExpenseAllocationFlow", tenant).ToString();
            this.Poco = new ExpenseAllocationFlow();
            this.Poco.Id = this.entityPM.Id;

            ExpenseAllocationFlowMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ExpenseAllocationFlowPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleById(theEntityPm.Id, entityPM.Tenant);
            ExpenseAllocationFlowMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
        }

    }
}