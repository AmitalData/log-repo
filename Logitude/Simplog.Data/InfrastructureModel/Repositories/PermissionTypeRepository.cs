using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class PermissionTypeRepository:IRepository<PermissionType>
    {

        IWebFreightContext webFreightContext;
        public PermissionTypeRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public PermissionTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public PermissionTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<PermissionType> GetPermissionTypes()
        {
            return context.PermissionTypes;
        }

        public PermissionType GetSinglePermissionType(string code)
        {
            return (from a in context.PermissionTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(PermissionType entity)
        {
            webFreightContext.PermissionTypes.Add(entity);
        }

        public void Remove(PermissionType entity)
        {
            webFreightContext.PermissionTypes.Remove(entity);
        }

        public void Update(PermissionType entity)
        {
            webFreightContext.PermissionTypes.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<PermissionType> All()
        {
            return context.PermissionTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }


        public List<PermissionType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PermissionType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}