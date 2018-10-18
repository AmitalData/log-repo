 
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
   public partial class ReconcileExternalPageStatusQueryService: EntityQueryService<ReconcileExternalPageStatus,ReconcileExternalPageStatusKeys,ReconcileExternalPageStatusPM,object,ReconcileExternalPageStatusKeys>
   {
   
        ReconcileExternalPageStatusRepository repository;
		IAccountingContext  context;
        public ReconcileExternalPageStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ReconcileExternalPageStatusRepository(context);
            Repository = repository;
            mapping = new ReconcileExternalPageStatusDataMapping();
        }

        public ReconcileExternalPageStatusQueryService(ReconcileExternalPageStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReconcileExternalPageStatusDataMapping();
        }

        public ReconcileExternalPageStatusQueryService(IAccountingContext context)
        {
            this.repository = new ReconcileExternalPageStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReconcileExternalPageStatusDataMapping();
        }
		 
		public  ReconcileExternalPageStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReconcileExternalPageStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReconcileExternalPageStatus entityPOCO)
        {
            ReconcileExternalPageStatusKeys entityKeys = new ReconcileExternalPageStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 