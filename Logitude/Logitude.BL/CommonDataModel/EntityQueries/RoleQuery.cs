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
    public class RoleQuery
    {
        RoleRepository repository;
        public RoleQuery(int tenant)
        {
            repository = new RoleRepository(tenant);
        }

        public RoleQuery(RoleRepository roleRepository)
        {
            repository = roleRepository;
        }

        public RolePM GetSinglePM(string id, int tenant)
        {
            var role = (from a in repository.context.Roles
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
            var role = (from a in repository.context.Roles
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
            var role = (from a in repository.context.Roles
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
                ContactTenant contacttenant = (from a in repository.context.ContactTenants
                                               where a.ContactId == userId && (a.TenantId == tenant)
                                               select a).FirstOrDefault();
                if (contacttenant == null)
                {
                    contacttenant = (from a in repository.context.ContactTenants
                                     where a.ContactId == userId && (a.TenantId == 0)
                                     select a).FirstOrDefault();
                }

                if (contacttenant != null)
                {

                    List<ContactTenantRole> contactTenantRoles = (from a in repository.context.ContactTenantRoles
                                                                  where a.ContactTenantId == contacttenant.Id && a.Tenant == tenant
                                                                  select a).ToList();

                    roles = (from a in repository.context.Roles
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
                roles = (from a in repository.context.Roles
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
                ContactTenant contacttenant = (from a in repository.context.ContactTenants
                                               where a.ContactId == contactid && (a.TenantId == tenant)
                                               select a).FirstOrDefault();
                if (contacttenant == null)
                {
                    throw new Exception($"contacttenant not exist in DB ({contactid})");
                }
                List<ContactTenantRole> contactTenantRoles = (from a in repository.context.ContactTenantRoles
                                                              where a.ContactTenantId == contacttenant.Id && a.Tenant == tenant
                                                              select a).ToList();

                roles = (from a in repository.context.Roles
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
                roles = (from a in repository.context.Roles
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
                myResult = (from a in repository.context.Roles
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
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo("GetCustomRolesByIds myRolesIds: " + string.Join(", ", myRolesIds));
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"Before GetCustomRolesByIds repository.context.GetConnection().Database: {repository.context.GetConnection()?.Database}");

                myResult = (from a in repository.context.Roles
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
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"GetCustomRolesByIds myResult: {string.Join(", ", myResult?.Select(a => a.Id))}");
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"After GetCustomRolesByIds repository.context.GetConnection().Database: {repository.context.GetConnection()?.Database}");

            }


            return myResult;
        }

        public IQueryable<RolePM> GetRolePMsByTenant(int tenant)
        {
            IQueryable<RolePM> roles = from a in repository.context.Roles
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
            IQueryable<RoleList> roles = from a in repository.context.Roles
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
