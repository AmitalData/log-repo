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
    public class DocumentOutCopyService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentOutCopy Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentOutCopyPM entityPm;
        private ICommonDataContext objectContext;
        private DocumentOutCopyRepository entityRepository;
        public DocumentOutCopyService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentOutCopyRepository(objectContext);
        }

        public void Create(DocumentOutCopyPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("DocumentOutCopy", tenant).ToString();
            this.Poco = new DocumentOutCopy();
            this.Poco.Id = this.entityPm.Id;

            DocumentOutCopyValidating.Validate(entityPM);
            DocumentOutCopyTracing.Trace(entityPM, Poco, isNewEntity);
            DocumentOutCopyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentOutCopyPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleDocumentOutCopy(entityPM.Id );


            DocumentOutCopyValidating.Validate(entityPM);
            DocumentOutCopyTracing.Trace(entityPM, Poco, isNewEntity);
            DocumentOutCopyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
