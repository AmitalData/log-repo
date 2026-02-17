 
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
   public partial class CustomerRoleTypeQueryService: EntityQueryService<CustomerRoleType,CustomerRoleTypeKeys,CustomerRoleTypePM,object,CustomerRoleTypeKeys>
   {
   
        CustomerRoleTypeRepository repository;
		ICustomContext  context;
        public CustomerRoleTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomerRoleTypeRepository(context);
            Repository = repository;
            mapping = new CustomerRoleTypeDataMapping();
        }

        public CustomerRoleTypeQueryService(CustomerRoleTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomerRoleTypeDataMapping();
        }

        public CustomerRoleTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomerRoleTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomerRoleTypeDataMapping();
        }
		 
		public  CustomerRoleTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomerRoleTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomerRoleType entityPOCO)
        {
            CustomerRoleTypeKeys entityKeys = new CustomerRoleTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 