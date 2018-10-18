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
    public class ExternalSystemsTablesCodeService
    {
         bool isNewEntity;
        private int tenant;
        public ExternalSystemsTablesCode Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ExternalSystemsTablesCodePM entityPM;
        private IInvoiceContext objectContext;
        private ExternalSystemsTablesCodeRepository entityRepository;
        public ExternalSystemsTablesCodeService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExternalSystemsTablesCodeRepository(objectContext);
        }

        public void Create(ExternalSystemsTablesCodePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ExternalSystemsTablesCode", tenant).ToString();
            this.Poco = new ExternalSystemsTablesCode();
            this.Poco.Id = this.entityPM.Id;

            ExternalSystemsTablesCodeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ExternalSystemsTablesCodePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleExternalSystemsTablesCode(theEntityPm.Id, entityPM.Tenant);
            ExternalSystemsTablesCodeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
        }

    }
}