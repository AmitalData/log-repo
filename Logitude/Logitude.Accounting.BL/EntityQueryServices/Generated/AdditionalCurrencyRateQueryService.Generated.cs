 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class AdditionalCurrencyRateQueryService: EntityQueryService<AdditionalCurrencyRate,AdditionalCurrencyRateKeys,AdditionalCurrencyRatePM,object,AdditionalCurrencyRateKeys>
   {
   
        AdditionalCurrencyRateRepository repository;
		IAccountingContext  context;
        public AdditionalCurrencyRateQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new AdditionalCurrencyRateRepository(context);
            Repository = repository;
            mapping = new AdditionalCurrencyRateDataMapping();
        }

        public AdditionalCurrencyRateQueryService(AdditionalCurrencyRateRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AdditionalCurrencyRateDataMapping();
        }

        public AdditionalCurrencyRateQueryService(IAccountingContext context)
        {
            this.repository = new AdditionalCurrencyRateRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AdditionalCurrencyRateDataMapping();
        }
		 
		public  AdditionalCurrencyRatePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AdditionalCurrencyRateKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AdditionalCurrencyRate entityPOCO)
        {
            AdditionalCurrencyRateKeys entityKeys = new AdditionalCurrencyRateKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 