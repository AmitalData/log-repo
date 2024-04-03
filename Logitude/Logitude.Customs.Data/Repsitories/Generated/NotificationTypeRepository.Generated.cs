 
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
   public partial class NotificationTypeRepository:IRepository<NotificationType>
   {
   
        private ICustomContext currentContext;
        public NotificationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NotificationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  NotificationType GetSingle(string code)
        {
            return (from a in context.NotificationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<NotificationType> GetAll()
        {
            return from a in context.NotificationTypes  
                   select a;
        }
				 
        public NotificationType GetSingle(EntityKeyFields entityKeys)
        {
            NotificationTypeKeys keys = entityKeys as NotificationTypeKeys;
            return (from a in context.NotificationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(NotificationType entity)
        {
            onAdd();
            context.NotificationTypes.Add(entity);
        }

        public void Remove(NotificationType entity)
        {
            context.NotificationTypes.Attach(entity);
            context.NotificationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(NotificationType entity)
        {
            onUpdate();
            context.NotificationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NotificationType> All()
        {
            return context.NotificationTypes.ToList();
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
	 