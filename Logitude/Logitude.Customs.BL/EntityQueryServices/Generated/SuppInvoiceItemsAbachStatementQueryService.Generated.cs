 
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
   public partial class SuppInvoiceItemsAbachStatementQueryService: EntityQueryService<SuppInvoiceItemsAbachStatement,SuppInvoiceItemsAbachStatementKeys,SuppInvoiceItemsAbachStatementPM,SupplierInvoiceItemPM,SupplierInvoiceItemKeys>
   {
   
        SuppInvoiceItemsAbachStatementRepository repository;
		ICustomContext  context;
        public SuppInvoiceItemsAbachStatementQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SuppInvoiceItemsAbachStatementRepository(context);
            Repository = repository;
            mapping = new SuppInvoiceItemsAbachStatementDataMapping();
        }

        public SuppInvoiceItemsAbachStatementQueryService(SuppInvoiceItemsAbachStatementRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SuppInvoiceItemsAbachStatementDataMapping();
        }

        public SuppInvoiceItemsAbachStatementQueryService(ICustomContext context)
        {
            this.repository = new SuppInvoiceItemsAbachStatementRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SuppInvoiceItemsAbachStatementDataMapping();
        }
		 
		public  SuppInvoiceItemsAbachStatementPM GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int? sequencenumeric,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SuppInvoiceItemsAbachStatementKeys(){ DeclarationId = declarationid, InvoiceCounterKey = invoicecounterkey, InvoiceItemLineNumber = invoiceitemlinenumber, SequenceNumeric = sequencenumeric };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SuppInvoiceItemsAbachStatement entityPOCO)
        {
            SuppInvoiceItemsAbachStatementKeys entityKeys = new SuppInvoiceItemsAbachStatementKeys() { DeclarationId = entityPOCO.DeclarationId, InvoiceCounterKey = entityPOCO.InvoiceCounterKey, InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber, SequenceNumeric = entityPOCO.SequenceNumeric,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 