 
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
   public partial class QuoteOPCustomerTypeQueryService: EntityQueryService<QuoteOPCustomerType,QuoteOPCustomerTypeKeys,QuoteOPCustomerTypePM,object,QuoteOPCustomerTypeKeys>
   {
   
        QuoteOPCustomerTypeRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPCustomerTypeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPCustomerTypeRepository(context);
            Repository = repository;
            mapping = new QuoteOPCustomerTypeDataMapping();
        }

        public QuoteOPCustomerTypeQueryService(QuoteOPCustomerTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPCustomerTypeDataMapping();
        }

        public QuoteOPCustomerTypeQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPCustomerTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPCustomerTypeDataMapping();
        }
		 
		public  QuoteOPCustomerTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPCustomerTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPCustomerType entityPOCO)
        {
            QuoteOPCustomerTypeKeys entityKeys = new QuoteOPCustomerTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 