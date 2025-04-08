using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentTypeMetaDataService
    {
        private int tenant;
        public DocumentTypeMetaData Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentTypeMetaDataPM entityPM;
        private ICommonDataContext objectContext;
        private DocumentTypeMetaDataRepository entityRepository;
        
        public DocumentTypeMetaDataService(int tenant)
        {
            this.tenant = tenant;
            ObjectContext = CommonDataContext.GetContext(tenant);
            entityRepository = new DocumentTypeMetaDataRepository(ObjectContext);
        }

        public DocumentTypeMetaDataService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentTypeMetaDataRepository(objectContext);
        }

        public void Create(DocumentTypeMetaDataPM theEntityPm)
        {
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("DocumentTypeMetaData", tenant).ToString();
            this.Poco = new DocumentTypeMetaData();
            this.Poco.Id = this.entityPM.Id;

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentTypeMetaDataPM theEntityPm)
        {
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentTypeMetaData(theEntityPm.Id, theEntityPm.Tenant);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(string id)
        {
            this.Poco = entityRepository.GetSingleDocumentTypeMetaData(id, tenant);
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
