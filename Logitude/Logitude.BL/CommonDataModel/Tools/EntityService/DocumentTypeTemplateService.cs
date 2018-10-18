using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentTypeTemplateService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentTypeTemplate Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentTypeTemplatePM entityPM;
        private ICommonDataContext objectContext;
        private DocumentTypeTemplateRepository entityRepository;
        public DocumentTypeTemplateService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentTypeTemplateRepository(objectContext);
        }

        public void Create(DocumentTypeTemplatePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("DocumentTypeTemplate", tenant).ToString();
            this.Poco = new DocumentTypeTemplate();
            this.Poco.Id = this.entityPM.Id;

            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            entityPM.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.LastUpdatedByUserId = loggedContact.Id;
            entityPM.LastUpdateByUserName = loggedContact.EnglishName;

            DocumentTypeTemplateValidating.Validate(theEntityPm);
            DocumentTypeTemplateTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentTypeTemplateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentTypeTemplatePM theEntityPm)
        {
            
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentTypeTemplate(theEntityPm.Id);

            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            entityPM.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.LastUpdatedByUserId = loggedContact.Id;
            entityPM.LastUpdateByUserName = loggedContact.EnglishName;

            DocumentTypeTemplateValidating.Validate(theEntityPm);
            DocumentTypeTemplateTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentTypeTemplateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
