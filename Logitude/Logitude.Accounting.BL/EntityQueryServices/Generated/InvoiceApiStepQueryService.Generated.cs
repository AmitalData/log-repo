 
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
   public partial class InvoiceApiStepQueryService: EntityQueryService<InvoiceApiStep,InvoiceApiStepKeys,InvoiceApiStepPM,object,InvoiceApiStepKeys>
   {
   
        InvoiceApiStepRepository repository;
		IAccountingContext  context;
        public InvoiceApiStepQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InvoiceApiStepRepository(context);
            Repository = repository;
            mapping = new InvoiceApiStepDataMapping();
        }

        public InvoiceApiStepQueryService(InvoiceApiStepRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InvoiceApiStepDataMapping();
        }

        public InvoiceApiStepQueryService(IAccountingContext context)
        {
            this.repository = new InvoiceApiStepRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InvoiceApiStepDataMapping();
        }
		 
		public  InvoiceApiStepPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InvoiceApiStepKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InvoiceApiStep entityPOCO)
        {
            InvoiceApiStepKeys entityKeys = new InvoiceApiStepKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 