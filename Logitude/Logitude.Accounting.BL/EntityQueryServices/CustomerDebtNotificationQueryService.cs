 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class CustomerDebtNotificationQueryService: EntityQueryService<CustomerDebtNotification,CustomerDebtNotificationKeys,CustomerDebtNotificationPM,object,CustomerDebtNotificationKeys>
   {
		public  CustomerDebtNotificationPM GetCustomerDebtNotificationByAccountId(int tenant,string accountId)
        {
            var customerDebtNotificationPM = context.CustomerDebtNotifications.Where(x =>x.Tenant == tenant && x.AccountId == accountId).Select(a => new CustomerDebtNotificationPM
            {
                Id = a.Id,
				Tenant = a.Tenant,
				InActive = a.InActive,
				TypesDebts = a.TypesDebts,
				DebtLevel = a.DebtLevel,
				DebtLevelAmount = a.DebtLevelAmount,
				TasksSchedulerId = a.TasksSchedulerId,
				PaymentNotes = a.PaymentNotes,
				AccountId = a.AccountId
			}).FirstOrDefault();
			return customerDebtNotificationPM;
		}

		


	}
   
}
	 