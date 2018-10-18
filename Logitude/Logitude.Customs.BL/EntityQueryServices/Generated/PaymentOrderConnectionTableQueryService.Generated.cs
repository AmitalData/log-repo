 
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
   public partial class PaymentOrderConnectionTableQueryService: EntityQueryService<PaymentOrderConnectionTable,PaymentOrderConnectionTableKeys,PaymentOrderConnectionTablePM,PaymentOrderPM,PaymentOrderKeys>
   {
   
        PaymentOrderConnectionTableRepository repository;
		ICustomContext  context;
        public PaymentOrderConnectionTableQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentOrderConnectionTableRepository(context);
            Repository = repository;
            mapping = new PaymentOrderConnectionTableDataMapping();
        }

        public PaymentOrderConnectionTableQueryService(PaymentOrderConnectionTableRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentOrderConnectionTableDataMapping();
        }

        public PaymentOrderConnectionTableQueryService(ICustomContext context)
        {
            this.repository = new PaymentOrderConnectionTableRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentOrderConnectionTableDataMapping();
        }
		 
		public  PaymentOrderConnectionTablePM GetSingle(string paymentorderid, string connectedentityid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentOrderConnectionTableKeys(){ PaymentOrderId = paymentorderid, ConnectedEntityId = connectedentityid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentOrderConnectionTable entityPOCO)
        {
            PaymentOrderConnectionTableKeys entityKeys = new PaymentOrderConnectionTableKeys() { PaymentOrderId = entityPOCO.PaymentOrderId, ConnectedEntityId = entityPOCO.ConnectedEntityId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 