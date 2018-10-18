 
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
   public partial class SupplierInvoiceModificationQueryService: EntityQueryService<SupplierInvoiceModification,SupplierInvoiceModificationKeys,SupplierInvoiceModificationPM,SupplierInvoicePM,SupplierInvoiceKeys>
   {
   
        SupplierInvoiceModificationRepository repository;
		ICustomContext  context;
        public SupplierInvoiceModificationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvoiceModificationRepository(context);
            Repository = repository;
            mapping = new SupplierInvoiceModificationDataMapping();
        }

        public SupplierInvoiceModificationQueryService(SupplierInvoiceModificationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvoiceModificationDataMapping();
        }

        public SupplierInvoiceModificationQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvoiceModificationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvoiceModificationDataMapping();
        }
		 
		public  SupplierInvoiceModificationPM GetSingle(string declarationid, int invoicecounterkey, int modificationcounterkey,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvoiceModificationKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, ModificationCounterKey = modificationcounterkey };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvoiceModification entityPOCO)
        {
            SupplierInvoiceModificationKeys entityKeys = new SupplierInvoiceModificationKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, ModificationCounterKey = entityPOCO.ModificationCounterKey,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 