 
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
   public partial class AutomaticReconcileMethodQueryService: EntityQueryService<AutomaticReconcileMethod,AutomaticReconcileMethodKeys,AutomaticReconcileMethodPM,object,AutomaticReconcileMethodKeys>
   {
   
        AutomaticReconcileMethodRepository repository;
		IAccountingContext  context;
        public AutomaticReconcileMethodQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AutomaticReconcileMethodRepository(context);
            Repository = repository;
            mapping = new AutomaticReconcileMethodDataMapping();
        }

        public AutomaticReconcileMethodQueryService(AutomaticReconcileMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AutomaticReconcileMethodDataMapping();
        }

        public AutomaticReconcileMethodQueryService(IAccountingContext context)
        {
            this.repository = new AutomaticReconcileMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AutomaticReconcileMethodDataMapping();
        }
		 
		public  AutomaticReconcileMethodPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AutomaticReconcileMethodKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AutomaticReconcileMethod entityPOCO)
        {
            AutomaticReconcileMethodKeys entityKeys = new AutomaticReconcileMethodKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 