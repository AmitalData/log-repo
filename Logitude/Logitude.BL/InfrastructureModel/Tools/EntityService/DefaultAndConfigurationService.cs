using System;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.Helpers;

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
        private DefaultAndConfigurationRepository entityRepository;
        public DefaultAndConfigurationService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DefaultAndConfigurationRepository(objectContext);
        }

        public void Create(DefaultAndConfigurationPM theEntityPm)
        {
            
            this.entityPM = theEntityPm;
            this.Poco = new DefaultAndConfiguration();
            DefaultAndConfigurationMapping.MapEntity(theEntityPm, Poco, true);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            DefaultService.Instance.ClearCache(entityPM.Tenant, entityPM.SetKey);
        }

        public void Update(DefaultAndConfigurationPM theEntityPm)
        {
             
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDefaultAndConfiguration(theEntityPm.Id);
            DefaultAndConfigurationMapping.MapEntity(theEntityPm, Poco, false);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            DefaultService.Instance.ClearCache(entityPM.Tenant, entityPM.SetKey, entityPM.AdditionalKey);
        }

        public void Delete(string Id)
        { 
            this.Poco = entityRepository.GetSingleDefaultAndConfiguration(Id); 
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
            DefaultService.Instance.ClearCache(Poco.Tenant, entityPM.SetKey, entityPM.AdditionalKey);

        }

    }
}