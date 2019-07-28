 
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
   public partial class JournalExternalReconcileQueryService: EntityQueryService<JournalExternalReconcile,JournalExternalReconcileKeys,JournalExternalReconcilePM,JournalPM,JournalKeys>
   {
   
        JournalExternalReconcileRepository repository;
		IAccountingContext  context;
        public JournalExternalReconcileQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalExternalReconcileRepository(context);
            Repository = repository;
            mapping = new JournalExternalReconcileDataMapping();
        }

        public JournalExternalReconcileQueryService(JournalExternalReconcileRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalExternalReconcileDataMapping();
        }

        public JournalExternalReconcileQueryService(IAccountingContext context)
        {
            this.repository = new JournalExternalReconcileRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalExternalReconcileDataMapping();
        }
		 
		public  JournalExternalReconcilePM GetSingle(string journalid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalExternalReconcileKeys(){ JournalId = journalid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalExternalReconcile entityPOCO)
        {
            JournalExternalReconcileKeys entityKeys = new JournalExternalReconcileKeys() { JournalId = entityPOCO.JournalId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 