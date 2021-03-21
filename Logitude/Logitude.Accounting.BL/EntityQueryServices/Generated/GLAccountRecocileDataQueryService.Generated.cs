 
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
   public partial class GLAccountRecocileDataQueryService: EntityQueryService<GLAccountRecocileData,GLAccountRecocileDataKeys,GLAccountRecocileDataPM,object,GLAccountRecocileDataKeys>
   {
   
        GLAccountRecocileDataRepository repository;
		IAccountingContext  context;
        public GLAccountRecocileDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountRecocileDataRepository(context);
            Repository = repository;
            mapping = new GLAccountRecocileDataDataMapping();
        }

        public GLAccountRecocileDataQueryService(GLAccountRecocileDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountRecocileDataDataMapping();
        }

        public GLAccountRecocileDataQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountRecocileDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountRecocileDataDataMapping();
        }
		 
		public  GLAccountRecocileDataPM GetSingle(string accountid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountRecocileDataKeys(){ AccountId = accountid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountRecocileData entityPOCO)
        {
            GLAccountRecocileDataKeys entityKeys = new GLAccountRecocileDataKeys() { AccountId = entityPOCO.AccountId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 