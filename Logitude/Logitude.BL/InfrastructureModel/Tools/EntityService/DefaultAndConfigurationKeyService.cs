using System;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DefaultAndConfigurationKeyService
    {
        bool isNewEntity;
        private int tenant;
        public DefaultAndConfigurationKeys Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DefaultAndConfigurationKeyPM entityPM;
        private IWebFreightContext objectContext;
        private DefaultAndConfigurationKeyRepository entityRepository;
        public DefaultAndConfigurationKeyService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DefaultAndConfigurationKeyRepository(objectContext);
        }

        public void Create(DefaultAndConfigurationKeyPM theEntityPm)
        {
            
            this.entityPM = theEntityPm;
            this.Poco = new DefaultAndConfigurationKeys();
            DefaultAndConfigurationKeyMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(DefaultAndConfigurationKeyPM theEntityPm)
        {
             
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDefaultAndConfigurationKey(theEntityPm.Id);
            DefaultAndConfigurationKeyMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(string Id)
        { 
            this.Poco = entityRepository.GetSingleDefaultAndConfigurationKey(Id); 
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }

    }
}