using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class LeadSourceService
    {
        bool isNewEntity;
        private int tenant;
        public LeadSource Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private LeadSourcePM entityPm;
        private ICommonDataContext objectContext;
        private LeadSourceRepository entityRepository;

        public LeadSourceService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new LeadSourceRepository(objectContext);
        }

        public void Create(LeadSourcePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("LeadSource", tenant).ToString();
            this.Poco = new LeadSource();
            this.Poco.Id = this.entityPm.Id;

            LeadSourceTracing.Trace(entityPM, Poco, isNewEntity);    
            LeadSourceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(LeadSourcePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleLeadSource(entityPM.Id, entityPm.Tenant);

            LeadSourceTracing.Trace(entityPM, Poco, isNewEntity);            
            LeadSourceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }   
    }
}
