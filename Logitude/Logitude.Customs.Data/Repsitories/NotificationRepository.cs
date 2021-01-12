 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Entity.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class NotificationRepository:IRepository<Notification>
   {
        public NotificationRepository()
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
        }

		public List<Notification> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<Notification> GetTopTenNotifications(string userId, int tenant)
        {
            return (from a in context.Notifications.Include("ObjectTable")
                    where a.Tenant == tenant && a.AssigneToId == userId && !a.IsClosedByAssignee

                    select a).OrderByDescending(d => d.CreateDate).Take(10).ToList();
        }

        public IQueryable<Notification> GetQBadjCount(string userId, int tenant)
        {
            return (from a in context.Notifications
                    where a.Tenant == tenant && a.AssigneToId == userId && a.BadjCount
                    //יש להוסיף לחיתוך הדיפולטיבי גם: לא סגורות (IsClosedByAssignee=False), וכן לבנות אינדקס על אחראי+IsClosedByAssignee+BadjCount.
                    //CREATE INDEX IX_NOTIFICATIONS_BC_ICBA_ATI ON NOTIFICATIONS (BADJCOUNT ASC, ASSIGNETOID ASC, ISCLOSEDBYASSIGNEE ASC) 
                    where !a.IsClosedByAssignee
                    select a);


        }

        public int GetBadjCount(string userId, int tenant)
        {
            return
            //return (from a in context.Notifications
            //        where a.Tenant == tenant && a.AssigneToId == userId && a.BadjCount
            //        //יש להוסיף לחיתוך הדיפולטיבי גם: לא סגורות (IsClosedByAssignee=False), וכן לבנות אינדקס על אחראי+IsClosedByAssignee+BadjCount.
            //        where !a.IsClosedByAssignee
            //        select a)
            GetQBadjCount(userId, tenant)
                    .Count();
        }

        public List<Notification> GetNotificationsWithBadjCount(string userId, int tenant)
        {
            return
                    //(from a in context.Notifications
                    //    where a.Tenant == tenant && a.AssigneToId == userId && a.BadjCount

                    //    select a)
                    GetQBadjCount(userId, tenant)
                    .ToList();

        }

        public int GetOpenNotificationCountForUser(string userId, int tenant)
        {
            return (from a in context.Notifications
                    where a.Tenant == tenant && a.AssigneToId == userId && !a.IsClosedByAssignee

                    select a).Count();
        }

       public List<Notification> GetNotificationsByIds (List<string> ids, int tenant)
       {
           List<Notification> notifications = new List<Notification>();


           foreach(string id in ids)
           {
             Notification notification = ( from a in context.Notifications.Include("AssigneTo.Contact").Include("AssigneTo").Include("NotificationDefinition")
                       where a.Tenant == tenant && a.Id == id

                       select a).FirstOrDefault();
             notifications.Add(notification);
           }

           return notifications;
       }
 
   }

}
   