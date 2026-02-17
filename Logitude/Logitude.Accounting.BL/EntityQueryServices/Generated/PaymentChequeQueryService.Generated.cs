 
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
   public partial class PaymentChequeQueryService: EntityQueryService<PaymentCheque,PaymentChequeKeys,PaymentChequePM,object,PaymentChequeKeys>
   {
   
        PaymentChequeRepository repository;
		IAccountingContext  context;
        public PaymentChequeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentChequeRepository(context);
            Repository = repository;
            mapping = new PaymentChequeDataMapping();
        }

        public PaymentChequeQueryService(PaymentChequeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentChequeDataMapping();
        }

        public PaymentChequeQueryService(IAccountingContext context)
        {
            this.repository = new PaymentChequeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentChequeDataMapping();
        }
		 
		public  PaymentChequePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentChequeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentCheque entityPOCO)
        {
            PaymentChequeKeys entityKeys = new PaymentChequeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 