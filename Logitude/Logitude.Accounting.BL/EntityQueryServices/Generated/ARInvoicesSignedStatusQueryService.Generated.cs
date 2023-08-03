 
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
   public partial class ARInvoicesSignedStatusQueryService: EntityQueryService<ARInvoicesSignedStatus,ARInvoicesSignedStatusKeys,ARInvoicesSignedStatusPM,object,ARInvoicesSignedStatusKeys>
   {
   
        ARInvoicesSignedStatusRepository repository;
		IAccountingContext  context;
        public ARInvoicesSignedStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ARInvoicesSignedStatusRepository(context);
            Repository = repository;
            mapping = new ARInvoicesSignedStatusDataMapping();
        }

        public ARInvoicesSignedStatusQueryService(ARInvoicesSignedStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ARInvoicesSignedStatusDataMapping();
        }

        public ARInvoicesSignedStatusQueryService(IAccountingContext context)
        {
            this.repository = new ARInvoicesSignedStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ARInvoicesSignedStatusDataMapping();
        }
		 
		public  ARInvoicesSignedStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ARInvoicesSignedStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ARInvoicesSignedStatus entityPOCO)
        {
            ARInvoicesSignedStatusKeys entityKeys = new ARInvoicesSignedStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 