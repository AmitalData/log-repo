 
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
   public partial class LedgerTransactionQueryService: EntityQueryService<LedgerTransaction,LedgerTransactionKeys,LedgerTransactionPM,object,LedgerTransactionKeys>
   {
   
        LedgerTransactionRepository repository;
		IAccountingContext  context;
        public LedgerTransactionQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new LedgerTransactionRepository(context);
            Repository = repository;
            mapping = new LedgerTransactionDataMapping();
        }

        public LedgerTransactionQueryService(LedgerTransactionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LedgerTransactionDataMapping();
        }

        public LedgerTransactionQueryService(IAccountingContext context)
        {
            this.repository = new LedgerTransactionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LedgerTransactionDataMapping();
        }
		 
		public  LedgerTransactionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LedgerTransactionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LedgerTransaction entityPOCO)
        {
            LedgerTransactionKeys entityKeys = new LedgerTransactionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 