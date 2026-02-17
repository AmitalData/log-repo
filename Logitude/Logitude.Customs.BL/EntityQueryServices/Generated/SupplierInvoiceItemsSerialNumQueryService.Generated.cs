 
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
   public partial class SupplierInvoiceItemsSerialNumQueryService: EntityQueryService<SupplierInvoiceItemsSerialNum,SupplierInvoiceItemsSerialNumKeys,SupplierInvoiceItemsSerialNumPM,SupplierInvoiceItemPM,SupplierInvoiceItemKeys>
   {
   
        SupplierInvoiceItemsSerialNumRepository repository;
		ICustomContext  context;
        public SupplierInvoiceItemsSerialNumQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceItemsSerialNumRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceItemsSerialNumDataMapping();
        }

        public SupplierInvoiceItemsSerialNumQueryService(SupplierInvoiceItemsSerialNumRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceItemsSerialNumDataMapping();
        }

        public SupplierInvoiceItemsSerialNumQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceItemsSerialNumRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceItemsSerialNumDataMapping();
        }
		 
		public  SupplierInvoiceItemsSerialNumPM GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceItemsSerialNumKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, InvoiceItemLineNumber = invoiceitemlinenumber, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceItemsSerialNum entityPOCO)
        {
            SupplierInvoiceItemsSerialNumKeys entityKeys = new SupplierInvoiceItemsSerialNumKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 