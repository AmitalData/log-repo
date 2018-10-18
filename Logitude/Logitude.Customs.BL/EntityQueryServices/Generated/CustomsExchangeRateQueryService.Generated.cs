 
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
   public partial class CustomsExchangeRateQueryService: EntityQueryService<CustomsExchangeRate,CustomsExchangeRateKeys,CustomsExchangeRatePM,object,CustomsExchangeRateKeys>
   {
   
        CustomsExchangeRateRepository repository;
		ICustomContext  context;
        public CustomsExchangeRateQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsExchangeRateRepository(context);
            Repository = repository;
            mapping = new CustomsExchangeRateDataMapping();
        }

        public CustomsExchangeRateQueryService(CustomsExchangeRateRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsExchangeRateDataMapping();
        }

        public CustomsExchangeRateQueryService(ICustomContext context)
        {
            this.repository = new CustomsExchangeRateRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsExchangeRateDataMapping();
        }
		 
		public  CustomsExchangeRatePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsExchangeRateKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsExchangeRate entityPOCO)
        {
            CustomsExchangeRateKeys entityKeys = new CustomsExchangeRateKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 