 
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
   public partial class MarkUpOPTypeQueryService: EntityQueryService<MarkUpOPType,MarkUpOPTypeKeys,MarkUpOPTypePM,object,MarkUpOPTypeKeys>
   {
   
        MarkUpOPTypeRepository repository;
		IQuoteOPMContext  context;
        public MarkUpOPTypeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new MarkUpOPTypeRepository(context);
            Repository = repository;
            mapping = new MarkUpOPTypeDataMapping();
        }

        public MarkUpOPTypeQueryService(MarkUpOPTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MarkUpOPTypeDataMapping();
        }

        public MarkUpOPTypeQueryService(IQuoteOPMContext context)
        {
            this.repository = new MarkUpOPTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MarkUpOPTypeDataMapping();
        }
		 
		public  MarkUpOPTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MarkUpOPTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MarkUpOPType entityPOCO)
        {
            MarkUpOPTypeKeys entityKeys = new MarkUpOPTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 