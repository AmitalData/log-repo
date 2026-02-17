 
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
   public partial class PaymentOrderMethodQueryService: EntityQueryService<PaymentOrderMethod,PaymentOrderMethodKeys,PaymentOrderMethodPM,PaymentOrderPM,PaymentOrderKeys>
   {
   
        PaymentOrderMethodRepository repository;
		ICustomContext  context;
        public PaymentOrderMethodQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentOrderMethodRepository(context);
            Repository = repository;
            mapping = new PaymentOrderMethodDataMapping();
        }

        public PaymentOrderMethodQueryService(PaymentOrderMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentOrderMethodDataMapping();
        }

        public PaymentOrderMethodQueryService(ICustomContext context)
        {
            this.repository = new PaymentOrderMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentOrderMethodDataMapping();
        }
		 
		public  PaymentOrderMethodPM GetSingle(string paymentorderid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentOrderMethodKeys(){ PaymentOrderId = paymentorderid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentOrderMethod entityPOCO)
        {
            PaymentOrderMethodKeys entityKeys = new PaymentOrderMethodKeys() { PaymentOrderId = entityPOCO.PaymentOrderId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 