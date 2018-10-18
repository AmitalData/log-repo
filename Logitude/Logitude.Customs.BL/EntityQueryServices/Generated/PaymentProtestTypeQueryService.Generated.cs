 
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
   public partial class PaymentProtestTypeQueryService: EntityQueryService<PaymentProtestType,PaymentProtestTypeKeys,PaymentProtestTypePM,object,PaymentProtestTypeKeys>
   {
   
        PaymentProtestTypeRepository repository;
		ICustomContext  context;
        public PaymentProtestTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentProtestTypeRepository(context);
            Repository = repository;
            mapping = new PaymentProtestTypeDataMapping();
        }

        public PaymentProtestTypeQueryService(PaymentProtestTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentProtestTypeDataMapping();
        }

        public PaymentProtestTypeQueryService(ICustomContext context)
        {
            this.repository = new PaymentProtestTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentProtestTypeDataMapping();
        }
		 
		public  PaymentProtestTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentProtestTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentProtestType entityPOCO)
        {
            PaymentProtestTypeKeys entityKeys = new PaymentProtestTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 