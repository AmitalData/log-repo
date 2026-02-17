 
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
   public partial class JournalReconcileQueryService: EntityQueryService<JournalReconcile,JournalReconcileKeys,JournalReconcilePM,JournalPM,JournalKeys>
   {
   
        JournalReconcileRepository repository;
		IAccountingContext  context;
        public JournalReconcileQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalReconcileRepository(context);
            Repository = repository;
            mapping = new JournalReconcileDataMapping();
        }

        public JournalReconcileQueryService(JournalReconcileRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalReconcileDataMapping();
        }

        public JournalReconcileQueryService(IAccountingContext context)
        {
            this.repository = new JournalReconcileRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalReconcileDataMapping();
        }
		 
		public  JournalReconcilePM GetSingle(string journalid, string ledgertransactionid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalReconcileKeys(){ JournalId = journalid, LedgerTransactionId = ledgertransactionid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalReconcile entityPOCO)
        {
            JournalReconcileKeys entityKeys = new JournalReconcileKeys() { JournalId = entityPOCO.JournalId, LedgerTransactionId = entityPOCO.LedgerTransactionId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 