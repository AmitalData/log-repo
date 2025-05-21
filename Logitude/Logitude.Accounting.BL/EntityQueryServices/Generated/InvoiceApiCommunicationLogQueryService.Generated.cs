 
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
   public partial class InvoiceApiCommunicationLogQueryService: EntityQueryService<InvoiceApiCommunicationLog,InvoiceApiCommunicationLogKeys,InvoiceApiCommunicationLogPM,object,InvoiceApiCommunicationLogKeys>
   {
   
        InvoiceApiCommunicationLogRepository repository;
		IAccountingContext  context;
        public InvoiceApiCommunicationLogQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InvoiceApiCommunicationLogRepository(context);
            Repository = repository;
            mapping = new InvoiceApiCommunicationLogDataMapping();
        }

        public InvoiceApiCommunicationLogQueryService(InvoiceApiCommunicationLogRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InvoiceApiCommunicationLogDataMapping();
        }

        public InvoiceApiCommunicationLogQueryService(IAccountingContext context)
        {
            this.repository = new InvoiceApiCommunicationLogRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InvoiceApiCommunicationLogDataMapping();
        }
		 
		public  InvoiceApiCommunicationLogPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InvoiceApiCommunicationLogKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InvoiceApiCommunicationLog entityPOCO)
        {
            InvoiceApiCommunicationLogKeys entityKeys = new InvoiceApiCommunicationLogKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 