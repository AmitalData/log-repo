 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class NotificationReplyRepository:IRepository<NotificationReply>
   {
   
        private ICustomContext currentContext;
        public NotificationReplyRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NotificationReplyRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  NotificationReply GetSingle(string notificationid, int line, int tenant)
        {
            return (from a in context.NotificationReplies
                    where a.NotificationId == notificationid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<NotificationReply> GetAll(int tenant)
        {
            return from a in context.NotificationReplies  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public NotificationReply GetSingle(EntityKeyFields entityKeys)
        {
            NotificationReplyKeys keys = entityKeys as NotificationReplyKeys;
            return (from a in context.NotificationReplies
                    where a.NotificationId == keys.NotificationId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(NotificationReply entity)
        {
            onAdd();
            context.NotificationReplies.Add(entity);
        }

        public void Remove(NotificationReply entity)
        {
            context.NotificationReplies.Attach(entity);
            context.NotificationReplies.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(NotificationReply entity)
        {
            onUpdate();
            context.NotificationReplies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NotificationReply> All()
        {
            return context.NotificationReplies.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 