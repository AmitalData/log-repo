using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class RoleTypeRepository:IRepository<RoleType>
    {


		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public RoleTypeRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public RoleTypeRepository()
		{
			globalContext = GlobalContext.GetContext();
		}

		public IQueryable<RoleType> GetRoleTypes()
        {
            return context.RoleTypes;
        }

        public RoleType GetSingleRoleType(string code)
        {
            return (from a in context.RoleTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(RoleType entity)
        {
			context.RoleTypes.Add(entity);
        }

        public void Remove(RoleType entity)
        {
			context.RoleTypes.Remove(entity);
        }

        public void Update(RoleType entity)
        {
			context.RoleTypes.Attach(entity);
			context.SetAsModified(entity);
        }

        public List<RoleType> All()
        {
            return context.RoleTypes.ToList();
        }


        public void SubmitChanges()
        {
			context.SaveChanges();
        }


        public List<RoleType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RoleType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}