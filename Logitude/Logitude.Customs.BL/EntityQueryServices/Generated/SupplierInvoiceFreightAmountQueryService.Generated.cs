 
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
   public partial class SupplierInvoiceFreightAmountQueryService: EntityQueryService<SupplierInvoiceFreightAmount,SupplierInvoiceFreightAmountKeys,SupplierInvoiceFreightAmountPM,SupplierInvoicePM,SupplierInvoiceKeys>
   {
   
        SupplierInvoiceFreightAmountRepository repository;
		ICustomContext  context;
        public SupplierInvoiceFreightAmountQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceFreightAmountRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceFreightAmountDataMapping();
        }

        public SupplierInvoiceFreightAmountQueryService(SupplierInvoiceFreightAmountRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceFreightAmountDataMapping();
        }

        public SupplierInvoiceFreightAmountQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceFreightAmountRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceFreightAmountDataMapping();
        }
		 
		public  SupplierInvoiceFreightAmountPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceFreightAmountKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceFreightAmount entityPOCO)
        {
            SupplierInvoiceFreightAmountKeys entityKeys = new SupplierInvoiceFreightAmountKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 