 
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
   
        CustomerDebtNotificationRepository repository;
		IAccountingContext  context;
        public CustomerDebtNotificationQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomerDebtNotificationRepository(context);
            Repository = repository;
            mapping = new CustomerDebtNotificationDataMapping();
        }

        public CustomerDebtNotificationQueryService(CustomerDebtNotificationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomerDebtNotificationDataMapping();
        }

        public CustomerDebtNotificationQueryService(IAccountingContext context)
        {
            this.repository = new CustomerDebtNotificationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomerDebtNotificationDataMapping();
        }
		 
		public  CustomerDebtNotificationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomerDebtNotificationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomerDebtNotification entityPOCO)
        {
            CustomerDebtNotificationKeys entityKeys = new CustomerDebtNotificationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 