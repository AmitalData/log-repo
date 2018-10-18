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
    public class ExternalSystemsSyncStatusService
    {

          bool isNewEntity;
        private int tenant;
        public ExternalSystemsSyncStatus Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ExternalSystemsSyncStatusPM entityPM;
        private IInvoiceContext objectContext;
        private ExternalSystemsSyncStatusRepository entityRepository;
        public ExternalSystemsSyncStatusService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExternalSystemsSyncStatusRepository(objectContext);
        }

        public void Create(ExternalSystemsSyncStatusPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ExternalSystemsSyncStatus", tenant).ToString();
            this.Poco = new ExternalSystemsSyncStatus();
            this.Poco.Id = this.entityPM.Id;

            ExternalSystemsSyncStatusMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ExternalSystemsSyncStatusPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleExternalSystemsSyncStatus(theEntityPm.Id, entityPM.Tenant);
            ExternalSystemsSyncStatusMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
        }

    }
}