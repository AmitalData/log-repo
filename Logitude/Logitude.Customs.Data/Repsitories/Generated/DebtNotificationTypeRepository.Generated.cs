 
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
   public partial class DebtNotificationTypeRepository:IRepository<DebtNotificationType>
   {
   
        private ICustomContext currentContext;
        public DebtNotificationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DebtNotificationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DebtNotificationType GetSingle(string code)
        {
            return (from a in context.DebtNotificationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DebtNotificationType> GetAll()
        {
            return from a in context.DebtNotificationTypes  
                   select a;
        }
				 
        public DebtNotificationType GetSingle(EntityKeyFields entityKeys)
        {
            DebtNotificationTypeKeys keys = entityKeys as DebtNotificationTypeKeys;
            return (from a in context.DebtNotificationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DebtNotificationType entity)
        {
            onAdd();
            context.DebtNotificationTypes.Add(entity);
        }

        public void Remove(DebtNotificationType entity)
        {
            context.DebtNotificationTypes.Attach(entity);
            context.DebtNotificationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DebtNotificationType entity)
        {
            onUpdate();
            context.DebtNotificationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DebtNotificationType> All()
        {
            return context.DebtNotificationTypes.ToList();
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
	 