 
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
   public partial class QuoteOPVATsTotalQueryService: EntityQueryService<QuoteOPVATsTotal,QuoteOPVATsTotalKeys,QuoteOPVATsTotalPM,object,QuoteOPVATsTotalKeys>
   {
   
        QuoteOPVATsTotalRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPVATsTotalQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPVATsTotalRepository(context);
            Repository = repository;
            mapping = new QuoteOPVATsTotalDataMapping();
        }

        public QuoteOPVATsTotalQueryService(QuoteOPVATsTotalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPVATsTotalDataMapping();
        }

        public QuoteOPVATsTotalQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPVATsTotalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPVATsTotalDataMapping();
        }
		 
		public  QuoteOPVATsTotalPM GetSingle(,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPVATsTotalKeys(){  };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPVATsTotal entityPOCO)
        {
            QuoteOPVATsTotalKeys entityKeys = new QuoteOPVATsTotalKeys() {  };
            return entityKeys;
        }
     
	 
   }
   
}
	 