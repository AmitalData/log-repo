using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class EmailProviderService
    {
        bool isNewEntity;
        private int tenant;
        public EmailProvider Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private EmailProviderPM entityPm;
        private ICommonDataContext objectContext;
        private EmailProviderRepository entityRepository;

        public EmailProviderService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new EmailProviderRepository(objectContext);
        }

        public void Create(EmailProviderPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.Poco = new EmailProvider();

            EmailProviderValidating.Validate(entityPM);
           
            EmailProviderMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(EmailProviderPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleEmailProvider(entityPM.ProviderNumber);

            EmailProviderValidating.Validate(entityPM);          
            EmailProviderMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
