 
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
   public partial class CustomerIdentificationTypeQueryService: EntityQueryService<CustomerIdentificationType,CustomerIdentificationTypeKeys,CustomerIdentificationTypePM,object,CustomerIdentificationTypeKeys>
   {
   
        CustomerIdentificationTypeRepository repository;
		ICustomContext  context;
        public CustomerIdentificationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomerIdentificationTypeRepository(context);
            Repository = repository;
            mapping = new CustomerIdentificationTypeDataMapping();
        }

        public CustomerIdentificationTypeQueryService(CustomerIdentificationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomerIdentificationTypeDataMapping();
        }

        public CustomerIdentificationTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomerIdentificationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomerIdentificationTypeDataMapping();
        }
		 
		public  CustomerIdentificationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomerIdentificationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomerIdentificationType entityPOCO)
        {
            CustomerIdentificationTypeKeys entityKeys = new CustomerIdentificationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 