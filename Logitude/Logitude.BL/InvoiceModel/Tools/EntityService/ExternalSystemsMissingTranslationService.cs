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
    public class ExternalSystemsMissingTranslationService
    {

          bool isNewEntity;
        private int tenant;
        public ExternalSystemsMissingTranslation Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ExternalSystemsMissingTranslationPM entityPM;
        private IInvoiceContext objectContext;
        private ExternalSystemsMissingTranslationRepository entityRepository;
        public ExternalSystemsMissingTranslationService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExternalSystemsMissingTranslationRepository(objectContext);
        }

        public void Create(ExternalSystemsMissingTranslationPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ExternalSystemsMissingTranslation", tenant).ToString();
            this.Poco = new ExternalSystemsMissingTranslation();
            this.Poco.Id = this.entityPM.Id;

            ExternalSystemsMissingTranslationMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ExternalSystemsMissingTranslationPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleExternalSystemsMissingTranslation(theEntityPm.Id, entityPM.Tenant);
            ExternalSystemsMissingTranslationMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
        }
    }
}