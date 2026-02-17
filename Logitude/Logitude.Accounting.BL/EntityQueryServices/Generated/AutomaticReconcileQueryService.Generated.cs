 
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
   public partial class AutomaticReconcileQueryService: EntityQueryService<AutomaticReconcile,AutomaticReconcileKeys,AutomaticReconcilePM,object,AutomaticReconcileKeys>
   {
   
        AutomaticReconcileRepository repository;
		IAccountingContext  context;
        public AutomaticReconcileQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AutomaticReconcileRepository(context);
            Repository = repository;
            mapping = new AutomaticReconcileDataMapping();
        }

        public AutomaticReconcileQueryService(AutomaticReconcileRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AutomaticReconcileDataMapping();
        }

        public AutomaticReconcileQueryService(IAccountingContext context)
        {
            this.repository = new AutomaticReconcileRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AutomaticReconcileDataMapping();
        }
		 
		public  AutomaticReconcilePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AutomaticReconcileKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AutomaticReconcile entityPOCO)
        {
            AutomaticReconcileKeys entityKeys = new AutomaticReconcileKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 