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
    public class CustomMetaDataTypesAddtionalService
    {
        bool isNewEntity;
        private int tenant;
        public CustomMetaDataTypesAddtional Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomMetaDataTypesAddtionalPM entityPm;
        private ICommonDataContext objectContext;
        private CustomMetaDataTypesAddtionalRepository entityRepository;

        public CustomMetaDataTypesAddtionalService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomMetaDataTypesAddtionalRepository(objectContext);
        }

        public void Create(CustomMetaDataTypesAddtionalPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("CustomMetaDataTypesAddtional", tenant).ToString();
            this.Poco = new CustomMetaDataTypesAddtional();
            this.Poco.Id = this.entityPm.Id;

        
            CustomMetaDataTypesAddtionalMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomMetaDataTypesAddtionalPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCustomMetaDataTypesAddtional(entityPM.Id, entityPm.Tenant);

            CustomMetaDataTypesAddtionalMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
