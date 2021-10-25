 
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
   public partial class ARPaymentBankTranferQueryService: EntityQueryService<ARPaymentBankTranfer,ARPaymentBankTranferKeys,ARPaymentBankTranferPM,object,ARPaymentBankTranferKeys>
   {
   
        ARPaymentBankTranferRepository repository;
		IAccountingContext  context;
        public ARPaymentBankTranferQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ARPaymentBankTranferRepository(context);
            Repository = repository;
            mapping = new ARPaymentBankTranferDataMapping();
        }

        public ARPaymentBankTranferQueryService(ARPaymentBankTranferRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ARPaymentBankTranferDataMapping();
        }

        public ARPaymentBankTranferQueryService(IAccountingContext context)
        {
            this.repository = new ARPaymentBankTranferRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ARPaymentBankTranferDataMapping();
        }
		 
		public  ARPaymentBankTranferPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ARPaymentBankTranferKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ARPaymentBankTranfer entityPOCO)
        {
            ARPaymentBankTranferKeys entityKeys = new ARPaymentBankTranferKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 