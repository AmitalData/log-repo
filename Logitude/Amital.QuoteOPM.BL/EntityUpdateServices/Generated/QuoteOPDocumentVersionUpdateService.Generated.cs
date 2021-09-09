 
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
   public partial class QuoteOPDocumentVersionUpdateService:EntityUpdateService<QuoteOPDocumentVersion,QuoteOPDocumentVersionPM,QuoteOPPM>
   {
   
        QuoteOPDocumentVersionRepository entityRepository;
        public QuoteOPDocumentVersionUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IQuoteOPMContext  context = mainContext as QuoteOPMContext;
            context = context ??mainContext as IQuoteOPMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new QuoteOPDocumentVersionDataMapping();
            Repository = new QuoteOPDocumentVersionRepository(context);
        }

       
        private IQuoteOPMContext currentContext;
        public QuoteOPDocumentVersionUpdateService(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPDocumentVersionUpdateService(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(QuoteOPDocumentVersionPM entityPM)
        {
            QuoteOPDocumentVersionKeys entityKeys = new QuoteOPDocumentVersionKeys() { QuoteOPId = entityPM.QuoteOPId, VersionNumber = entityPM.VersionNumber };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(QuoteOPDocumentVersionPM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(QuoteOPDocumentVersionPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 