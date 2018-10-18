 
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
   public partial class ExternalReconciliationQueryService: EntityQueryService<ExternalReconciliation,ExternalReconciliationKeys,ExternalReconciliationPM,object,ExternalReconciliationKeys>
   {
   
        ExternalReconciliationRepository repository;
		IAccountingContext  context;
        public ExternalReconciliationQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ExternalReconciliationRepository(context);
            Repository = repository;
            mapping = new ExternalReconciliationDataMapping();
        }

        public ExternalReconciliationQueryService(ExternalReconciliationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExternalReconciliationDataMapping();
        }

        public ExternalReconciliationQueryService(IAccountingContext context)
        {
            this.repository = new ExternalReconciliationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExternalReconciliationDataMapping();
        }
		 
		public  ExternalReconciliationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExternalReconciliationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExternalReconciliation entityPOCO)
        {
            ExternalReconciliationKeys entityKeys = new ExternalReconciliationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 