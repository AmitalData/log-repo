 
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
   public partial class SupplierInvoiceItemModVehicleQueryService: EntityQueryService<SupplierInvoiceItemModVehicle,SupplierInvoiceItemModVehicleKeys,SupplierInvoiceItemModVehiclePM,SupplierInvoiceItemPM,SupplierInvoiceItemKeys>
   {
   
        SupplierInvoiceItemModVehicleRepository repository;
		ICustomContext  context;
        public SupplierInvoiceItemModVehicleQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceItemModVehicleRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceItemModVehicleDataMapping();
        }

        public SupplierInvoiceItemModVehicleQueryService(SupplierInvoiceItemModVehicleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceItemModVehicleDataMapping();
        }

        public SupplierInvoiceItemModVehicleQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceItemModVehicleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceItemModVehicleDataMapping();
        }
		 
		public  SupplierInvoiceItemModVehiclePM GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, string adjustmenttypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceItemModVehicleKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, InvoiceItemLineNumber = invoiceitemlinenumber, AdjustmentTypeCode = adjustmenttypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceItemModVehicle entityPOCO)
        {
            SupplierInvoiceItemModVehicleKeys entityKeys = new SupplierInvoiceItemModVehicleKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber, AdjustmentTypeCode = entityPOCO.AdjustmentTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 