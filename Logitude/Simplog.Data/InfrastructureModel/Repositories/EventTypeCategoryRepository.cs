using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class EventTypeCategoryRepository : IRepository<EventTypeCategory>
    {
        IWebFreightContext webFreightContext;

        public EventTypeCategoryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public EventTypeCategoryRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public EventTypeCategoryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<EventTypeCategory> GetEventTypeCategories()
        {
            return context.EventTypeCategories;
        }

        public EventTypeCategory GetSingleEventTypeCategory(string code)
        {
            return (from a in context.EventTypeCategories
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(EventTypeCategory entity)
        {
            context.EventTypeCategories.Add(entity);
        }

        public void Remove(EventTypeCategory entity)
        {
            context.EventTypeCategories.Attach(entity);
            context.EventTypeCategories.Remove(entity);
        }

        public void Update(EventTypeCategory entity)
        {
            context.EventTypeCategories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EventTypeCategory> All()
        {
            return context.EventTypeCategories.ToList();
        }

        public List<EventTypeCategory> GetAll()
        {
            return context.EventTypeCategories.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<EventTypeCategory> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public EventTypeCategory GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}