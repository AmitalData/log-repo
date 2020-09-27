 
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
   public partial class CurrencyTypeTenantQueryService: EntityQueryService<CurrencyTypeTenant,CurrencyTypeTenantKeys,CurrencyTypeTenantPM,object,CurrencyTypeTenantKeys>
   {
   
        CurrencyTypeTenantRepository repository;
		ICustomContext  context;
        public CurrencyTypeTenantQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CurrencyTypeTenantRepository(context);
            Repository = repository;
            mapping = new CurrencyTypeTenantDataMapping();
        }

        public CurrencyTypeTenantQueryService(CurrencyTypeTenantRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CurrencyTypeTenantDataMapping();
        }

        public CurrencyTypeTenantQueryService(ICustomContext context)
        {
            this.repository = new CurrencyTypeTenantRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CurrencyTypeTenantDataMapping();
        }
		 
		public  CurrencyTypeTenantPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CurrencyTypeTenantKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CurrencyTypeTenant entityPOCO)
        {
            CurrencyTypeTenantKeys entityKeys = new CurrencyTypeTenantKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 