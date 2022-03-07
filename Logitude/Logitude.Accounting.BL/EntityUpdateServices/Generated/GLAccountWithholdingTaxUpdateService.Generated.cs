 
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
   public partial class GLAccountWithholdingTaxUpdateService:EntityUpdateService<GLAccountWithholdingTax,GLAccountWithholdingTaxPM,GLAccountPM>
   {
   
        GLAccountWithholdingTaxRepository entityRepository;
        public GLAccountWithholdingTaxUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new GLAccountWithholdingTaxDataMapping();
            Repository = new GLAccountWithholdingTaxRepository(context);
        }

       
        private IAccountingContext currentContext;
        public GLAccountWithholdingTaxUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountWithholdingTaxUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(GLAccountWithholdingTaxPM entityPM)
        {
            GLAccountWithholdingTaxKeys entityKeys = new GLAccountWithholdingTaxKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(GLAccountWithholdingTaxPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("GLAccountWithholdingTax", entityPM.Tenant); 
					
			DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
							
		    entityPM.CreateDate =  myDate;
					 
	    }
        
		protected override void FillDefaultValuesOnUpdate(GLAccountWithholdingTaxPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 