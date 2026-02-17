 
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
   public partial class ARPaymentChequeStatusQueryService: EntityQueryService<ARPaymentChequeStatus,ARPaymentChequeStatusKeys,ARPaymentChequeStatusPM,object,ARPaymentChequeStatusKeys>
   {
   
        ARPaymentChequeStatusRepository repository;
		IAccountingContext  context;
        public ARPaymentChequeStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ARPaymentChequeStatusRepository(context);
            Repository = repository;
            mapping = new ARPaymentChequeStatusDataMapping();
        }

        public ARPaymentChequeStatusQueryService(ARPaymentChequeStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ARPaymentChequeStatusDataMapping();
        }

        public ARPaymentChequeStatusQueryService(IAccountingContext context)
        {
            this.repository = new ARPaymentChequeStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ARPaymentChequeStatusDataMapping();
        }
		 
		public  ARPaymentChequeStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ARPaymentChequeStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ARPaymentChequeStatus entityPOCO)
        {
            ARPaymentChequeStatusKeys entityKeys = new ARPaymentChequeStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 