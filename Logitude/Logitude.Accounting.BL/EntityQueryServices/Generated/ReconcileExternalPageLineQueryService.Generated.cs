 
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
   public partial class ReconcileExternalPageLineQueryService: EntityQueryService<ReconcileExternalPageLine,ReconcileExternalPageLineKeys,ReconcileExternalPageLinePM,ReconcileExternalPagePM,ReconcileExternalPageKeys>
   {
   
        ReconcileExternalPageLineRepository repository;
		IAccountingContext  context;
        public ReconcileExternalPageLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ReconcileExternalPageLineRepository(context);
            Repository = repository;
            mapping = new ReconcileExternalPageLineDataMapping();
        }

        public ReconcileExternalPageLineQueryService(ReconcileExternalPageLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReconcileExternalPageLineDataMapping();
        }

        public ReconcileExternalPageLineQueryService(IAccountingContext context)
        {
            this.repository = new ReconcileExternalPageLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReconcileExternalPageLineDataMapping();
        }
		 
		public  ReconcileExternalPageLinePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReconcileExternalPageLineKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReconcileExternalPageLine entityPOCO)
        {
            ReconcileExternalPageLineKeys entityKeys = new ReconcileExternalPageLineKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 