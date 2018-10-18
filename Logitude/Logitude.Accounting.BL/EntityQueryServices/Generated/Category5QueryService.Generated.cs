 
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
   public partial class Category5QueryService: EntityQueryService<Category5,Category5Keys,Category5PM,object,Category5Keys>
   {
   
        Category5Repository repository;
		IAccountingContext  context;
        public Category5QueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Category5Repository(context);
            Repository = repository;
            mapping = new Category5DataMapping();
        }

        public Category5QueryService(Category5Repository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Category5DataMapping();
        }

        public Category5QueryService(IAccountingContext context)
        {
            this.repository = new Category5Repository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Category5DataMapping();
        }
		 
		public  Category5PM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Category5Keys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Category5 entityPOCO)
        {
            Category5Keys entityKeys = new Category5Keys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 