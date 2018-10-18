 
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
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class SupplierInvoiceItemVehiclesAddtionalQueryService: EntityQueryService<SupplierInvoiceItemVehiclesAddtional,SupplierInvoiceItemVehiclesAddtionalKeys,SupplierInvoiceItemVehiclesAddtionalPM,object,SupplierInvoiceItemVehiclesAddtionalKeys>
   {
   
        SupplierInvoiceItemVehiclesAddtionalRepository repository;
		ICustomContext  context;
        public SupplierInvoiceItemVehiclesAddtionalQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceItemVehiclesAddtionalRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceItemVehiclesAddtionalDataMapping();
        }

        public SupplierInvoiceItemVehiclesAddtionalQueryService(SupplierInvoiceItemVehiclesAddtionalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceItemVehiclesAddtionalDataMapping();
        }

        public SupplierInvoiceItemVehiclesAddtionalQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceItemVehiclesAddtionalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceItemVehiclesAddtionalDataMapping();
        }
		 
		public  SupplierInvoiceItemVehiclesAddtionalPM GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceItemVehiclesAddtionalKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, InvoiceItemLineNumber = invoiceitemlinenumber, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceItemVehiclesAddtional entityPOCO)
        {
            SupplierInvoiceItemVehiclesAddtionalKeys entityKeys = new SupplierInvoiceItemVehiclesAddtionalKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 