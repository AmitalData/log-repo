 
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
   public partial class AccountingEntityJournalQueryService: EntityQueryService<AccountingEntityJournal,AccountingEntityJournalKeys,AccountingEntityJournalPM,object,AccountingEntityJournalKeys>
   {
   
        AccountingEntityJournalRepository repository;
		IAccountingContext  context;
        public AccountingEntityJournalQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AccountingEntityJournalRepository(context);
            Repository = repository;
            mapping = new AccountingEntityJournalDataMapping();
        }

        public AccountingEntityJournalQueryService(AccountingEntityJournalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AccountingEntityJournalDataMapping();
        }

        public AccountingEntityJournalQueryService(IAccountingContext context)
        {
            this.repository = new AccountingEntityJournalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AccountingEntityJournalDataMapping();
        }
		 
		public  AccountingEntityJournalPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AccountingEntityJournalKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AccountingEntityJournal entityPOCO)
        {
            AccountingEntityJournalKeys entityKeys = new AccountingEntityJournalKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 