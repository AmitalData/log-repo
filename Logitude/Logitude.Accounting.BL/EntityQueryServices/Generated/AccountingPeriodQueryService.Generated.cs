 
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
   public partial class AccountingPeriodQueryService: EntityQueryService<AccountingPeriod,AccountingPeriodKeys,AccountingPeriodPM,object,AccountingPeriodKeys>
   {
   
        AccountingPeriodRepository repository;
		IAccountingContext  context;
        public AccountingPeriodQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AccountingPeriodRepository(context);
            Repository = repository;
            mapping = new AccountingPeriodDataMapping();
        }

        public AccountingPeriodQueryService(AccountingPeriodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AccountingPeriodDataMapping();
        }

        public AccountingPeriodQueryService(IAccountingContext context)
        {
            this.repository = new AccountingPeriodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AccountingPeriodDataMapping();
        }
		 
		public  AccountingPeriodPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AccountingPeriodKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AccountingPeriod entityPOCO)
        {
            AccountingPeriodKeys entityKeys = new AccountingPeriodKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 