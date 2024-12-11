using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RoleRepository:IRepository<Role>
    {
        ICommonDataContext commonDataContext;
        public RoleRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public RoleRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
        public RoleRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public IQueryable<Role> GetRoles(int tenant)
        {
            return (from d in context.Roles where (d.Tenant == tenant || d.Tenant == 0) select d);
        }

        public Role GetSingleRole(string id, int tenant)
        {
            return (from d in context.Roles where d.Id == id select d).FirstOrDefault();
        }

        public Role GetSingleByName(string name, int tenant)
        {
            var role = (from a in context.Roles
                        where a.Name == name && (a.Tenant == tenant || a.Tenant == 0)
                        select a).FirstOrDefault();
            return role;
        }

        public Role GetSingleByCode(string code, int tenant)
        {

            var role = (from a in context.Roles
                        where a.Code == code && (a.Tenant == tenant || a.Tenant == 0)
                        select a).FirstOrDefault();
            return role; 
        }

        public bool CheckIfLastUserRole(string userId, string roleId, int tenant)
        {
            bool result = false;

            ContactTenant contactTenant = (from a in context.ContactTenants where a.ContactId == userId && (a.TenantId == tenant) select a).FirstOrDefault();
            
            if (contactTenant != null)
            {
                List<ContactTenantRole> contactTenantRoles = (from a in context.ContactTenantRoles where a.ContactTenantId == contactTenant.Id && a.Tenant == tenant select a).ToList();

                if (contactTenantRoles.Count == 1)
                {
                    result = true;

                    //string ExistingRoleId = contactTenantRoles.First().RoleId;
                    //if (roleId == ExistingRoleId)
                    //{
                    //    Result = true;
                    //}

                }
            }

            return result;
        }

        public List<string> GetUserRolesIds(string userId, int tenant)
        {
            List<string> myResult = new List<string>();

            ContactTenant contactTenant = (from a in context.ContactTenants where a.ContactId == userId && a.TenantId == tenant select a).FirstOrDefault();
            if (contactTenant != null)
            {
                myResult = (from a in context.ContactTenantRoles where a.ContactTenantId == contactTenant.Id && a.Tenant == tenant select a.RoleId).ToList();
            }

            return myResult;
        }

        public void Add(Role entity)
        {
            this.context.Roles.Add(entity);
        }

        public void Remove(Role entity)
        {
            this.context.Roles.Attach(entity);
            this.context.Roles.Remove(entity);
        }

        public void Update(Role entity)
        {
            try
            {
                this.context.Roles.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<Role> All()
        {
            return this.context.Roles.ToList<Role>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<Role> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Role GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<Role> GetUserRolesByIds(List<string> myRolesIds, int tenant)
        {
            List<Role> myResult = new List<Role>();

            if (myRolesIds.Count > 0)
            {
                myResult = (from a in context.Roles
                            where myRolesIds.Contains(a.Id) && a.Tenant == tenant || a.Tenant == 0
                            select a).ToList();
            }

            return myResult;
        }
    }
}