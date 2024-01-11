 
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
   public partial class AssigneeNotificationTypeRepository:IRepository<AssigneeNotificationType>
   {
   
        private ICustomContext currentContext;
        public AssigneeNotificationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AssigneeNotificationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AssigneeNotificationType GetSingle(string code)
        {
            return (from a in context.AssigneeNotificationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AssigneeNotificationType> GetAll()
        {
            return from a in context.AssigneeNotificationTypes  
                   select a;
        }
				 
        public AssigneeNotificationType GetSingle(EntityKeyFields entityKeys)
        {
            AssigneeNotificationTypeKeys keys = entityKeys as AssigneeNotificationTypeKeys;
            return (from a in context.AssigneeNotificationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AssigneeNotificationType entity)
        {
            onAdd();
            context.AssigneeNotificationTypes.Add(entity);
        }

        public void Remove(AssigneeNotificationType entity)
        {
            context.AssigneeNotificationTypes.Attach(entity);
            context.AssigneeNotificationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AssigneeNotificationType entity)
        {
            onUpdate();
            context.AssigneeNotificationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AssigneeNotificationType> All()
        {
            return context.AssigneeNotificationTypes.ToList();
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
	 