using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UserLoginLogRepository : IRepository<UserLoginLog>
    {
        ICommonDataContext commonDataContext;

        public UserLoginLogRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public UserLoginLogRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public UserLoginLogRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
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

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<UserLoginLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public UserLoginLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}