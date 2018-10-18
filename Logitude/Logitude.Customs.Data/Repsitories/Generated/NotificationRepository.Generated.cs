 
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
   public partial class NotificationRepository:IRepository<Notification>
   {
   
        private ICustomContext currentContext;
        public NotificationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NotificationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Notification GetSingle(string id, int tenant)
        {
            return (from a in context.Notifications
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Notification> GetAll(int tenant)
        {
            return from a in context.Notifications  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Notification GetSingle(EntityKeyFields entityKeys)
        {
            NotificationKeys keys = entityKeys as NotificationKeys;
            return (from a in context.Notifications
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Notification entity)
        {
            onAdd();
            context.Notifications.Add(entity);
        }

        public void Remove(Notification entity)
        {
            context.Notifications.Attach(entity);
            context.Notifications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Notification entity)
        {
            onUpdate();
            context.Notifications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Notification> All()
        {
            return context.Notifications.ToList();
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
	 