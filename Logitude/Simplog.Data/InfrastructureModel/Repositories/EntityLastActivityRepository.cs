using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class EntityLastActivityRepository:IRepository<EntityLastActivity>
    {

        IWebFreightContext webFreightContext;
        public EntityLastActivityRepository()
        {
            webFreightContext = new WebFreightContext();

        }

        public EntityLastActivityRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public EntityLastActivityRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<EntityLastActivity> GetEntityLastActivitiesForUser(string userId, int tenant)
        {
            DateTime present = DateTime.Now.Date;
            IQueryable<EntityLastActivity> entityLastAccesses = from a in context.EntityLastActivities
                                                              where a.UserId == userId && a.Tenant == tenant && a.ActivityDate < present
                                                              select a;
            return entityLastAccesses;
        }

        public EntityLastActivity GetSingleEntityActivity(string id)
        {
            return (from a in context.EntityLastActivities
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public List<EntityLastActivity> GetEntityLastActivitiesForTenant(int tenant)
        {
            return (from a in context.EntityLastActivities
                    where a.Tenant == tenant
                    select a).ToList();
        }

        public List<EntityLastActivity> GetTopEntityLastActivities(int tenant, string userId, string objectTableId)
        {
            IQueryable<EntityLastActivity> lastActivitiesQuery = null;
            if (tenant != 65)
            {
                lastActivitiesQuery = (from a in context.EntityLastActivities.Include("ActivityType").Include("User.Contact")
                                       where a.Tenant == tenant && a.UserId == userId && a.ObjectTableId == objectTableId
                                       select a).OrderByDescending(d => d.ActivityDate);
            }
            else
            {
                lastActivitiesQuery = (from a in context.EntityLastActivities.Include("ActivityType").Include("User.Contact")
                                       where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                       select a).OrderByDescending(d => d.ActivityDate);
            }
            var lastActivitiesGroup = (from a in lastActivitiesQuery
                                       group a by new { EntityId = a.EntityId, ObjectTableId = a.ObjectTableId, UserId = a.UserId, Tenant = a.Tenant } into g
                                       select new { EntityId = g.Key.EntityId, ObjectTableId = g.Key.ObjectTableId, UserId = g.Key.UserId, Tenant = g.Key.Tenant, ActivityDate = g.Max(d => d.ActivityDate) }).OrderByDescending(d => d.ActivityDate).Take(10);
            List<EntityLastActivity> lastActivities = new List<EntityLastActivity>();
            foreach (var activityGroup in lastActivitiesGroup)
            {

                EntityLastActivity activity = lastActivitiesQuery.Where(d => d.ActivityDate == activityGroup.ActivityDate && d.EntityId == activityGroup.EntityId && d.ObjectTableId == activityGroup.ObjectTableId && d.UserId == activityGroup.UserId).FirstOrDefault();
                EntityLastActivity existedActivity = (from a in lastActivities
                                                      where a.EntityId == activity.EntityId && a.ObjectTableId == activity.ObjectTableId
                                                      select a).FirstOrDefault();
                if (existedActivity != null)
                {
                    if (activity.ActivityDate > existedActivity.ActivityDate)
                    {
                        existedActivity.ActivityDate = activity.ActivityDate;
                    }
                }
                else
                {
                    lastActivities.Add(activity);
                }

            }
            return lastActivities;
        }

        public void Add(EntityLastActivity entity)
        {
            context.EntityLastActivities.Add(entity);
        }

        public void Remove(EntityLastActivity entity)
        {
            context.EntityLastActivities.Attach(entity);
            context.EntityLastActivities.Remove(entity);
        }

        public void Update(EntityLastActivity entity)
        {
            context.EntityLastActivities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EntityLastActivity> All()
        {
            return context.EntityLastActivities.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<EntityLastActivity> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EntityLastActivity GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}