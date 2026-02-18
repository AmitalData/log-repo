using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public ContactTenantPM GetContactTenantForUser(string contactId, int tenantId)
        {
            SecurityUtility.AuthenticationOnTenant(tenantId);

            contactTenantRepository = new ContactTenantRepository(tenantId);
            ContactTenantQuery contactTenantQuery = new ContactTenantQuery(contactTenantRepository);
            return contactTenantQuery.GetContactTenantPMsByTenant(tenantId).Where(ct => ct.TenantId == tenantId && ct.ContactId == contactId).FirstOrDefault();
        }

        public List<ContactTenantRolePM> GetContactRoles(string contactId, int tenantId)
        {
            SecurityUtility.AuthenticationOnTenant(tenantId);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenantId);
            }
            contactTenantRepository = new ContactTenantRepository(objectContext);
            contactTenantRoleRepository = new ContactTenantRoleRepository(objectContext);
            ContactTenantQuery contactTenantQuery = new ContactTenantQuery(contactTenantRepository);
            ContactTenantRoleQuery contactTenantRoleQuery = new ContactTenantRoleQuery(contactTenantRoleRepository);
            try
            {
                ContactTenantPM contactTenant = contactTenantQuery.GetContactTenantPMsByTenant(tenantId).Where(ct => ct.TenantId == tenantId && ct.ContactId == contactId).FirstOrDefault();
                List<ContactTenantRolePM> roles = contactTenantRoleQuery.GetContactTenantRolePMsByTenant(tenantId).Where(ctr => ctr.ContactTenantId == contactTenant.Id).ToList<ContactTenantRolePM>();
                return roles;
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<ContactTenantRole> GetContactTenantRoles(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            contactTenantRoleRepository = new ContactTenantRoleRepository(tenant);
            return contactTenantRoleRepository.GetContactTenantRoles(0);
        }

        //public void MapContactTenantRoleContactTenantRolePM(ContactTenantRolePM contactTenantRolePm, ContactTenantRole contactTenantRole)
        //{
        //    contactTenantRole.ContactTenantId = contactTenantRolePm.ContactTenantId;
        //    contactTenantRole.RoleId = contactTenantRolePm.RoleId;
        //    contactTenantRole.Tenant = contactTenantRolePm.Tenant;
        //}

        public void InsertContactTenantRole(ContactTenantRolePM contactTenantRole)
        {

            ContactTenantRoleService service = new ContactTenantRoleService(objectContext, contactTenantRole.Tenant);
            service.Create(contactTenantRole);

            //contactTenantRoleRepository = new ContactTenantRoleRepository(contactTenantRole.Tenant);
            //contactTenantRole.Id = IdCounter.GetNumber("ContactTenantRole", contactTenantRole.Tenant).ToString();
            //ContactTenantRole newContactTenantRole = new ContactTenantRole();
            //newContactTenantRole.Id = contactTenantRole.Id;

            //MapContactTenantRoleContactTenantRolePM(contactTenantRole, newContactTenantRole);
            //contactTenantRoleRepository.Add(newContactTenantRole);
        }

        public void UpdateContactTenantRole(ContactTenantRolePM currentContactTenantRole)
        {
            ContactTenantRoleService service = new ContactTenantRoleService(objectContext, currentContactTenantRole.Tenant);
            service.Update(currentContactTenantRole);


            //contactTenantRoleRepository = new ContactTenantRoleRepository(currentContactTenantRole.Tenant);
            //ContactTenantRole entity = contactTenantRoleRepository.GetSingleContactTenantRole(currentContactTenantRole.Id, currentContactTenantRole.Tenant);
            //MapContactTenantRoleContactTenantRolePM(currentContactTenantRole, entity);
            //contactTenantRoleRepository.Update(entity);
        }

        public void DeleteContactTenantRole(ContactTenantRolePM contactTenantRole)
        {
            contactTenantRoleRepository = new ContactTenantRoleRepository(contactTenantRole.Tenant);
            ContactTenantRole entity = contactTenantRoleRepository.GetSingleContactTenantRole(contactTenantRole.Id, contactTenantRole.Tenant);
            contactTenantRoleRepository.Remove(entity);
        }

        public List<ContactTenantRolePM> GetContactTenantRolesForContactTenant(string contactTenantId, int tenant)
        {
            ContactTenantRoleQuery contactTenantRoleQuery = new ContactTenantRoleQuery();
            List<ContactTenantRolePM> roles = contactTenantRoleQuery.GetContactTenantRolesForContactTenant(contactTenantId, tenant);
            return roles;
        }
    }
}