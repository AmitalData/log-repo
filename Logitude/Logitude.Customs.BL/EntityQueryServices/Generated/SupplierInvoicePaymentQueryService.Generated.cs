 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class SupplierInvoicePaymentQueryService: EntityQueryService<SupplierInvoicePayment,SupplierInvoicePaymentKeys,SupplierInvoicePaymentPM,SupplierInvoicePM,SupplierInvoiceKeys>
   {
   
        SupplierInvoicePaymentRepository repository;
		ICustomContext  context;
        public SupplierInvoicePaymentQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoicePaymentRepository(context);
            Repository = repository;
            mapping = new SupplierInvoicePaymentDataMapping();
        }

        public SupplierInvoicePaymentQueryService(SupplierInvoicePaymentRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoicePaymentDataMapping();
        }

        public SupplierInvoicePaymentQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoicePaymentRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoicePaymentDataMapping();
        }
		 
		public  SupplierInvoicePaymentPM GetSingle(string declarationid, int invoicecounterkey, int sequencenumeric,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoicePaymentKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, SequenceNumeric = sequencenumeric };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoicePayment entityPOCO)
        {
            SupplierInvoicePaymentKeys entityKeys = new SupplierInvoicePaymentKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, SequenceNumeric = entityPOCO.SequenceNumeric,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 