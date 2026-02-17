 
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
   public partial class JournalMoreDataQueryService: EntityQueryService<JournalMoreData,JournalMoreDataKeys,JournalMoreDataPM,JournalPM,JournalKeys>
   {
   
        JournalMoreDataRepository repository;
		IAccountingContext  context;
        public JournalMoreDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalMoreDataRepository(context);
            Repository = repository;
            mapping = new JournalMoreDataDataMapping();
        }

        public JournalMoreDataQueryService(JournalMoreDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalMoreDataDataMapping();
        }

        public JournalMoreDataQueryService(IAccountingContext context)
        {
            this.repository = new JournalMoreDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalMoreDataDataMapping();
        }
		 
		public  JournalMoreDataPM GetSingle(string journalid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalMoreDataKeys(){ JournalId = journalid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalMoreData entityPOCO)
        {
            JournalMoreDataKeys entityKeys = new JournalMoreDataKeys() { JournalId = entityPOCO.JournalId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 