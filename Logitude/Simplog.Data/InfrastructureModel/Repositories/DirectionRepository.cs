using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DirectionRepository:IRepository<Direction>
    {
        IWebFreightContext webFreightContext;

        public DirectionRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public DirectionRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DirectionRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<Direction> GetDirections()
        {
            return context.Directions;
        }

        public Direction GetSingleDirection(string id)
        {
            return (from a in context.Directions where a.Id == id select a).FirstOrDefault();
        }

        #region IRepository<Direction> Members

        public void Add(Direction entity)
        {
            context.Directions.Add(entity);
        }

        public void Remove(Direction entity)
        {
            context.Directions.Attach(entity);
            context.Directions.Remove(entity);
        }

        public void Update(Direction entity)
        {
            context.Directions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Direction> All()
        {
            return context.Directions.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        #endregion
        
        public List<Direction> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Direction GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}