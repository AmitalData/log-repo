using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Queries
{ 
    public class RoleQuery
    {
        IRepository<Role> repository;
        IAmitalCloudContext context;
        public RoleQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = new Repository<Role>(context);
        }

        public RoleQuery(Repository<Role> roleRepository, int tenant) 
        {
            repository = roleRepository;
            context = AmitalCloudContext.GetContext(tenant);
        }

        public RolePM GetSinglePM(string id, int tenant)
        {
            var role = (from a in context.Roles
                        where a.Id == id
                        select new RolePM()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Tenant = a.Tenant,
                            Code = a.Code,
                            RoleTypeCode = a.RoleTypeCode,
                            CurrentTenant = tenant,
                            Description = a.Description,
                            ParentRoleId = a.ParentRoleId,
                            IsCustomRole = a.IsCustomRole,
                            SearchFields = a.SearchFields,
                            Inactive = a.Inactive,
                        }).FirstOrDefault();

            return role;
        }

        public RolePM GetSinglePMByName(string name, int tenant)
        {
            var role = (from a in context.Roles
                        where a.Name == name && (a.Tenant == tenant || a.Tenant == 0)
                        select new RolePM()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Tenant = a.Tenant,
                            Code = a.Code,
                            RoleTypeCode = a.RoleTypeCode,
                            CurrentTenant = tenant,
                            Description = a.Description,
                            ParentRoleId = a.ParentRoleId,
                            IsCustomRole = a.IsCustomRole,
                            SearchFields = a.SearchFields,
                            Inactive = a.Inactive,
                        }).FirstOrDefault();

            return role;
        }

        public RolePM GetSinglePMByCode(string code, int tenant)
        {
            var role = (from a in context.Roles
                        where a.Code == code && (a.Tenant == tenant || a.Tenant == 0)
                        select new RolePM()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Tenant = a.Tenant,
                            Code = a.Code,
                            RoleTypeCode = a.RoleTypeCode,
                            CurrentTenant = tenant,
                            Description = a.Description,
                            ParentRoleId = a.ParentRoleId,
                            IsCustomRole = a.IsCustomRole,
                            SearchFields = a.SearchFields,
                            Inactive = a.Inactive,
                        }).FirstOrDefault();

            return role;
        }

        public List<RolePM> GetRolesByUser(string userId, int tenant)
        {
            List<RolePM> roles = null;
            if (userId != null)
            {
                ContactTenant contacttenant = (from a in context.ContactTenants
                                               where a.ContactId == userId && (a.TenantId == tenant)
                                               select a).FirstOrDefault();
                if (contacttenant == null)
                {
                    contacttenant = (from a in context.ContactTenants
                                     where a.ContactId == userId && (a.TenantId == 0)
                                     select a).FirstOrDefault();
                }

                if (contacttenant != null)
                {

                    List<ContactTenantRole> contactTenantRoles = (from a in context.ContactTenantRoles
                                                                  where a.ContactTenantId == contacttenant.Id && a.Tenant == tenant
                                                                  select a).ToList();

                    roles = (from a in context.Roles
                             where a.Tenant == tenant || a.Tenant == 0
                             select new RolePM()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Tenant = a.Tenant,
                                 Code = a.Code,
                                 RoleTypeCode = a.RoleTypeCode,
                                 CurrentTenant = tenant,
                                 Description = a.Description,
                                 ParentRoleId = a.ParentRoleId,
                                 IsCustomRole = a.IsCustomRole,
                                 SearchFields = a.SearchFields,
                                 Inactive = a.Inactive,
                             }).ToList();

                    foreach (ContactTenantRole contacttenantrole in contactTenantRoles)
                    {
                        RolePM role = (from a in roles
                                       where a.Id == contacttenantrole.RoleId
                                       select a).FirstOrDefault();
                        role.Exists = true;
                        role.UserId = contacttenant.ContactId;
                    }
                }
            }

            else
            {
                roles = (from a in context.Roles
                         where a.Tenant == tenant || a.Tenant == 0
                         select new RolePM()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Tenant = a.Tenant,
                             Code = a.Code,
                             RoleTypeCode = a.RoleTypeCode,
                             CurrentTenant = tenant,
                             Description = a.Description,
                             ParentRoleId = a.ParentRoleId,
                             IsCustomRole = a.IsCustomRole,
                             SearchFields = a.SearchFields,
                             Inactive = a.Inactive,
                         }).ToList();
            }

            return roles;
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
                         select new RolePM()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Tenant = a.Tenant,
                             Code = a.Code,
                             RoleTypeCode = a.RoleTypeCode,
                             CurrentTenant = tenant,
                             Description = a.Description,
                             ParentRoleId = a.ParentRoleId,
                             IsCustomRole = a.IsCustomRole,
                             SearchFields = a.SearchFields,
                             Inactive = a.Inactive,
                         }).ToList();

                foreach (ContactTenantRole contacttenantrole in contactTenantRoles)
                {
                    RolePM role = (from a in roles
                                   where a.Id == contacttenantrole.RoleId
                                   select a).FirstOrDefault();
                    role.Exists = true;
                    role.UserId = contacttenant.ContactId;
                }

            }
            else
            {
                roles = (from a in context.Roles
                         where a.Tenant == 0
                         select new RolePM()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Tenant = a.Tenant,
                             Code = a.Code,
                             RoleTypeCode = a.RoleTypeCode,
                             CurrentTenant = tenant,
                             Description = a.Description,
                             ParentRoleId = a.ParentRoleId,
                             IsCustomRole = a.IsCustomRole,
                             SearchFields = a.SearchFields,
                             Inactive = a.Inactive,
                         }).ToList();
            }
            return roles.Where(d => d.Exists == true).ToList();
        }

        public List<RolePM> GetRolesByIds(List<string> myRolesIds)
        {
            List<RolePM> myResult = new List<RolePM>();

            if (myRolesIds.Count > 0)
            {
                myResult = (from a in context.Roles
                            where myRolesIds.Contains(a.Id)
                            select new RolePM()
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Tenant = a.Tenant,
                                Code = a.Code,
                                RoleTypeCode = a.RoleTypeCode,                                
                                Description = a.Description,
                                ParentRoleId = a.ParentRoleId,
                                IsCustomRole = a.IsCustomRole,
                                SearchFields = a.SearchFields,
                                Inactive = a.Inactive,
                            }).ToList();
            }

            return myResult;
        }
        public List<RolePM> GetCustomRolesByIds(List<string> myRolesIds)
        {
            List<RolePM> myResult = new List<RolePM>();

            if (myRolesIds.Count > 0)
            {
                myResult = (from a in context.Roles
                            where myRolesIds.Contains(a.Id) && a.IsCustomRole == true
                            select new RolePM()
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Tenant = a.Tenant,
                                Code = a.Code,
                                RoleTypeCode = a.RoleTypeCode,
                                Description = a.Description,
                                ParentRoleId = a.ParentRoleId,
                                IsCustomRole = a.IsCustomRole,
                                SearchFields = a.SearchFields,
                                Inactive = a.Inactive,
                            }).ToList();
            }

            return myResult;
        }

        public IQueryable<RolePM> GetRolePMsByTenant(int tenant)
        {
            IQueryable<RolePM> roles = from a in context.Roles
                                       where a.Tenant == tenant || a.Tenant == 0
                                       select new RolePM()
                                       {
                                           Id = a.Id,
                                           Name = a.Name,
                                           Tenant = a.Tenant,
                                           Code = a.Code,
                                           RoleTypeCode = a.RoleTypeCode,
                                           CurrentTenant = tenant,
                                           Description = a.Description,
                                           ParentRoleId = a.ParentRoleId,
                                           IsCustomRole = a.IsCustomRole,
                                           SearchFields = a.SearchFields,
                                           Inactive = a.Inactive,
                                       };
            return roles;
        }

        public IQueryable<RoleList> GetIQueryableEntityList(IQueryable<Role> iQueryable)
        {
            IQueryable<RoleList> result = from a in iQueryable
                                          select new RoleList()
                                          {
                                              Id = a.Id,
                                              Name = a.Name,
                                              Tenant = a.Tenant,
                                              Code = a.Code,
                                              RoleTypeCode = a.RoleTypeCode,
                                              Description = a.Description,
                                              ParentRoleId = a.ParentRoleId,
                                              IsCustomRole = a.IsCustomRole,
                                              SearchFields = a.SearchFields,
                                              Inactive = a.Inactive,
                                          };
            return result;
        }


        public IQueryable<RoleList> GetRoleListsByTenant(int tenant)
        {
            IQueryable<RoleList> roles = from a in context.Roles
                                       where a.Tenant == tenant || a.Tenant == 0
                                       select new RoleList()
                                       {
                                           Id = a.Id,
                                           Name = a.Name,
                                           Tenant = a.Tenant,
                                           Code = a.Code,
                                           RoleTypeCode = a.RoleTypeCode,
                                           Description = a.Description,
                                           ParentRoleId = a.ParentRoleId,
                                           IsCustomRole = a.IsCustomRole,
                                           SearchFields = a.SearchFields,
                                           Inactive = a.Inactive,
                                       };
            return roles;
        }
    }
}
