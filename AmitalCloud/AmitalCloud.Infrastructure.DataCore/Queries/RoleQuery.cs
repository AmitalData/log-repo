using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class RoleQuery
    {
        IRepository<Role> repository;
        IAmitalCloudContext context;
        public RoleQuery(int tenant) : this(AmitalCloudContext.GetContext(tenant))
        {
        }

        public RoleQuery(Repository<Role> roleRepository, int tenant)
        {
            repository = roleRepository;
        }

        public RoleQuery(IAmitalCloudContext amitalCloudContext) : this(new Repository<Role>(amitalCloudContext), amitalCloudContext.Tenant)
        {
            context = amitalCloudContext;
        }

        public List<RolePM> GetRolesForContact(string contactid, int tenant)
        {
            List<RolePM> roles = null;
            if (contactid != null)
            {
                ContactTenant contacttenant = (from a in context.ContactTenants
                                               where a.ContactId == contactid && (a.TenantId == tenant)
                                               select a).FirstOrDefault();
                if (contacttenant == null)
                {
                    throw new Exception($"contacttenant not exist in DB ({contactid})");
                }
                List<ContactTenantRole> contactTenantRoles = (from a in context.ContactTenantRoles
                                                              where a.ContactTenantId == contacttenant.Id && a.Tenant == tenant
                                                              select a).ToList();

                roles = (from a in context.Roles
                         where a.Tenant == tenant || a.Tenant == 0
                         select new RolePM(a)).ToList();

                foreach (ContactTenantRole contacttenantrole in contactTenantRoles)
                {
                    RolePM role = (from a in roles
                                   where a.Id == contacttenantrole.RoleId
                                   select a).FirstOrDefault();
                    //role.Exists = true;
                    //role.UserId = contacttenant.ContactId;
                }

            }
            else
            {
                roles = (from a in context.Roles
                         where a.Tenant == 0 
                         select new RolePM(a)).ToList();
            }
            return roles; //.Where(d => d.Exists == true).ToList();
        }
        private List<RolePM> GetCustomRolesByIds(List<string> myRolesIds)
        {
            List<RolePM> myResult = new List<RolePM>();

            if (myRolesIds.Count > 0)
            {
                myResult = (from a in context.Roles
                            where myRolesIds.Contains(a.Id) && a.IsCustomRole == true
                            select new RolePM(a)).ToList();
            }

            return myResult;
        }
    }
}
