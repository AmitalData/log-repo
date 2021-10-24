 
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
   public partial class ARPaymentBankTranferUpdateService:EntityUpdateService<ARPaymentBankTranfer,ARPaymentBankTranferPM,EntityPM>
   {
   
        ARPaymentBankTranferRepository entityRepository;
        public ARPaymentBankTranferUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ARPaymentBankTranferDataMapping();
            Repository = new ARPaymentBankTranferRepository(context);
        }

       
        private IAccountingContext currentContext;
        public ARPaymentBankTranferUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ARPaymentBankTranferUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ARPaymentBankTranferPM entityPM)
        {
            ARPaymentBankTranferKeys entityKeys = new ARPaymentBankTranferKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(ARPaymentBankTranferPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("ARPaymentBankTranfer", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(ARPaymentBankTranferPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 