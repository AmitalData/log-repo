 
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
   public partial class RefundCustomerActivityTypeQueryService: EntityQueryService<RefundCustomerActivityType,RefundCustomerActivityTypeKeys,RefundCustomerActivityTypePM,object,RefundCustomerActivityTypeKeys>
   {
   
        RefundCustomerActivityTypeRepository repository;
		ICustomContext  context;
        public RefundCustomerActivityTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RefundCustomerActivityTypeRepository(context);
            Repository = repository;
            mapping = new RefundCustomerActivityTypeDataMapping();
        }

        public RefundCustomerActivityTypeQueryService(RefundCustomerActivityTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RefundCustomerActivityTypeDataMapping();
        }

        public RefundCustomerActivityTypeQueryService(ICustomContext context)
        {
            this.repository = new RefundCustomerActivityTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RefundCustomerActivityTypeDataMapping();
        }
		 
		public  RefundCustomerActivityTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RefundCustomerActivityTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RefundCustomerActivityType entityPOCO)
        {
            RefundCustomerActivityTypeKeys entityKeys = new RefundCustomerActivityTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 