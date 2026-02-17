 
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
   public partial class SupplierInvoiceItemsTaxQueryService: EntityQueryService<SupplierInvoiceItemsTax,SupplierInvoiceItemsTaxKeys,SupplierInvoiceItemsTaxPM,SupplierInvoiceItemPM,SupplierInvoiceItemKeys>
   {
   
        SupplierInvoiceItemsTaxRepository repository;
		ICustomContext  context;
        public SupplierInvoiceItemsTaxQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceItemsTaxRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceItemsTaxDataMapping();
        }

        public SupplierInvoiceItemsTaxQueryService(SupplierInvoiceItemsTaxRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceItemsTaxDataMapping();
        }

        public SupplierInvoiceItemsTaxQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceItemsTaxRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceItemsTaxDataMapping();
        }
		 
		public  SupplierInvoiceItemsTaxPM GetSingle(string declarationid, int invoicecounterkey, int linenumber, string taxtypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceItemsTaxKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, LineNumber = linenumber, TaxTypeCode = taxtypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceItemsTax entityPOCO)
        {
            SupplierInvoiceItemsTaxKeys entityKeys = new SupplierInvoiceItemsTaxKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, LineNumber = entityPOCO.LineNumber, TaxTypeCode = entityPOCO.TaxTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 