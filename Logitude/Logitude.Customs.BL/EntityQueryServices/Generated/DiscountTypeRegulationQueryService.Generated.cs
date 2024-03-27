 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class DiscountTypeRegulationQueryService: EntityQueryService<DiscountTypeRegulation,DiscountTypeRegulationKeys,DiscountTypeRegulationPM,object,DiscountTypeRegulationKeys>
   {
   
        DiscountTypeRegulationRepository repository;
		ICustomContext  context;
        public DiscountTypeRegulationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DiscountTypeRegulationRepository(context);
            Repository = repository;
            mapping = new DiscountTypeRegulationDataMapping();
        }

        public DiscountTypeRegulationQueryService(DiscountTypeRegulationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DiscountTypeRegulationDataMapping();
        }

        public DiscountTypeRegulationQueryService(ICustomContext context)
        {
            this.repository = new DiscountTypeRegulationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DiscountTypeRegulationDataMapping();
        }
		 
		public  DiscountTypeRegulationPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DiscountTypeRegulationKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DiscountTypeRegulation entityPOCO)
        {
            DiscountTypeRegulationKeys entityKeys = new DiscountTypeRegulationKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 