 
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
   public partial class InterestTransactionQueryService: EntityQueryService<InterestTransaction,InterestTransactionKeys,InterestTransactionPM,object,InterestTransactionKeys>
   {
   
        InterestTransactionRepository repository;
		IAccountingContext  context;
        public InterestTransactionQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestTransactionRepository(context);
            Repository = repository;
            mapping = new InterestTransactionDataMapping();
        }

        public InterestTransactionQueryService(InterestTransactionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestTransactionDataMapping();
        }

        public InterestTransactionQueryService(IAccountingContext context)
        {
            this.repository = new InterestTransactionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestTransactionDataMapping();
        }
		 
		public  InterestTransactionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestTransactionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestTransaction entityPOCO)
        {
            InterestTransactionKeys entityKeys = new InterestTransactionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 