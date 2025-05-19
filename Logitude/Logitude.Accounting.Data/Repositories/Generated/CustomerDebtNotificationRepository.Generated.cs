 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class CustomerDebtNotificationRepository:IRepository<CustomerDebtNotification>
   {
   
        private IAccountingContext currentContext;
        public CustomerDebtNotificationRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CustomerDebtNotificationRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerDebtNotification GetSingle(string id, int tenant)
        {
            return (from a in context.CustomerDebtNotifications
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerDebtNotification> GetAll(int tenant)
        {
            return from a in context.CustomerDebtNotifications  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomerDebtNotification GetSingle(EntityKeyFields entityKeys)
        {
            CustomerDebtNotificationKeys keys = entityKeys as CustomerDebtNotificationKeys;
            return (from a in context.CustomerDebtNotifications
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerDebtNotification entity)
        {
            onAdd();
            context.CustomerDebtNotifications.Add(entity);
        }

        public void Remove(CustomerDebtNotification entity)
        {
            context.CustomerDebtNotifications.Attach(entity);
            context.CustomerDebtNotifications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerDebtNotification entity)
        {
            onUpdate();
            context.CustomerDebtNotifications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerDebtNotification> All()
        {
            return context.CustomerDebtNotifications.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 