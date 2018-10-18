 
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
   public partial class RevaluationStatusQueryService: EntityQueryService<RevaluationStatus,RevaluationStatusKeys,RevaluationStatusPM,object,RevaluationStatusKeys>
   {
   
        RevaluationStatusRepository repository;
		IAccountingContext  context;
        public RevaluationStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new RevaluationStatusRepository(context);
            Repository = repository;
            mapping = new RevaluationStatusDataMapping();
        }

        public RevaluationStatusQueryService(RevaluationStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RevaluationStatusDataMapping();
        }

        public RevaluationStatusQueryService(IAccountingContext context)
        {
            this.repository = new RevaluationStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RevaluationStatusDataMapping();
        }
		 
		public  RevaluationStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RevaluationStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RevaluationStatus entityPOCO)
        {
            RevaluationStatusKeys entityKeys = new RevaluationStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 