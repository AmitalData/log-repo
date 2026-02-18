using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class UserLoginLogRepository : IRepository<UserLoginLog,string>
    {
        IAmitalCloudContext currentContext;

        public UserLoginLogRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public UserLoginLogRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public UserLoginLogRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }

        public IQueryable<UserLoginLog> GetUserLoginLogs(int tenant)
        {
            return (from d in context.UserLoginLogs where d.Tenant == tenant select d);
        }

        public IQueryable<UserLoginLog> GetUserLoginHistory(string userId, int tenant)
        {
            return (from d in context.UserLoginLogs where d.UserId == userId && d.Tenant == tenant select d);
        }

        public UserLoginLog GetSingleUserLoginLog(string id, int tenant,bool getFromCache)
        {
            string entityName = "UserLoginLog" + id + tenant;
            UserLoginLog entity;
            if (getFromCache)
            {
              
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from record in context.UserLoginLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            if (entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (UserLoginLog)CacheManager.CacheWrapper.Get(entityName);
                    }

                
             
            }
            else
            {
                entity = (from record in context.UserLoginLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            }
            return entity;
        }

        public void Add(UserLoginLog entity)
        {
            context.UserLoginLogs.Add(entity);
        }

        public void Remove(UserLoginLog entity)
        {
            try
            {
                context.UserLoginLogs.Attach(entity);
            }
            catch { };
            context.UserLoginLogs.Remove(entity);
        }

        public void Update(UserLoginLog entity)
        {
            try
            {
                context.UserLoginLogs.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<UserLoginLog> All()
        {
            return context.UserLoginLogs.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<UserLoginLog> GetMulti(IEntityKeyFields<UserLoginLog,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public UserLoginLog GetSingle(IEntityKeyFields<UserLoginLog,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}