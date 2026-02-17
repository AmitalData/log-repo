 
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
   public partial class GLAccountTotalByMonthUpdateService:EntityUpdateService<GLAccountTotalByMonth,GLAccountTotalByMonthPM,EntityPM>
   {
   
        GLAccountTotalByMonthRepository entityRepository;
        public GLAccountTotalByMonthUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new GLAccountTotalByMonthDataMapping();
            Repository = new GLAccountTotalByMonthRepository(context);
        }

       
        private IAccountingContext currentContext;
        public GLAccountTotalByMonthUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountTotalByMonthUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(GLAccountTotalByMonthPM entityPM)
        {
            GLAccountTotalByMonthKeys entityKeys = new GLAccountTotalByMonthKeys() { AccountId = entityPM.AccountId, DateTypeCode = entityPM.DateTypeCode, Year = entityPM.Year, Month = entityPM.Month, CurrencyId = entityPM.CurrencyId };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(GLAccountTotalByMonthPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(GLAccountTotalByMonthPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 