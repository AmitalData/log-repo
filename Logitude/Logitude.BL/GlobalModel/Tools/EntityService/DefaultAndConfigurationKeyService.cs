using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class DefaultAndConfigurationKeyService
    {
        bool isNewEntity;
        private int tenant;
        public DefaultAndConfigurationKey Poco { get; set; }

        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DefaultAndConfigurationKeyPM entityPM;
        private IGlobalContext objectContext;
        private DefaultAndConfigurationKeyRepository entityRepository;
        public DefaultAndConfigurationKeyService(IGlobalContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DefaultAndConfigurationKeyRepository(objectContext);
        }

        public void Create(DefaultAndConfigurationKeyPM theEntityPm)
        {
            
            this.entityPM = theEntityPm;
            this.Poco = new DefaultAndConfigurationKey();
            DefaultAndConfigurationKeyMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(DefaultAndConfigurationKeyPM theEntityPm)
        {
             
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDefaultAndConfigurationKey(theEntityPm.SetKey);
            DefaultAndConfigurationKeyMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(string SetKey)
        { 
            this.Poco = entityRepository.GetSingleDefaultAndConfigurationKey(SetKey); 
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }

    }
}