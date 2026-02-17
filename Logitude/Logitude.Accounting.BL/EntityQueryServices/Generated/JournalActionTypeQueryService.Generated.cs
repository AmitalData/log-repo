 
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
   public partial class JournalActionTypeQueryService: EntityQueryService<JournalActionType,JournalActionTypeKeys,JournalActionTypePM,object,JournalActionTypeKeys>
   {
   
        JournalActionTypeRepository repository;
		IAccountingContext  context;
        public JournalActionTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalActionTypeRepository(context);
            Repository = repository;
            mapping = new JournalActionTypeDataMapping();
        }

        public JournalActionTypeQueryService(JournalActionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalActionTypeDataMapping();
        }

        public JournalActionTypeQueryService(IAccountingContext context)
        {
            this.repository = new JournalActionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalActionTypeDataMapping();
        }
		 
		public  JournalActionTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalActionTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalActionType entityPOCO)
        {
            JournalActionTypeKeys entityKeys = new JournalActionTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 