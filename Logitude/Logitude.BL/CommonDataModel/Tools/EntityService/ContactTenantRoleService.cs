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
    public class ContactTenantRoleService
    {
        bool isNewEntity;
        private int tenant;
        public ContactTenantRole Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ContactTenantRolePM entityPm;
        private ICommonDataContext objectContext;
        private ContactTenantRoleRepository entityRepository;
        public ContactTenantRoleService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ContactTenantRoleRepository(objectContext);
        }

        public void Create(ContactTenantRolePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("ContactTenantRole", tenant).ToString();
            this.Poco = new ContactTenantRole();
            this.Poco.Id = this.entityPm.Id;

            ContactTenantRoleValidating.Validate(entityPM);
            ContactTenantRoleTracing.Trace(entityPM, Poco, isNewEntity);
            ContactTenantRoleMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ContactTenantRolePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleContactTenantRole(entityPM.Id , entityPm.Tenant);

            ContactTenantRoleValidating.Validate(entityPM);
            ContactTenantRoleTracing.Trace(entityPM, Poco, isNewEntity);
            ContactTenantRoleMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
