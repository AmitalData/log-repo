 
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
   public partial class Category1QueryService: EntityQueryService<Category1,Category1Keys,Category1PM,object,Category1Keys>
   {
   
        Category1Repository repository;
		IAccountingContext  context;
        public Category1QueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Category1Repository(context);
            Repository = repository;
            mapping = new Category1DataMapping();
        }

        public Category1QueryService(Category1Repository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Category1DataMapping();
        }

        public Category1QueryService(IAccountingContext context)
        {
            this.repository = new Category1Repository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Category1DataMapping();
        }
		 
		public  Category1PM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Category1Keys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Category1 entityPOCO)
        {
            Category1Keys entityKeys = new Category1Keys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 