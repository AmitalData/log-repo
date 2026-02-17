 
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
   public partial class RevaluationQueryService: EntityQueryService<Revaluation,RevaluationKeys,RevaluationPM,object,RevaluationKeys>
   {
   
        RevaluationRepository repository;
		IAccountingContext  context;
        public RevaluationQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new RevaluationRepository(context);
            Repository = repository;
            mapping = new RevaluationDataMapping();
        }

        public RevaluationQueryService(RevaluationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RevaluationDataMapping();
        }

        public RevaluationQueryService(IAccountingContext context)
        {
            this.repository = new RevaluationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RevaluationDataMapping();
        }
		 
		public  RevaluationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RevaluationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Revaluation entityPOCO)
        {
            RevaluationKeys entityKeys = new RevaluationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 