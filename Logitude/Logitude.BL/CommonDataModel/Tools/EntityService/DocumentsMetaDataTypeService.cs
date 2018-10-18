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
    public class DocumentsMetaDataTypeService
    {
             bool isNewEntity;
        private int tenant;
        public DocumentsMetaDataType Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentsMetaDataTypePM entityPM;
        private ICommonDataContext objectContext;
        private DocumentsMetaDataTypeRepository entityRepository;
        public DocumentsMetaDataTypeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentsMetaDataTypeRepository(objectContext);
        }

        public void Create(DocumentsMetaDataTypePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("DocumentsMetaDataType", tenant).ToString();
            this.Poco = new DocumentsMetaDataType();
            this.Poco.Id = this.entityPM.Id;

            DocumentsMetaDataTypeValidating.Validate(theEntityPm);
            DocumentsMetaDataTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentsMetaDataTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentsMetaDataTypePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentsMetaDataType(theEntityPm.Id, theEntityPm.Tenant);

            DocumentsMetaDataTypeValidating.Validate(theEntityPm);
            DocumentsMetaDataTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentsMetaDataTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
