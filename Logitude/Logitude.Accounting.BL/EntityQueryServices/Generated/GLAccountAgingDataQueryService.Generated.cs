 
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
   public partial class GLAccountAgingDataQueryService: EntityQueryService<GLAccountAgingData,GLAccountAgingDataKeys,GLAccountAgingDataPM,object,GLAccountAgingDataKeys>
   {
   
        GLAccountAgingDataRepository repository;
		IAccountingContext  context;
        public GLAccountAgingDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountAgingDataRepository(context);
            Repository = repository;
            mapping = new GLAccountAgingDataDataMapping();
        }

        public GLAccountAgingDataQueryService(GLAccountAgingDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountAgingDataDataMapping();
        }

        public GLAccountAgingDataQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountAgingDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountAgingDataDataMapping();
        }
		 
		public  GLAccountAgingDataPM GetSingle(string accountid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountAgingDataKeys(){ AccountId = accountid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountAgingData entityPOCO)
        {
            GLAccountAgingDataKeys entityKeys = new GLAccountAgingDataKeys() { AccountId = entityPOCO.AccountId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 