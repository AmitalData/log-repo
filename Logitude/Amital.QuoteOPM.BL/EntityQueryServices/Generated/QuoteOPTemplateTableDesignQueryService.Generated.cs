 
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
   public partial class QuoteOPTemplateTableDesignQueryService: EntityQueryService<QuoteOPTemplateTableDesign,QuoteOPTemplateTableDesignKeys,QuoteOPTemplateTableDesignPM,object,QuoteOPTemplateTableDesignKeys>
   {
   
        QuoteOPTemplateTableDesignRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTemplateTableDesignQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTemplateTableDesignRepository(context);
            Repository = repository;
            mapping = new QuoteOPTemplateTableDesignDataMapping();
        }

        public QuoteOPTemplateTableDesignQueryService(QuoteOPTemplateTableDesignRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTemplateTableDesignDataMapping();
        }

        public QuoteOPTemplateTableDesignQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTemplateTableDesignRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTemplateTableDesignDataMapping();
        }
		 
		public  QuoteOPTemplateTableDesignPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTemplateTableDesignKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPTemplateTableDesign entityPOCO)
        {
            QuoteOPTemplateTableDesignKeys entityKeys = new QuoteOPTemplateTableDesignKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 