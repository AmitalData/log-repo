 
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
   public partial class ReconcileMethodQueryService: EntityQueryService<ReconcileMethod,ReconcileMethodKeys,ReconcileMethodPM,object,ReconcileMethodKeys>
   {
   
        ReconcileMethodRepository repository;
		IAccountingContext  context;
        public ReconcileMethodQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ReconcileMethodRepository(context);
            Repository = repository;
            mapping = new ReconcileMethodDataMapping();
        }

        public ReconcileMethodQueryService(ReconcileMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReconcileMethodDataMapping();
        }

        public ReconcileMethodQueryService(IAccountingContext context)
        {
            this.repository = new ReconcileMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReconcileMethodDataMapping();
        }
		 
		public  ReconcileMethodPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReconcileMethodKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReconcileMethod entityPOCO)
        {
            ReconcileMethodKeys entityKeys = new ReconcileMethodKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 