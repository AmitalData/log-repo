using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ContactTenantRoleQuery
    {
        ContactTenantRoleRepository repository;
        public ContactTenantRoleQuery()
        {
               repository = new ContactTenantRoleRepository(); 
        }

        public ContactTenantRoleQuery(int tenant)
        {
            repository = new ContactTenantRoleRepository(tenant);
        }

        public ContactTenantRoleQuery(ContactTenantRoleRepository contactTenantRoleRepository)
        {
            repository = contactTenantRoleRepository;
        }

        public ContactTenantRolePM GetSinglePM(string id, int tenant)
        {
            var contactTenantRole = (from a in repository.context.ContactTenantRoles
                                     where a.Id == id && a.Tenant == tenant
                                     select new ContactTenantRolePM()
                                     {
                                         ContactTenantId = a.ContactTenantId,
                                         Id = a.Id,
                                         RoleId = a.RoleId,
                                         Tenant = a.Tenant,
                                     }).FirstOrDefault();
            return contactTenantRole;
        }

        public IQueryable<ContactTenantRolePM> GetContactTenantRolePMsByTenant(int tenant)
        {
            IQueryable<ContactTenantRolePM> contactTenantRole = from a in repository.context.ContactTenantRoles
                                                                where a.Tenant == tenant
                                                                select new ContactTenantRolePM()
                                                                {
                                                                    ContactTenantId = a.ContactTenantId,
                                                                    Id = a.Id,
                                                                    RoleId = a.RoleId,
                                                                    Tenant = a.Tenant,
                                                                };
            return contactTenantRole;
        }

        public List<ContactTenantRolePM> GetContactTenantRolesForContactTenant(string contactTenantId, int tenant)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("  GetContactTenantRolesForContactTenant contactTenantId:" + contactTenantId+"tenant"+tenant);
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" before GetContactTenantRolesForContactTenant  repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);

            var contactTenantRole = (from a in repository.context.ContactTenantRoles
                                                           where a.ContactTenantId == contactTenantId && a.Tenant == tenant
                                                           select new ContactTenantRolePM()
                                                           {
                                                               ContactTenantId = a.ContactTenantId,
                                                               Id = a.Id,
                                                               RoleId = a.RoleId,
                                                               Tenant = a.Tenant,
                                                           });
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo(" after GetContactTenantRolesForContactTenant  repository.context.GetConnection().Database" + repository.context.GetConnection()?.Database);
            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("  GetContactTenantRolesForContactTenant  contactTenantRole" + contactTenantRole?.Count());

            if (contactTenantRole==null)
            {
                return null;
            }
            return contactTenantRole.ToList();

        }
    }
}
