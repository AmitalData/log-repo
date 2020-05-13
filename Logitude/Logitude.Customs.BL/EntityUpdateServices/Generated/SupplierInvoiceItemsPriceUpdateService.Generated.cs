 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{ 
   public partial class SupplierInvoiceItemsPriceUpdateService:EntityUpdateService<SupplierInvoiceItemsPrice,SupplierInvoiceItemsPricePM,SupplierInvoiceItemPM>
   {
   
        SupplierInvoiceItemsPriceRepository entityRepository;
        public SupplierInvoiceItemsPriceUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new SupplierInvoiceItemsPriceDataMapping();
            Repository = new SupplierInvoiceItemsPriceRepository(context);
        }

       
        private ICustomContext currentContext;
        public SupplierInvoiceItemsPriceUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsPriceUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(SupplierInvoiceItemsPricePM entityPM)
        {
            SupplierInvoiceItemsPriceKeys entityKeys = new SupplierInvoiceItemsPriceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey, InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber, LineNumber = entityPM.LineNumber };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(SupplierInvoiceItemsPricePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(SupplierInvoiceItemsPricePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 