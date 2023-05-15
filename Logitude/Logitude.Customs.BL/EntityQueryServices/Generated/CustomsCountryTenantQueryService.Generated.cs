 
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
   public partial class CustomsCountryTenantQueryService: EntityQueryService<CustomsCountryTenant,CustomsCountryTenantKeys,CustomsCountryTenantPM,object,CustomsCountryTenantKeys>
   {
   
        CustomsCountryTenantRepository repository;
		ICustomContext  context;
        public CustomsCountryTenantQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsCountryTenantRepository(context);
            Repository = repository;
            mapping = new CustomsCountryTenantDataMapping();
        }

        public CustomsCountryTenantQueryService(CustomsCountryTenantRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsCountryTenantDataMapping();
        }

        public CustomsCountryTenantQueryService(ICustomContext context)
        {
            this.repository = new CustomsCountryTenantRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsCountryTenantDataMapping();
        }
		 
		public  CustomsCountryTenantPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsCountryTenantKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsCountryTenant entityPOCO)
        {
            CustomsCountryTenantKeys entityKeys = new CustomsCountryTenantKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 