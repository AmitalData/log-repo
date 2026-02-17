using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class EntityLastActivityTypeRepository:IRepository<EntityLastActivityType>
    {
      IWebFreightContext webFreightContext;
        public EntityLastActivityTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public EntityLastActivityTypeRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public EntityLastActivityTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<EntityLastActivityType> GetEntityLastActivityTypes()
        {
            return context.EntityLastActivityTypes;
        }

        public EntityLastActivityType GetSingleEntityLastActivityType(string code)
        {
            return (from a in context.EntityLastActivityTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }
            

        public void Add(EntityLastActivityType entity)
        {
            context.EntityLastActivityTypes.Add(entity);
        }

        public void Remove(EntityLastActivityType entity)
        {
            context.EntityLastActivityTypes.Attach(entity);
            context.EntityLastActivityTypes.Remove(entity);
        }

        public void Update(EntityLastActivityType entity)
        {
            context.EntityLastActivityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EntityLastActivityType> All()
        {
            return context.EntityLastActivityTypes.ToList();
        }

        public IWebFreightContext context
        {
            get {return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<EntityLastActivityType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EntityLastActivityType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
