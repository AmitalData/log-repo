 
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
   public partial class AccountingNoteQueryService: EntityQueryService<AccountingNote,AccountingNoteKeys,AccountingNotePM,object,AccountingNoteKeys>
   {
   
        AccountingNoteRepository repository;
		IAccountingContext  context;
        public AccountingNoteQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AccountingNoteRepository(context);
            Repository = repository;
            mapping = new AccountingNoteDataMapping();
        }

        public AccountingNoteQueryService(AccountingNoteRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AccountingNoteDataMapping();
        }

        public AccountingNoteQueryService(IAccountingContext context)
        {
            this.repository = new AccountingNoteRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AccountingNoteDataMapping();
        }
		 
		public  AccountingNotePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AccountingNoteKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AccountingNote entityPOCO)
        {
            AccountingNoteKeys entityKeys = new AccountingNoteKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 