 
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
using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class ReconcileCurrencyTypeUpdateService:EntityUpdateService<ReconcileCurrencyType,ReconcileCurrencyTypePM,EntityPM>
   {
   
        ReconcileCurrencyTypeRepository entityRepository;
        public ReconcileCurrencyTypeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            Mapping = new ReconcileCurrencyTypeDataMapping();
            Repository = new ReconcileCurrencyTypeRepository(context);
        }

       
        private IAccountingContext currentContext;
        public ReconcileCurrencyTypeUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconcileCurrencyTypeUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ReconcileCurrencyTypePM entityPM)
        {
            ReconcileCurrencyTypeKeys entityKeys = new ReconcileCurrencyTypeKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(ReconcileCurrencyTypePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(ReconcileCurrencyTypePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 