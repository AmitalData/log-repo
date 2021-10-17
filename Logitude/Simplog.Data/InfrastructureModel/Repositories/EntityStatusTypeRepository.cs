using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class EntityStatusTypeRepository : IRepository<EntityStatusType>
    {
        IWebFreightContext webFreightContext;

        public EntityStatusTypeRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public EntityStatusTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public EntityStatusTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(EntityStatusType entity)
        {
            context.EntityStatusTypes.Add(entity);
        }

        public void Remove(EntityStatusType entity)
        {
            context.EntityStatusTypes.Attach(entity);
            context.EntityStatusTypes.Remove(entity);
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public void Update(EntityStatusType entity)
        {
            context.EntityStatusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }
        public EntityStatusType GetSingleEntityStatusType(string code)
        {
            return (from a in context.EntityStatusTypes where a.Code == code select a).FirstOrDefault();
        }

        public List<EntityStatusType> GetAll()
        {
            return context.EntityStatusTypes.ToList();
        }

        public IQueryable<EntityStatusType> GetEntityStatusTypes()
        {
            return webFreightContext.EntityStatusTypes.OrderBy(d => d.Code);
        }

        List<EntityStatusType> IRepository<EntityStatusType>.All()
        {
            throw new NotImplementedException();
        }

        List<EntityStatusType> IRepository<EntityStatusType>.GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        EntityStatusType IRepository<EntityStatusType>.GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}