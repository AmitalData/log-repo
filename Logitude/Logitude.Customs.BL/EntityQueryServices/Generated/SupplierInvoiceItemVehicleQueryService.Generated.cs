 
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
   public partial class SupplierInvoiceItemVehicleQueryService: EntityQueryService<SupplierInvoiceItemVehicle,SupplierInvoiceItemVehicleKeys,SupplierInvoiceItemVehiclePM,SupplierInvoiceItemPM,SupplierInvoiceItemKeys>
   {
   
        SupplierInvoiceItemVehicleRepository repository;
		ICustomContext  context;
        public SupplierInvoiceItemVehicleQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceItemVehicleRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceItemVehicleDataMapping();
        }

        public SupplierInvoiceItemVehicleQueryService(SupplierInvoiceItemVehicleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceItemVehicleDataMapping();
        }

        public SupplierInvoiceItemVehicleQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceItemVehicleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceItemVehicleDataMapping();
        }
		 
		public  SupplierInvoiceItemVehiclePM GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceItemVehicleKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, InvoiceItemLineNumber = invoiceitemlinenumber, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceItemVehicle entityPOCO)
        {
            SupplierInvoiceItemVehicleKeys entityKeys = new SupplierInvoiceItemVehicleKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 