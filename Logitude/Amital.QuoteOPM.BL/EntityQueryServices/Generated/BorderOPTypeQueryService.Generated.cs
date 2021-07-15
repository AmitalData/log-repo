 
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
   public partial class BorderOPTypeQueryService: EntityQueryService<BorderOPType,BorderOPTypeKeys,BorderOPTypePM,object,BorderOPTypeKeys>
   {
   
        BorderOPTypeRepository repository;
		IQuoteOPMContext  context;
        public BorderOPTypeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new BorderOPTypeRepository(context);
            Repository = repository;
            mapping = new BorderOPTypeDataMapping();
        }

        public BorderOPTypeQueryService(BorderOPTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BorderOPTypeDataMapping();
        }

        public BorderOPTypeQueryService(IQuoteOPMContext context)
        {
            this.repository = new BorderOPTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BorderOPTypeDataMapping();
        }
		 
		public  BorderOPTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BorderOPTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BorderOPType entityPOCO)
        {
            BorderOPTypeKeys entityKeys = new BorderOPTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 