 
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
   public partial class CashBookLineUpdateService:EntityUpdateService<CashBookLine,CashBookLinePM,CashBookPM>
   {
   
        CashBookLineRepository entityRepository;
        public CashBookLineUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CashBookLineDataMapping();
            Repository = new CashBookLineRepository(context);
        }

       
        private IAccountingContext currentContext;
        public CashBookLineUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CashBookLineUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CashBookLinePM entityPM)
        {
            CashBookLineKeys entityKeys = new CashBookLineKeys() { CashBookId = entityPM.CashBookId, ARPChequeId = entityPM.ARPChequeId };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CashBookLinePM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(CashBookLinePM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 