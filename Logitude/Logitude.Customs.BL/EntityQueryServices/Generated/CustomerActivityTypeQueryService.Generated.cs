 
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
   public partial class CustomerActivityTypeQueryService: EntityQueryService<CustomerActivityType,CustomerActivityTypeKeys,CustomerActivityTypePM,object,CustomerActivityTypeKeys>
   {
   
        CustomerActivityTypeRepository repository;
		ICustomContext  context;
        public CustomerActivityTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomerActivityTypeRepository(context);
            Repository = repository;
            mapping = new CustomerActivityTypeDataMapping();
        }

        public CustomerActivityTypeQueryService(CustomerActivityTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomerActivityTypeDataMapping();
        }

        public CustomerActivityTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomerActivityTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomerActivityTypeDataMapping();
        }
		 
		public  CustomerActivityTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomerActivityTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomerActivityType entityPOCO)
        {
            CustomerActivityTypeKeys entityKeys = new CustomerActivityTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 