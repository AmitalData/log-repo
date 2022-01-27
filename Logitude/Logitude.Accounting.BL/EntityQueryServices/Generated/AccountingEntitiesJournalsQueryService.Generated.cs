 
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
   public partial class AccountingEntitiesJournalQueryService: EntityQueryService<AccountingEntitiesJournal,AccountingEntitiesJournalKeys,AccountingEntitiesJournalPM,object,AccountingEntitiesJournalKeys>
   {
   
        AccountingEntitiesJournalRepository repository;
		IAccountingContext  context;
        public AccountingEntitiesJournalQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AccountingEntitiesJournalRepository(context);
            Repository = repository;
            mapping = new AccountingEntitiesJournalDataMapping();
        }

        public AccountingEntitiesJournalQueryService(AccountingEntitiesJournalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AccountingEntitiesJournalDataMapping();
        }

        public AccountingEntitiesJournalQueryService(IAccountingContext context)
        {
            this.repository = new AccountingEntitiesJournalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AccountingEntitiesJournalDataMapping();
        }
		 
		public  AccountingEntitiesJournalPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AccountingEntitiesJournalKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AccountingEntitiesJournal entityPOCO)
        {
            AccountingEntitiesJournalKeys entityKeys = new AccountingEntitiesJournalKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 