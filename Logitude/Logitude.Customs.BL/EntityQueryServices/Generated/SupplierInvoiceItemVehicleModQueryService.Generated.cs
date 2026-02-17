 
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
   public partial class SupplierInvoiceItemVehicleModQueryService: EntityQueryService<SupplierInvoiceItemVehicleMod,SupplierInvoiceItemVehicleModKeys,SupplierInvoiceItemVehicleModPM,SupplierInvoiceItemVehiclePM,SupplierInvoiceItemVehicleKeys>
   {
   
        SupplierInvoiceItemVehicleModRepository repository;
		ICustomContext  context;
        public SupplierInvoiceItemVehicleModQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceItemVehicleModRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceItemVehicleModDataMapping();
        }

        public SupplierInvoiceItemVehicleModQueryService(SupplierInvoiceItemVehicleModRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceItemVehicleModDataMapping();
        }

        public SupplierInvoiceItemVehicleModQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceItemVehicleModRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceItemVehicleModDataMapping();
        }
		 
		public  SupplierInvoiceItemVehicleModPM GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int vehiclelinenumber, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceItemVehicleModKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, InvoiceItemLineNumber = invoiceitemlinenumber, VehicleLineNumber = vehiclelinenumber, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceItemVehicleMod entityPOCO)
        {
            SupplierInvoiceItemVehicleModKeys entityKeys = new SupplierInvoiceItemVehicleModKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber, VehicleLineNumber = entityPOCO.VehicleLineNumber, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 