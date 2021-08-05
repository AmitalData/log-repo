 
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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Amital.QuoteOPM.BL.EntityDataMappings;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Data.EntityKeys;
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{ 
   public partial class QuoteOPSaleChargeUpdateService:EntityUpdateService<QuoteOPSaleCharge,QuoteOPSaleChargePM,EntityPM>
   {
   
        QuoteOPSaleChargeRepository entityRepository;
        public QuoteOPSaleChargeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IQuoteOPMContext  context = mainContext as QuoteOPMContext;
            context = context ??mainContext as IQuoteOPMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new QuoteOPSaleChargeDataMapping();
            Repository = new QuoteOPSaleChargeRepository(context);
        }

       
        private IQuoteOPMContext currentContext;
        public QuoteOPSaleChargeUpdateService(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPSaleChargeUpdateService(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(QuoteOPSaleChargePM entityPM)
        {
            QuoteOPSaleChargeKeys entityKeys = new QuoteOPSaleChargeKeys() {  };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(QuoteOPSaleChargePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(QuoteOPSaleChargePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 