using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class EntityLastAccessQuery
    {
        EntityLastActivityRepository repository;
        public EntityLastAccessQuery()
        {
            repository = new EntityLastActivityRepository(); 
        }

        public EntityLastAccessQuery(int tenant)
        {
            repository = new EntityLastActivityRepository(tenant);
        }

        public EntityLastAccessQuery(EntityLastActivityRepository entityLastAccessRepository)
        {
            repository = entityLastAccessRepository;
        }

        public EntityLastAccessPM GetEntityLastAccessByCurrentUser(string objectTableId, string entityId, string userId, int tenant)
        {
            EntityLastAccessPM entitylast = (from a in repository.context.EntityLastActivities.Include("User.Contact")
                                             where a.ObjectTableId == objectTableId && a.EntityId == entityId && a.Tenant == tenant && a.UserId == userId
                                             select new EntityLastAccessPM()
                                             {
                                                 AccessDate = a.ActivityDate,
                                                 EntityId = a.EntityId,
                                                 Id = a.Id,
                                                 ObjectTableId = a.ObjectTableId,
                                                 Tenant = a.Tenant,
                                                 UserId = a.UserId,
                                                 UserName = a.User.Contact.EnglishName,
                                             }).OrderByDescending(d => d.AccessDate).FirstOrDefault();
            return entitylast;
        }

        public EntityLastAccessPM GetEntityLastAccessByEntity(string objectTableId, string entityId, int tenant)
        {
            return (from a in repository.context.EntityLastActivities.Include("User.Contact")
                    where a.ObjectTableId == objectTableId && a.EntityId == entityId && a.Tenant == tenant
                    select new EntityLastAccessPM()
                    {
                        AccessDate = a.ActivityDate,
                        EntityId = a.EntityId,
                        Id = a.Id,
                        ObjectTableId = a.ObjectTableId,
                        Tenant = a.Tenant,
                        UserId = a.UserId,
                        UserName = a.User.Contact.EnglishName,
                    }).OrderByDescending(d => d.AccessDate).FirstOrDefault();
        }



    }
}