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
    public class DocumentTypeCopyService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentTypeCopy Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentTypeCopyPM entityPm;
        private ICommonDataContext objectContext;
        private DocumentTypeCopyRepository entityRepository;
        public DocumentTypeCopyService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentTypeCopyRepository(objectContext);
        }

        public void Create(DocumentTypeCopyPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString();
            this.Poco = new DocumentTypeCopy();
            this.Poco.Id = this.entityPm.Id;

            DocumentTypeCopyValidating.Validate(entityPM);
            DocumentTypeCopyTracing.Trace(entityPM, Poco, isNewEntity);
            DocumentTypeCopyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentTypeCopyPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleDocumentTypeCopy(entityPM.Id);

            DocumentTypeCopyValidating.Validate(entityPM);
            DocumentTypeCopyTracing.Trace(entityPM, Poco, isNewEntity);
            DocumentTypeCopyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
