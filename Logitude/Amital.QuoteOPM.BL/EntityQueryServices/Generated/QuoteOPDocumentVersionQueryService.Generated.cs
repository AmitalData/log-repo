 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Amital.QuoteOPM.BL.EntityDataMappings;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Data.EntityKeys;
using Amital.QuoteOPM.Data;
using Simplog.Server.Infrastructure;
namespace Amital.QuoteOPM.BL.EntityQueryServices
{ 
   public partial class QuoteOPDocumentVersionQueryService: EntityQueryService<QuoteOPDocumentVersion,QuoteOPDocumentVersionKeys,QuoteOPDocumentVersionPM,QuoteOPPM,QuoteOPKeys>
   {
   
        QuoteOPDocumentVersionRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPDocumentVersionQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPDocumentVersionRepository(context);
            Repository = repository;
            mapping = new QuoteOPDocumentVersionDataMapping();
        }

        public QuoteOPDocumentVersionQueryService(QuoteOPDocumentVersionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPDocumentVersionDataMapping();
        }

        public QuoteOPDocumentVersionQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPDocumentVersionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPDocumentVersionDataMapping();
        }
		 
		public  QuoteOPDocumentVersionPM GetSingle(string quoteopid, int versionnumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPDocumentVersionKeys(){ QuoteOPId = quoteopid, VersionNumber = versionnumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPDocumentVersion entityPOCO)
        {
            QuoteOPDocumentVersionKeys entityKeys = new QuoteOPDocumentVersionKeys() { QuoteOPId = entityPOCO.QuoteOPId, VersionNumber = entityPOCO.VersionNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 