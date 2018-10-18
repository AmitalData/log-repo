 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class Category1UpdateService:EntityUpdateService<Category1,Category1PM,EntityPM>
   {
   
        Category1Repository entityRepository;
        public Category1UpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new Category1DataMapping();
            Repository = new Category1Repository(context);
        }

       
        private IAccountingContext currentContext;
        public Category1UpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Category1UpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(Category1PM entityPM)
        {
            Category1Keys entityKeys = new Category1Keys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(Category1PM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("Category1", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(Category1PM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 