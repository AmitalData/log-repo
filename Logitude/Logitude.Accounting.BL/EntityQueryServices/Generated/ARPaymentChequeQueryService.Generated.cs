 
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
   public partial class ARPaymentChequeQueryService: EntityQueryService<ARPaymentCheque,ARPaymentChequeKeys,ARPaymentChequePM,object,ARPaymentChequeKeys>
   {
   
        ARPaymentChequeRepository repository;
		IAccountingContext  context;
        public ARPaymentChequeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ARPaymentChequeRepository(context);
            Repository = repository;
            mapping = new ARPaymentChequeDataMapping();
        }

        public ARPaymentChequeQueryService(ARPaymentChequeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ARPaymentChequeDataMapping();
        }

        public ARPaymentChequeQueryService(IAccountingContext context)
        {
            this.repository = new ARPaymentChequeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ARPaymentChequeDataMapping();
        }
		 
		public  ARPaymentChequePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ARPaymentChequeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ARPaymentCheque entityPOCO)
        {
            ARPaymentChequeKeys entityKeys = new ARPaymentChequeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 