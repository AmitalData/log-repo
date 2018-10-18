using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class RestrictionService
    {
        bool isNewEntity;
        private int tenant;
        public Restriction Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private RestrictionPM entityPM;
        private ICommonDataContext objectContext;
        private RestrictionRepository entityRepository;
        private ContactTenantRepository contactTenantsRepository;
        private ContactTenantQuery contactTenantQuery;
        public RestrictionService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new RestrictionRepository(objectContext);
        }

        public void Create(RestrictionPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Restriction", theEntityPm.Tenant).ToString();
            
            this.Poco = new Restriction();
            this.Poco.Id = this.entityPM.Id;
             contactTenantsRepository = new ContactTenantRepository(objectContext);
             contactTenantQuery = new ContactTenantQuery(contactTenantsRepository);
            ContactTenantPM contactTenant = contactTenantQuery.GetContactTenantForUser(theEntityPm.UserId, theEntityPm.Tenant);
            theEntityPm.ContactTenantId = contactTenant.Id;
            RestrictionValidating.Validate(theEntityPm);
            RestrictionTracing.Trace(theEntityPm, Poco, isNewEntity);
            RestrictionMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(RestrictionPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleRestriction(theEntityPm.Id);

            RestrictionValidating.Validate(theEntityPm);
            RestrictionTracing.Trace(theEntityPm, Poco, isNewEntity);
            RestrictionMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
