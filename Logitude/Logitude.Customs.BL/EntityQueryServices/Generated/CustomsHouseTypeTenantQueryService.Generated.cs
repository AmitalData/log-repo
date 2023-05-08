 
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
   public partial class CustomsHouseTypeTenantQueryService: EntityQueryService<CustomsHouseTypeTenant,CustomsHouseTypeTenantKeys,CustomsHouseTypeTenantPM,object,CustomsHouseTypeTenantKeys>
   {
   
        CustomsHouseTypeTenantRepository repository;
		ICustomContext  context;
        public CustomsHouseTypeTenantQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsHouseTypeTenantRepository(context);
            Repository = repository;
            mapping = new CustomsHouseTypeTenantDataMapping();
        }

        public CustomsHouseTypeTenantQueryService(CustomsHouseTypeTenantRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsHouseTypeTenantDataMapping();
        }

        public CustomsHouseTypeTenantQueryService(ICustomContext context)
        {
            this.repository = new CustomsHouseTypeTenantRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsHouseTypeTenantDataMapping();
        }
		 
		public  CustomsHouseTypeTenantPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsHouseTypeTenantKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsHouseTypeTenant entityPOCO)
        {
            CustomsHouseTypeTenantKeys entityKeys = new CustomsHouseTypeTenantKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 