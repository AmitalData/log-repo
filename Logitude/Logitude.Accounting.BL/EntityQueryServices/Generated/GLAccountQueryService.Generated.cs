 
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
   public partial class GLAccountQueryService: EntityQueryService<GLAccount,GLAccountKeys,GLAccountPM,object,GLAccountKeys>
   {
   
        GLAccountRepository repository;
		IAccountingContext  context;
        public GLAccountQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountRepository(context);
            Repository = repository;
            mapping = new GLAccountDataMapping();
        }

        public GLAccountQueryService(GLAccountRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountDataMapping();
        }

        public GLAccountQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountDataMapping();
        }
		 
		public  GLAccountPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccount entityPOCO)
        {
            GLAccountKeys entityKeys = new GLAccountKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 