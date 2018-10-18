 
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
   public partial class GLAccountTotalDateTypeQueryService: EntityQueryService<GLAccountTotalDateType,GLAccountTotalDateTypeKeys,GLAccountTotalDateTypePM,object,GLAccountTotalDateTypeKeys>
   {
   
        GLAccountTotalDateTypeRepository repository;
		IAccountingContext  context;
        public GLAccountTotalDateTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountTotalDateTypeRepository(context);
            Repository = repository;
            mapping = new GLAccountTotalDateTypeDataMapping();
        }

        public GLAccountTotalDateTypeQueryService(GLAccountTotalDateTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountTotalDateTypeDataMapping();
        }

        public GLAccountTotalDateTypeQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountTotalDateTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountTotalDateTypeDataMapping();
        }
		 
		public  GLAccountTotalDateTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountTotalDateTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountTotalDateType entityPOCO)
        {
            GLAccountTotalDateTypeKeys entityKeys = new GLAccountTotalDateTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 