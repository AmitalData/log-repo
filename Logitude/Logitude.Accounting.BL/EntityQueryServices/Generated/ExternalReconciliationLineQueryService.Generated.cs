 
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
   public partial class ExternalReconciliationLineQueryService: EntityQueryService<ExternalReconciliationLine,ExternalReconciliationLineKeys,ExternalReconciliationLinePM,ExternalReconciliationPM,ExternalReconciliationKeys>
   {
   
        ExternalReconciliationLineRepository repository;
		IAccountingContext  context;
        public ExternalReconciliationLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ExternalReconciliationLineRepository(context);
            Repository = repository;
            mapping = new ExternalReconciliationLineDataMapping();
        }

        public ExternalReconciliationLineQueryService(ExternalReconciliationLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExternalReconciliationLineDataMapping();
        }

        public ExternalReconciliationLineQueryService(IAccountingContext context)
        {
            this.repository = new ExternalReconciliationLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExternalReconciliationLineDataMapping();
        }
		 
		public  ExternalReconciliationLinePM GetSingle(string reconciliationid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExternalReconciliationLineKeys(){ ReconciliationId = reconciliationid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExternalReconciliationLine entityPOCO)
        {
            ExternalReconciliationLineKeys entityKeys = new ExternalReconciliationLineKeys() { ReconciliationId = entityPOCO.ReconciliationId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 