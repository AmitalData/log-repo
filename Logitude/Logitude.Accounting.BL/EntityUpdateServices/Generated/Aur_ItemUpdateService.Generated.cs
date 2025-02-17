 
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
   public partial class Aur_ItemUpdateService:EntityUpdateService<Aur_Item,Aur_ItemPM,Aur_PaymentPM>
   {
   
        Aur_ItemRepository entityRepository;
        public Aur_ItemUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IAccountingContext  context = mainContext as AccountingContext;
            context = context ??mainContext as IAccountingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new Aur_ItemDataMapping();
            Repository = new Aur_ItemRepository(context);
        }

       
        private IAccountingContext currentContext;
        public Aur_ItemUpdateService(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Aur_ItemUpdateService(IAccountingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(Aur_ItemPM entityPM)
        {
            Aur_ItemKeys entityKeys = new Aur_ItemKeys() { PaymentId = entityPM.PaymentId, Line = entityPM.Line };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(Aur_ItemPM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(Aur_ItemPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 