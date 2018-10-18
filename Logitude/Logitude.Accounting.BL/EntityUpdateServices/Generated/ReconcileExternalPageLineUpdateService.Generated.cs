 
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
   public partial class ReconcileExternalPageLineUpdateService:EntityUpdateService<ReconcileExternalPageLine,ReconcileExternalPageLinePM,ReconcileExternalPagePM>
   {
   
        ReconcileExternalPageLineRepository entityRepository;
        public ReconcileExternalPageLineUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ReconcileExternalPageLineDataMapping();
            Repository = new ReconcileExternalPageLineRepository(context);
        }

       
        private IAccountingContext currentContext;
        public ReconcileExternalPageLineUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconcileExternalPageLineUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ReconcileExternalPageLinePM entityPM)
        {
            ReconcileExternalPageLineKeys entityKeys = new ReconcileExternalPageLineKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(ReconcileExternalPageLinePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(ReconcileExternalPageLinePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 