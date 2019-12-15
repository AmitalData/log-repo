 
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
   public partial class GLAccountInterestPeriodUpdateService:EntityUpdateService<GLAccountInterestPeriod,GLAccountInterestPeriodPM,GLAccountPM>
   {
   
        GLAccountInterestPeriodRepository entityRepository;
        public GLAccountInterestPeriodUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new GLAccountInterestPeriodDataMapping();
            Repository = new GLAccountInterestPeriodRepository(context);
        }

       
        private IAccountingContext currentContext;
        public GLAccountInterestPeriodUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountInterestPeriodUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(GLAccountInterestPeriodPM entityPM)
        {
            GLAccountInterestPeriodKeys entityKeys = new GLAccountInterestPeriodKeys() { LineNumber = entityPM.LineNumber, GLAccountId = entityPM.GLAccountId };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(GLAccountInterestPeriodPM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(GLAccountInterestPeriodPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 