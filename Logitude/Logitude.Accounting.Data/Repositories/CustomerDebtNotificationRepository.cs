 
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
        
		public List<CustomerDebtNotification> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
		public CustomerDebtNotification GetCustomerDebtNotificationByTaskSchudler(int tenant, string tasksSchedulerId)
		{
			var customerDebtNotificationPM = context.CustomerDebtNotifications.Where(x => x.Tenant== tenant && x.TasksSchedulerId == tasksSchedulerId).FirstOrDefault();
			return customerDebtNotificationPM;
		}
		public CustomerDebtNotification GetCustomerDebtNotificationByAccountId(int tenant, string accountId)
		{
			var customerDebtNotificationPM = context.CustomerDebtNotifications.Where(x => x.Tenant == tenant && x.AccountId == accountId).FirstOrDefault();
			return customerDebtNotificationPM;
		}
	}

}
   