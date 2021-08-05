 
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
   public partial class QuoteOPTypeQueryService: EntityQueryService<QuoteOPType,QuoteOPTypeKeys,QuoteOPTypePM,object,QuoteOPTypeKeys>
   {
   
        QuoteOPTypeRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTypeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTypeRepository(context);
            Repository = repository;
            mapping = new QuoteOPTypeDataMapping();
        }

        public QuoteOPTypeQueryService(QuoteOPTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTypeDataMapping();
        }

        public QuoteOPTypeQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTypeDataMapping();
        }
		 
		public  QuoteOPTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPType entityPOCO)
        {
            QuoteOPTypeKeys entityKeys = new QuoteOPTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 