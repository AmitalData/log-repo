 
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
   public partial class PaymentOrderTypeQueryService: EntityQueryService<PaymentOrderType,PaymentOrderTypeKeys,PaymentOrderTypePM,object,PaymentOrderTypeKeys>
   {
   
        PaymentOrderTypeRepository repository;
		ICustomContext  context;
        public PaymentOrderTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentOrderTypeRepository(context);
            Repository = repository;
            mapping = new PaymentOrderTypeDataMapping();
        }

        public PaymentOrderTypeQueryService(PaymentOrderTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentOrderTypeDataMapping();
        }

        public PaymentOrderTypeQueryService(ICustomContext context)
        {
            this.repository = new PaymentOrderTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentOrderTypeDataMapping();
        }
		 
		public  PaymentOrderTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentOrderTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentOrderType entityPOCO)
        {
            PaymentOrderTypeKeys entityKeys = new PaymentOrderTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 