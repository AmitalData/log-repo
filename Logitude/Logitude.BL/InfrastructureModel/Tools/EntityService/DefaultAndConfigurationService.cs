using System;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DefaultAndConfigurationService
    {
        bool isNewEntity;
        private int tenant;
        public DefaultAndConfiguration Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DefaultAndConfigurationPM entityPM;
        private IWebFreightContext objectContext;
        private DefaultAndConfigurationsRepository entityRepository;
        public DefaultAndConfigurationService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DefaultAndConfigurationsRepository(objectContext);
        }

        public void Create(DefaultAndConfigurationPM theEntityPm)
        {
            
            this.entityPM = theEntityPm;
            this.Poco = new DefaultAndConfiguration();
            DefaultAndConfigurationMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(DefaultAndConfigurationPM theEntityPm)
        {
             
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDefaultAndConfigurations(theEntityPm.Id);
            DefaultAndConfigurationMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(string Id)
        { 
            this.Poco = entityRepository.GetSingleDefaultAndConfigurations(Id); 
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }

    }
}