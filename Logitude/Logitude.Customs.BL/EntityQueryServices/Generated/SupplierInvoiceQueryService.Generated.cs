 
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
   public partial class SupplierInvoiceQueryService: EntityQueryService<SupplierInvoice,SupplierInvoiceKeys,SupplierInvoicePM,object,SupplierInvoiceKeys>
   {
   
        SupplierInvoiceRepository repository;
		ICustomContext  context;
        public SupplierInvoiceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceDataMapping();
        }

        public SupplierInvoiceQueryService(SupplierInvoiceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceDataMapping();
        }

        public SupplierInvoiceQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceDataMapping();
        }
		 
		public  SupplierInvoicePM GetSingle(string declarationid, int invoicecounterkey,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoice entityPOCO)
        {
            SupplierInvoiceKeys entityKeys = new SupplierInvoiceKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 