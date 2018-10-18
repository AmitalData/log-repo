 
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
   public partial class Category3QueryService: EntityQueryService<Category3,Category3Keys,Category3PM,object,Category3Keys>
   {
   
        Category3Repository repository;
		IAccountingContext  context;
        public Category3QueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Category3Repository(context);
            Repository = repository;
            mapping = new Category3DataMapping();
        }

        public Category3QueryService(Category3Repository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Category3DataMapping();
        }

        public Category3QueryService(IAccountingContext context)
        {
            this.repository = new Category3Repository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Category3DataMapping();
        }
		 
		public  Category3PM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Category3Keys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Category3 entityPOCO)
        {
            Category3Keys entityKeys = new Category3Keys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 