 
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
   public partial class ARPaymentsJournalQueryService: EntityQueryService<ARPaymentsJournal,ARPaymentsJournalKeys,ARPaymentsJournalPM,object,ARPaymentsJournalKeys>
   {
   
        ARPaymentsJournalRepository repository;
		IAccountingContext  context;
        public ARPaymentsJournalQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ARPaymentsJournalRepository(context);
            Repository = repository;
            mapping = new ARPaymentsJournalDataMapping();
        }

        public ARPaymentsJournalQueryService(ARPaymentsJournalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ARPaymentsJournalDataMapping();
        }

        public ARPaymentsJournalQueryService(IAccountingContext context)
        {
            this.repository = new ARPaymentsJournalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ARPaymentsJournalDataMapping();
        }
		 
		public  ARPaymentsJournalPM GetSingle(int tenant, string paymentid, bool isvoided,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ARPaymentsJournalKeys(){ Tenant = tenant, PaymentId = paymentid, IsVoided = isvoided };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ARPaymentsJournal entityPOCO)
        {
            ARPaymentsJournalKeys entityKeys = new ARPaymentsJournalKeys() { Tenant = entityPOCO.Tenant, PaymentId = entityPOCO.PaymentId, IsVoided = entityPOCO.IsVoided,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 