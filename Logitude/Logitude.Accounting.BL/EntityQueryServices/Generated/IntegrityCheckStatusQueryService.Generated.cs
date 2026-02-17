 
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
   public partial class IntegrityCheckStatusQueryService: EntityQueryService<IntegrityCheckStatus,IntegrityCheckStatusKeys,IntegrityCheckStatusPM,object,IntegrityCheckStatusKeys>
   {
   
        IntegrityCheckStatusRepository repository;
		IAccountingContext  context;
        public IntegrityCheckStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new IntegrityCheckStatusRepository(context);
            Repository = repository;
            mapping = new IntegrityCheckStatusDataMapping();
        }

        public IntegrityCheckStatusQueryService(IntegrityCheckStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new IntegrityCheckStatusDataMapping();
        }

        public IntegrityCheckStatusQueryService(IAccountingContext context)
        {
            this.repository = new IntegrityCheckStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new IntegrityCheckStatusDataMapping();
        }
		 
		public  IntegrityCheckStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new IntegrityCheckStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(IntegrityCheckStatus entityPOCO)
        {
            IntegrityCheckStatusKeys entityKeys = new IntegrityCheckStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 