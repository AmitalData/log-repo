using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ContactTenantRoleRepository:IRepository<ContactTenantRole>
    {
        ICommonDataContext commonDataContext;
        public ContactTenantRoleRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ContactTenantRoleRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ContactTenantRoleRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ContactTenantRole GetContactTenantRoleByRoleIdAndContactTenant(string roleId,string contactTenantId, int tenant)
        {
            ContactTenantRole role= (from a in context.ContactTenantRoles
                    where a.RoleId == roleId && a.Tenant == tenant&&a.ContactTenantId==contactTenantId
                    select a).FirstOrDefault();
            return role;
        }

        public IQueryable<ContactTenantRole> GetContactTenantRolesListByRoleAndContactTenant(string roleId, string contactTenantId, int tenant)
        {
            return (from a in context.ContactTenantRoles where a.RoleId == roleId && a.Tenant == tenant && a.ContactTenantId == contactTenantId select a);
        }


        public IQueryable<ContactTenantRole> GetContactTenantRolesListByContactAndTenant(string contactTenantId, int tenant)
        {
            return (from a in context.ContactTenantRoles where  a.Tenant == tenant && a.ContactTenantId == contactTenantId select a);
        }


        public bool CheckIfLastUserRole(string roleId, string contactTenantId, int tenant)
        {
            return (from a in context.ContactTenantRoles where a.RoleId != roleId && a.ContactTenantId == contactTenantId && a.Tenant == tenant select a).Any();
        }

        public IQueryable<ContactTenantRole> GetContactTenantRolesByRoleId(string roleId, int tenant)
        {
            return (from a in context.ContactTenantRoles where a.RoleId == roleId && a.Tenant == tenant select a);
        }

        public IQueryable<ContactTenantRole> GetContactTenantRoles(int tenant)
        {
            return (from record in context.ContactTenantRoles where record.Tenant == tenant select record);
        }

        public ContactTenantRole GetSingleContactTenantRole(string id, int tenant)
        {
            return (from record in context.ContactTenantRoles where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
     
        public void Add(ContactTenantRole entity)
        {
            this.context.ContactTenantRoles.Add(entity);
        }

        public void Remove(ContactTenantRole entity)
        {
            this.context.ContactTenantRoles.Attach(entity);
            this.context.ContactTenantRoles.Remove(entity);
        }

        public void Update(ContactTenantRole entity)
        {
            this.context.ContactTenantRoles.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<ContactTenantRole> All()
        {
            return this.context.ContactTenantRoles.ToList<ContactTenantRole>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<ContactTenantRole> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContactTenantRole GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}