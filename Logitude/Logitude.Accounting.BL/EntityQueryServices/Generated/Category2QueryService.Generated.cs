 
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
   public partial class Category2QueryService: EntityQueryService<Category2,Category2Keys,Category2PM,object,Category2Keys>
   {
   
        Category2Repository repository;
		IAccountingContext  context;
        public Category2QueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Category2Repository(context);
            Repository = repository;
            mapping = new Category2DataMapping();
        }

        public Category2QueryService(Category2Repository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Category2DataMapping();
        }

        public Category2QueryService(IAccountingContext context)
        {
            this.repository = new Category2Repository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Category2DataMapping();
        }
		 
		public  Category2PM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Category2Keys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Category2 entityPOCO)
        {
            Category2Keys entityKeys = new Category2Keys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 