 
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
   public partial class QuoteOPPropertiesUpdateService:EntityUpdateService<QuoteOPProperties,QuoteOPPropertiesPM, QuoteOPPM>
   {
   
        QuoteOPPropertiesRepository entityRepository;
        public QuoteOPPropertiesUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IQuoteOPMContext  context = mainContext as QuoteOPMContext;
            context = context ??mainContext as IQuoteOPMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new QuoteOPPropertiesDataMapping();
            Repository = new QuoteOPPropertiesRepository(context);
        }

       
        private IQuoteOPMContext currentContext;
        public QuoteOPPropertiesUpdateService(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPPropertiesUpdateService(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(QuoteOPPropertiesPM entityPM)
        {
            QuoteOPPropertiesKeys entityKeys = new QuoteOPPropertiesKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(QuoteOPPropertiesPM entityPM)
        {     		
		    entityPM.Id = IdCounter.GetNumber("QuoteOPProperties", entityPM.Tenant); 					
	    }
        
		protected override void FillDefaultValuesOnUpdate(QuoteOPPropertiesPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 