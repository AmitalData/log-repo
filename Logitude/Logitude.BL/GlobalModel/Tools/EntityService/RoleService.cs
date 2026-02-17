using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class RoleService
    {
        bool isNewEntity;
        private int tenant;
        public Role Poco { get; set; }
        private RolePM entityPM;
        private IGlobalContext objectContext;
		private ICommonDataContext commonDataContext;
		private RoleRepository entityRepository;
        private ContactTenantRepository contactTenantsRepository;
        private ContactTenantRoleRepository contactTenantRolesRepository;        
        public RoleService(IGlobalContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
			this.commonDataContext = CommonDataContext.GetContext(tenant);
			this.entityRepository = new RoleRepository(objectContext);
            this.contactTenantsRepository = new ContactTenantRepository(this.commonDataContext);
            this.contactTenantRolesRepository = new ContactTenantRoleRepository(this.commonDataContext);
        }

        public void Create(RolePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("Role", tenant).ToString();

            if (entityPM.ParentRoleId != null)
            {
                entityPM.Code = entityPM.Id;
            }

            this.Poco = new Role()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant,
            };

            RoleValidating.Validate(entityPM, isNewEntity, entityRepository);
            RoleTracing.Trace(entityPM, Poco, isNewEntity);
            RoleMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            if (entityPM.Added)
            {
                this.AddRoleUser();
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Role");
        }

        public void Update(RolePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleRole(entityPM.Id, entityPM.Tenant);


            RoleValidating.Validate(entityPM, isNewEntity, entityRepository);
            RoleTracing.Trace(entityPM, Poco, isNewEntity);
            RoleMapping.MapEntity(entityPM, Poco, isNewEntity);

            if (entityPM.UserId != null)
            {
                if (entityPM.Added)
                {
                    this.AddRoleUser();
                }

                if (entityPM.Removed)
                {
                    ContactTenant myContactTenant = contactTenantsRepository.GetContactTenantForContactId(entityPM.UserId, tenant);
                    List<ContactTenantRole> myContactTenantRolesList = contactTenantRolesRepository.GetContactTenantRolesListByRoleAndContactTenant(entityPM.Id, myContactTenant.Id, tenant).ToList();

                    if (myContactTenantRolesList.Count > 0)
                    {
                        foreach (ContactTenantRole item in myContactTenantRolesList)
                        {
                            contactTenantRolesRepository.Remove(item);
                        }

                        contactTenantRolesRepository.SubmitChanges();
                    }
                }
            }

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Role");
        }

        private void AddRoleUser()
        {
            if (!string.IsNullOrEmpty(entityPM.UserId))
            {
                ContactTenant myContactTenant = contactTenantsRepository.GetContactTenantForContactId(entityPM.UserId, tenant);
                ContactTenantRole myContactTenantRole = contactTenantRolesRepository.GetContactTenantRoleByRoleIdAndContactTenant(entityPM.Id, myContactTenant.Id, tenant);

                if (myContactTenantRole == null)
                {
                    myContactTenantRole = new ContactTenantRole()
                    {
                        ContactTenantId = myContactTenant.Id,
                        RoleId = entityPM.Id,
                        Id = IdCounter.GetNumber("ContactTenantRole", entityPM.CurrentTenant).ToString(),
                        Tenant = tenant
                    };

                    contactTenantRolesRepository.Add(myContactTenantRole);
                    contactTenantRolesRepository.SubmitChanges();
                }
            }
        }

    }
}
