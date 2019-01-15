 
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
   public partial class AccountingIntegrityCheckQueryService: EntityQueryService<AccountingIntegrityCheck,AccountingIntegrityCheckKeys,AccountingIntegrityCheckPM,object,AccountingIntegrityCheckKeys>
   {
   
        AccountingIntegrityCheckRepository repository;
		IAccountingContext  context;
        public AccountingIntegrityCheckQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AccountingIntegrityCheckRepository(context);
            Repository = repository;
            mapping = new AccountingIntegrityCheckDataMapping();
        }

        public AccountingIntegrityCheckQueryService(AccountingIntegrityCheckRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AccountingIntegrityCheckDataMapping();
        }

        public AccountingIntegrityCheckQueryService(IAccountingContext context)
        {
            this.repository = new AccountingIntegrityCheckRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AccountingIntegrityCheckDataMapping();
        }
		 
		public  AccountingIntegrityCheckPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AccountingIntegrityCheckKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AccountingIntegrityCheck entityPOCO)
        {
            AccountingIntegrityCheckKeys entityKeys = new AccountingIntegrityCheckKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 