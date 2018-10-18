 
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
   public partial class GLAccountMoreDataQueryService: EntityQueryService<GLAccountMoreData,GLAccountMoreDataKeys,GLAccountMoreDataPM,object,GLAccountMoreDataKeys>
   {
   
        GLAccountMoreDataRepository repository;
		IAccountingContext  context;
        public GLAccountMoreDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountMoreDataRepository(context);
            Repository = repository;
            mapping = new GLAccountMoreDataDataMapping();
        }

        public GLAccountMoreDataQueryService(GLAccountMoreDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountMoreDataDataMapping();
        }

        public GLAccountMoreDataQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountMoreDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountMoreDataDataMapping();
        }
		 
		public  GLAccountMoreDataPM GetSingle(string accountid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountMoreDataKeys(){ AccountId = accountid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountMoreData entityPOCO)
        {
            GLAccountMoreDataKeys entityKeys = new GLAccountMoreDataKeys() { AccountId = entityPOCO.AccountId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 