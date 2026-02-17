 
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
   public partial class JournalTypeQueryService: EntityQueryService<JournalType,JournalTypeKeys,JournalTypePM,object,JournalTypeKeys>
   {
   
        JournalTypeRepository repository;
		IAccountingContext  context;
        public JournalTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalTypeRepository(context);
            Repository = repository;
            mapping = new JournalTypeDataMapping();
        }

        public JournalTypeQueryService(JournalTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalTypeDataMapping();
        }

        public JournalTypeQueryService(IAccountingContext context)
        {
            this.repository = new JournalTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalTypeDataMapping();
        }
		 
		public  JournalTypePM GetSingle(string journaltypeid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalTypeKeys(){ JournalTypeID = journaltypeid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(JournalType entityPOCO)
        {
            JournalTypeKeys entityKeys = new JournalTypeKeys() { JournalTypeID = entityPOCO.JournalTypeID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 