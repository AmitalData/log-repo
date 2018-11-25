 
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
   public partial class JournalAdditionalDataQueryService: EntityQueryService<JournalAdditionalData,JournalAdditionalDataKeys,JournalAdditionalDataPM,object,JournalAdditionalDataKeys>
   {
   
        JournalAdditionalDataRepository repository;
		IAccountingContext  context;
        public JournalAdditionalDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalAdditionalDataRepository(context);
            Repository = repository;
            mapping = new JournalAdditionalDataDataMapping();
        }

        public JournalAdditionalDataQueryService(JournalAdditionalDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalAdditionalDataDataMapping();
        }

        public JournalAdditionalDataQueryService(IAccountingContext context)
        {
            this.repository = new JournalAdditionalDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalAdditionalDataDataMapping();
        }
		 
		public  JournalAdditionalDataPM GetSingle(string journalid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalAdditionalDataKeys(){ JournalId = journalid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalAdditionalData entityPOCO)
        {
            JournalAdditionalDataKeys entityKeys = new JournalAdditionalDataKeys() { JournalId = entityPOCO.JournalId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 