using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class MobileNotificationLogRepository : IRepository<MobileNotificationLog>
    {
        IGlobalContext globalContext;
        public MobileNotificationLogRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public MobileNotificationLogRepository(IGlobalContext context)
        {
            globalContext = context;
        }


        //public MobileNotificationLog GetSingleMobileNotificationLogByEmailAndEntityId(string id)
        //{
        //    return (from a in context.MobileNotificationLogs
        //            where a.Id == id
        //            select a).FirstOrDefault();
        //}


        public int GetNotificationCountByEmail(string email)
        {
            email = email.ToLower();
            return context.MobileNotificationLogs.Where(a => a.Email == email && !a.IsRead && !a.IsDelete).Count();

        }


        public MobileNotificationLog GetSingleMobileNotificationLog(string id)
        {
            return (from a in context.MobileNotificationLogs
                    where a.Id == id 
                    select a).FirstOrDefault();
        }



        public MobileNotificationLog GetSingleMobileNotificationLogBuIdAndTenant(string id, int? tenant)
        {
            return (from a in context.MobileNotificationLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

    
        public MobileNotificationLog GetFirstNotCompletedMobileNotificationLog()
        {
             return (from a in context.MobileNotificationLogs
                     where a.IOSStatus == "W" || a.AndroidStatus == "W"
                    select a).FirstOrDefault();
   
        }



        public IQueryable<MobileNotificationLog> GetAllMobileNotificationLogs()
        {
            return from a in context.MobileNotificationLogs
                   select a;
        }


        public IQueryable<MobileNotificationLog> GetAllMobileNotificationLogsByEmail(string email)
        {
            email = email.ToLower();
            return from a in context.MobileNotificationLogs
                   where a.Email == email
                   select a;
        }

        


        public void Add(MobileNotificationLog entity)
        {
            context.MobileNotificationLogs.Add(entity);
        }

        public void Remove(MobileNotificationLog entity)
        {
            context.MobileNotificationLogs.Attach(entity);
            context.MobileNotificationLogs.Remove(entity);
        }

        public void Update(MobileNotificationLog entity)
        {
            context.MobileNotificationLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MobileNotificationLog> All()
        {
            return context.MobileNotificationLogs.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<MobileNotificationLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MobileNotificationLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

      
    }
}