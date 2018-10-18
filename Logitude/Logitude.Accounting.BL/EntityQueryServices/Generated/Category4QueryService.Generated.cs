 
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
   public partial class Category4QueryService: EntityQueryService<Category4,Category4Keys,Category4PM,object,Category4Keys>
   {
   
        Category4Repository repository;
		IAccountingContext  context;
        public Category4QueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Category4Repository(context);
            Repository = repository;
            mapping = new Category4DataMapping();
        }

        public Category4QueryService(Category4Repository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Category4DataMapping();
        }

        public Category4QueryService(IAccountingContext context)
        {
            this.repository = new Category4Repository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Category4DataMapping();
        }
		 
		public  Category4PM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Category4Keys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Category4 entityPOCO)
        {
            Category4Keys entityKeys = new Category4Keys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 