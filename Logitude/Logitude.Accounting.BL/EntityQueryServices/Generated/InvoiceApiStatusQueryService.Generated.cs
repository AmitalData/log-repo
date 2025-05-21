 
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
   public partial class InvoiceApiStatusQueryService: EntityQueryService<InvoiceApiStatus,InvoiceApiStatusKeys,InvoiceApiStatusPM,object,InvoiceApiStatusKeys>
   {
   
        InvoiceApiStatusRepository repository;
		IAccountingContext  context;
        public InvoiceApiStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InvoiceApiStatusRepository(context);
            Repository = repository;
            mapping = new InvoiceApiStatusDataMapping();
        }

        public InvoiceApiStatusQueryService(InvoiceApiStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InvoiceApiStatusDataMapping();
        }

        public InvoiceApiStatusQueryService(IAccountingContext context)
        {
            this.repository = new InvoiceApiStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InvoiceApiStatusDataMapping();
        }
		 
		public  InvoiceApiStatusPM GetSingle(string statuscode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InvoiceApiStatusKeys(){ StatusCode = statuscode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InvoiceApiStatus entityPOCO)
        {
            InvoiceApiStatusKeys entityKeys = new InvoiceApiStatusKeys() { StatusCode = entityPOCO.StatusCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 