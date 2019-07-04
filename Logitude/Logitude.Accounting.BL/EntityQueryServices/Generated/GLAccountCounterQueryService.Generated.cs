 
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
   public partial class GLAccountCounterQueryService: EntityQueryService<GLAccountCounter,GLAccountCounterKeys,GLAccountCounterPM,object,GLAccountCounterKeys>
   {
   
        GLAccountCounterRepository repository;
		IAccountingContext  context;
        public GLAccountCounterQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountCounterRepository(context);
            Repository = repository;
            mapping = new GLAccountCounterDataMapping();
        }

        public GLAccountCounterQueryService(GLAccountCounterRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountCounterDataMapping();
        }

        public GLAccountCounterQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountCounterRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountCounterDataMapping();
        }
		 
		public  GLAccountCounterPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountCounterKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountCounter entityPOCO)
        {
            GLAccountCounterKeys entityKeys = new GLAccountCounterKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 