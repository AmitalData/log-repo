 
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
   public partial class ReconciliationQueryService: EntityQueryService<Reconciliation,ReconciliationKeys,ReconciliationPM,object,ReconciliationKeys>
   {
   
        ReconciliationRepository repository;
		IAccountingContext  context;
        public ReconciliationQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ReconciliationRepository(context);
            Repository = repository;
            mapping = new ReconciliationDataMapping();
        }

        public ReconciliationQueryService(ReconciliationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReconciliationDataMapping();
        }

        public ReconciliationQueryService(IAccountingContext context)
        {
            this.repository = new ReconciliationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReconciliationDataMapping();
        }
		 
		public  ReconciliationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReconciliationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Reconciliation entityPOCO)
        {
            ReconciliationKeys entityKeys = new ReconciliationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 