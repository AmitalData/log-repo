 
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
   public partial class QuoteOPTemplateTextDesignQueryService: EntityQueryService<QuoteOPTemplateTextDesign,QuoteOPTemplateTextDesignKeys,QuoteOPTemplateTextDesignPM,object,QuoteOPTemplateTextDesignKeys>
   {
   
        QuoteOPTemplateTextDesignRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPTemplateTextDesignQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPTemplateTextDesignRepository(context);
            Repository = repository;
            mapping = new QuoteOPTemplateTextDesignDataMapping();
        }

        public QuoteOPTemplateTextDesignQueryService(QuoteOPTemplateTextDesignRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPTemplateTextDesignDataMapping();
        }

        public QuoteOPTemplateTextDesignQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPTemplateTextDesignRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPTemplateTextDesignDataMapping();
        }
		 
		public  QuoteOPTemplateTextDesignPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPTemplateTextDesignKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPTemplateTextDesign entityPOCO)
        {
            QuoteOPTemplateTextDesignKeys entityKeys = new QuoteOPTemplateTextDesignKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 