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
    public class ContactTenantService
    {
        bool isNewEntity;
        private int tenant;
        public ContactTenant Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ContactTenantPM entityPm;
        private ICommonDataContext objectContext;
        private ContactTenantRepository entityRepository;
        public ContactTenantService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ContactTenantRepository(objectContext);
        }

        public void Create(ContactTenantPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("ContactTenant", tenant).ToString();
            this.Poco = new ContactTenant();
            this.Poco.Id = this.entityPm.Id;

            ContactTenantValidating.Validate(entityPM);
            ContactTenantTracing.Trace(entityPM, Poco, isNewEntity);
            ContactTenantMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ContactTenantPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleContactTenant(entityPM.Id, entityPm.TenantId);

            ContactTenantValidating.Validate(entityPM);
            ContactTenantTracing.Trace(entityPM, Poco, isNewEntity);
            ContactTenantMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
