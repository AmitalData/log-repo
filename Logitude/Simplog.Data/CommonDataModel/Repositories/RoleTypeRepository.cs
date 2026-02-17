using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RoleTypeRepository:IRepository<RoleType>
    {

        ICommonDataContext commonDataContext;
        public RoleTypeRepository()
        {
            commonDataContext = new CommonDataContext();

        }
        public RoleTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }
        public RoleTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
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
            commonDataContext.RoleTypes.Add(entity);
        }

        public void Remove(RoleType entity)
        {
            commonDataContext.RoleTypes.Remove(entity);
        }

        public void Update(RoleType entity)
        {
            commonDataContext.RoleTypes.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<RoleType> All()
        {
            return context.RoleTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
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