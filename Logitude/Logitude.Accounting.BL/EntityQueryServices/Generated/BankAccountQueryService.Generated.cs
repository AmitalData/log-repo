 
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
   public partial class BankAccountQueryService: EntityQueryService<BankAccount,BankAccountKeys,BankAccountPM,object,BankAccountKeys>
   {
   
        BankAccountRepository repository;
		IAccountingContext  context;
        public BankAccountQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new BankAccountRepository(context);
            Repository = repository;
            mapping = new BankAccountDataMapping();
        }

        public BankAccountQueryService(BankAccountRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BankAccountDataMapping();
        }

        public BankAccountQueryService(IAccountingContext context)
        {
            this.repository = new BankAccountRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BankAccountDataMapping();
        }
		 
		public  BankAccountPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BankAccountKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BankAccount entityPOCO)
        {
            BankAccountKeys entityKeys = new BankAccountKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 