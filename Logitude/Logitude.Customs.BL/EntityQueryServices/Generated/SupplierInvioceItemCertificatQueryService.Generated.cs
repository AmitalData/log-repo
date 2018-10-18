 
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
   public partial class SupplierInvioceItemCertificatQueryService: EntityQueryService<SupplierInvioceItemCertificat,SupplierInvioceItemCertificatKeys,SupplierInvioceItemCertificatPM,SupplierInvoiceItemPM,SupplierInvoiceItemKeys>
   {
   
        SupplierInvioceItemCertificatRepository repository;
		ICustomContext  context;
        public SupplierInvioceItemCertificatQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvioceItemCertificatRepository(context);
            Repository = repository;
            mapping = new SupplierInvioceItemCertificatDataMapping();
        }

        public SupplierInvioceItemCertificatQueryService(SupplierInvioceItemCertificatRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvioceItemCertificatDataMapping();
        }

        public SupplierInvioceItemCertificatQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvioceItemCertificatRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvioceItemCertificatDataMapping();
        }
		 
		public  SupplierInvioceItemCertificatPM GetSingle(string declarationid, int invoicecounterkey, int linenumber, int itemcertificatecounterkey,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvioceItemCertificatKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, LineNumber = linenumber, ItemCertificateCounterKey = itemcertificatecounterkey };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvioceItemCertificat entityPOCO)
        {
            SupplierInvioceItemCertificatKeys entityKeys = new SupplierInvioceItemCertificatKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, LineNumber = entityPOCO.LineNumber, ItemCertificateCounterKey = entityPOCO.ItemCertificateCounterKey,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 