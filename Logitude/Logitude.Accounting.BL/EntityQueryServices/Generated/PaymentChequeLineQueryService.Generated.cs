 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class PaymentChequeLineQueryService: EntityQueryService<PaymentChequeLine,PaymentChequeLineKeys,PaymentChequeLinePM,PaymentChequePM,PaymentChequeKeys>
   {
   
        PaymentChequeLineRepository repository;
		IAccountingContext  context;
        public PaymentChequeLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentChequeLineRepository(context);
            Repository = repository;
            mapping = new PaymentChequeLineDataMapping();
        }

        public PaymentChequeLineQueryService(PaymentChequeLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentChequeLineDataMapping();
        }

        public PaymentChequeLineQueryService(IAccountingContext context)
        {
            this.repository = new PaymentChequeLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentChequeLineDataMapping();
        }
		 
		public  PaymentChequeLinePM GetSingle(string paymentchequeid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentChequeLineKeys(){ PaymentChequeId = paymentchequeid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentChequeLine entityPOCO)
        {
            PaymentChequeLineKeys entityKeys = new PaymentChequeLineKeys() { PaymentChequeId = entityPOCO.PaymentChequeId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 