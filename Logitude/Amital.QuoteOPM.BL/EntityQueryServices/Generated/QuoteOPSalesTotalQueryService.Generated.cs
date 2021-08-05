 
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
   public partial class QuoteOPSalesTotalQueryService: EntityQueryService<QuoteOPSalesTotal,QuoteOPSalesTotalKeys,QuoteOPSalesTotalPM,object,QuoteOPSalesTotalKeys>
   {
   
        QuoteOPSalesTotalRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPSalesTotalQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPSalesTotalRepository(context);
            Repository = repository;
            mapping = new QuoteOPSalesTotalDataMapping();
        }

        public QuoteOPSalesTotalQueryService(QuoteOPSalesTotalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPSalesTotalDataMapping();
        }

        public QuoteOPSalesTotalQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPSalesTotalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPSalesTotalDataMapping();
        }
		 
		public  QuoteOPSalesTotalPM GetSingle(,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPSalesTotalKeys(){  };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPSalesTotal entityPOCO)
        {
            QuoteOPSalesTotalKeys entityKeys = new QuoteOPSalesTotalKeys() {  };
            return entityKeys;
        }
     
	 
   }
   
}
	 