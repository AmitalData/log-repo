 
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
   public partial class JournalQueryService: EntityQueryService<Journal,JournalKeys,JournalPM,object,JournalKeys>
   {
   
        JournalRepository repository;
		IAccountingContext  context;
        public JournalQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new JournalRepository(context);
            Repository = repository;
            mapping = new JournalDataMapping();
        }

        public JournalQueryService(JournalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new JournalDataMapping();
        }

        public JournalQueryService(IAccountingContext context)
        {
            this.repository = new JournalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new JournalDataMapping();
        }
		 
		public  JournalPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new JournalKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Journal entityPOCO)
        {
            JournalKeys entityKeys = new JournalKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 