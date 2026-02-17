 
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
   public partial class JournalStatusTypeQueryService: EntityQueryService<JournalStatusType,JournalStatusTypeKeys,JournalStatusTypePM,object,JournalStatusTypeKeys>
   {
   
        JournalStatusTypeRepository repository;
		IAccountingContext  context;
        public JournalStatusTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalStatusTypeRepository(context);
            Repository = repository;
            mapping = new JournalStatusTypeDataMapping();
        }

        public JournalStatusTypeQueryService(JournalStatusTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalStatusTypeDataMapping();
        }

        public JournalStatusTypeQueryService(IAccountingContext context)
        {
            this.repository = new JournalStatusTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalStatusTypeDataMapping();
        }
		 
		public  JournalStatusTypePM GetSingle(string journalstatusid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalStatusTypeKeys(){ JournalStatusID = journalstatusid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalStatusType entityPOCO)
        {
            JournalStatusTypeKeys entityKeys = new JournalStatusTypeKeys() { JournalStatusID = entityPOCO.JournalStatusID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 