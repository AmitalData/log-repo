 
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
   public partial class CustomerClassificationTypeQueryService: EntityQueryService<CustomerClassificationType,CustomerClassificationTypeKeys,CustomerClassificationTypePM,object,CustomerClassificationTypeKeys>
   {
   
        CustomerClassificationTypeRepository repository;
		ICustomContext  context;
        public CustomerClassificationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomerClassificationTypeRepository(context);
            Repository = repository;
            mapping = new CustomerClassificationTypeDataMapping();
        }

        public CustomerClassificationTypeQueryService(CustomerClassificationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomerClassificationTypeDataMapping();
        }

        public CustomerClassificationTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomerClassificationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomerClassificationTypeDataMapping();
        }
		 
		public  CustomerClassificationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomerClassificationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomerClassificationType entityPOCO)
        {
            CustomerClassificationTypeKeys entityKeys = new CustomerClassificationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 