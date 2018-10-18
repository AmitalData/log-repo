 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class NotificationRepository:IRepository<Notification>
   {
        
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

        public int GetBadjCount(string userId, int tenant)
        {
            return (from a in context.Notifications
                    where a.Tenant == tenant && a.AssigneToId == userId && a.BadjCount

                    select a).Count();
        }

        public List<Notification> GetNotificationsWithBadjCount(string userId, int tenant)
        {
            return (from a in context.Notifications
                    where a.Tenant == tenant && a.AssigneToId == userId && a.BadjCount

                    select a).ToList();

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
   