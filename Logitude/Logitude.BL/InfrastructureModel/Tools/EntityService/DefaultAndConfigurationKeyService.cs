using System;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DefaultAndConfigurationKeyService
    {
        bool isNewEntity;
        private int tenant;
        public DefaultAndConfigurationKey Poco { get; set; }

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