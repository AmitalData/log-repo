using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class RoleRepository:IRepository<Role>
    {

		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public RoleRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public RoleRepository()
		{
			globalContext = GlobalContext.GetContext();
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