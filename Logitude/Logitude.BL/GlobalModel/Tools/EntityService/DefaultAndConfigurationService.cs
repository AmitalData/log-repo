using Logitude.BL.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.Tools.DataMapping;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class DefaultAndConfigurationService
    {        
        public DefaultAndConfiguration Poco { get; set; }

        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DefaultAndConfigurationPM entityPM;
        private IGlobalContext objectContext;
        private DefaultAndConfigurationRepository entityRepository;
        public DefaultAndConfigurationService(IGlobalContext objectContext, int tenant)
        {
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