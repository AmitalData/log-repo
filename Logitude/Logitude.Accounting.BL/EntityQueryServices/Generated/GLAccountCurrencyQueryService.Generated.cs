 
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
   public partial class GLAccountCurrencyQueryService: EntityQueryService<GLAccountCurrency,GLAccountCurrencyKeys,GLAccountCurrencyPM,GLAccountPM,GLAccountKeys>
   {
   
        GLAccountCurrencyRepository repository;
		IAccountingContext  context;
        public GLAccountCurrencyQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountCurrencyRepository(context);
            Repository = repository;
            mapping = new GLAccountCurrencyDataMapping();
        }

        public GLAccountCurrencyQueryService(GLAccountCurrencyRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountCurrencyDataMapping();
        }

        public GLAccountCurrencyQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountCurrencyRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountCurrencyDataMapping();
        }
		 
		public  GLAccountCurrencyPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountCurrencyKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountCurrency entityPOCO)
        {
            GLAccountCurrencyKeys entityKeys = new GLAccountCurrencyKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 