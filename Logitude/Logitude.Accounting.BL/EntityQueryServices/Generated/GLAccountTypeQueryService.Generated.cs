 
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
   public partial class GLAccountTypeQueryService: EntityQueryService<GLAccountType,GLAccountTypeKeys,GLAccountTypePM,object,GLAccountTypeKeys>
   {
   
        GLAccountTypeRepository repository;
		IAccountingContext  context;
        public GLAccountTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountTypeRepository(context);
            Repository = repository;
            mapping = new GLAccountTypeDataMapping();
        }

        public GLAccountTypeQueryService(GLAccountTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountTypeDataMapping();
        }

        public GLAccountTypeQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountTypeDataMapping();
        }
		 
		public  GLAccountTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountType entityPOCO)
        {
            GLAccountTypeKeys entityKeys = new GLAccountTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 