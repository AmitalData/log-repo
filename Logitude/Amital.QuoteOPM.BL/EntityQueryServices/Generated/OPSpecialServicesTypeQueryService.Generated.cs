 
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
   public partial class OPSpecialServicesTypeQueryService: EntityQueryService<OPSpecialServicesType,OPSpecialServicesTypeKeys,OPSpecialServicesTypePM,object,OPSpecialServicesTypeKeys>
   {
   
        OPSpecialServicesTypeRepository repository;
		IQuoteOPMContext  context;
        public OPSpecialServicesTypeQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new OPSpecialServicesTypeRepository(context);
            Repository = repository;
            mapping = new OPSpecialServicesTypeDataMapping();
        }

        public OPSpecialServicesTypeQueryService(OPSpecialServicesTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OPSpecialServicesTypeDataMapping();
        }

        public OPSpecialServicesTypeQueryService(IQuoteOPMContext context)
        {
            this.repository = new OPSpecialServicesTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OPSpecialServicesTypeDataMapping();
        }
		 
		public  OPSpecialServicesTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OPSpecialServicesTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OPSpecialServicesType entityPOCO)
        {
            OPSpecialServicesTypeKeys entityKeys = new OPSpecialServicesTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 