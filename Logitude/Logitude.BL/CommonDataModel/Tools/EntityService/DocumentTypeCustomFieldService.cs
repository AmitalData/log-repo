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
    public class DocumentTypeCustomFieldService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentTypeCustomField Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentTypeCustomFieldPM entityPM;
        private ICommonDataContext objectContext;
        private DocumentTypeCustomFieldRepository entityRepository;
        public DocumentTypeCustomFieldService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentTypeCustomFieldRepository(objectContext);
        }

        public void Create(DocumentTypeCustomFieldPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("DocumentTypeCustomField", tenant).ToString();
            this.Poco = new DocumentTypeCustomField();
            this.Poco.Id = this.entityPM.Id;

            DocumentTypeCustomFieldValidating.Validate(theEntityPm);
            DocumentTypeCustomFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentTypeCustomFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentTypeCustomFieldPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentTypeCusotmField(theEntityPm.Id, theEntityPm.Tenant);

            DocumentTypeCustomFieldValidating.Validate(theEntityPm);
            DocumentTypeCustomFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentTypeCustomFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
