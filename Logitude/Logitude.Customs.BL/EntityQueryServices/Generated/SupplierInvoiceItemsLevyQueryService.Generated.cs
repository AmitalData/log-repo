 
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
   public partial class SupplierInvoiceItemsLevyQueryService: EntityQueryService<SupplierInvoiceItemsLevy,SupplierInvoiceItemsLevyKeys,SupplierInvoiceItemsLevyPM,SupplierInvoiceItemPM,SupplierInvoiceItemKeys>
   {
   
        SupplierInvoiceItemsLevyRepository repository;
		ICustomContext  context;
        public SupplierInvoiceItemsLevyQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceItemsLevyRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceItemsLevyDataMapping();
        }

        public SupplierInvoiceItemsLevyQueryService(SupplierInvoiceItemsLevyRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceItemsLevyDataMapping();
        }

        public SupplierInvoiceItemsLevyQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceItemsLevyRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceItemsLevyDataMapping();
        }
		 
		public  SupplierInvoiceItemsLevyPM GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceItemsLevyKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, InvoiceItemLineNumber = invoiceitemlinenumber, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceItemsLevy entityPOCO)
        {
            SupplierInvoiceItemsLevyKeys entityKeys = new SupplierInvoiceItemsLevyKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 