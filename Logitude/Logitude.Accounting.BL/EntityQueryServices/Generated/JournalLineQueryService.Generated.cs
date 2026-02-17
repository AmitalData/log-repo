 
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
   public partial class JournalLineQueryService: EntityQueryService<JournalLine,JournalLineKeys,JournalLinePM,JournalPM,JournalKeys>
   {
   
        JournalLineRepository repository;
		IAccountingContext  context;
        public JournalLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalLineRepository(context);
            Repository = repository;
            mapping = new JournalLineDataMapping();
        }

        public JournalLineQueryService(JournalLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalLineDataMapping();
        }

        public JournalLineQueryService(IAccountingContext context)
        {
            this.repository = new JournalLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalLineDataMapping();
        }
		 
		public  JournalLinePM GetSingle(string journalid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalLineKeys(){ JournalId = journalid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalLine entityPOCO)
        {
            JournalLineKeys entityKeys = new JournalLineKeys() { JournalId = entityPOCO.JournalId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 