 
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
   public partial class ReconciliationLineQueryService: EntityQueryService<ReconciliationLine,ReconciliationLineKeys,ReconciliationLinePM,ReconciliationPM,ReconciliationKeys>
   {
   
        ReconciliationLineRepository repository;
		IAccountingContext  context;
        public ReconciliationLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ReconciliationLineRepository(context);
            Repository = repository;
            mapping = new ReconciliationLineDataMapping();
        }

        public ReconciliationLineQueryService(ReconciliationLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReconciliationLineDataMapping();
        }

        public ReconciliationLineQueryService(IAccountingContext context)
        {
            this.repository = new ReconciliationLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReconciliationLineDataMapping();
        }
		 
		public  ReconciliationLinePM GetSingle(string reconciliationid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReconciliationLineKeys(){ ReconciliationId = reconciliationid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReconciliationLine entityPOCO)
        {
            ReconciliationLineKeys entityKeys = new ReconciliationLineKeys() { ReconciliationId = entityPOCO.ReconciliationId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 