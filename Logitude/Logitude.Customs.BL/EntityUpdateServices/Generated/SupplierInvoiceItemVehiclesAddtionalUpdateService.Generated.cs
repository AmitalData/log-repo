 
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
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{ 
   public partial class SupplierInvoiceItemVehiclesAddtionalUpdateService:EntityUpdateService<SupplierInvoiceItemVehiclesAddtional,SupplierInvoiceItemVehiclesAddtionalPM,EntityPM>
   {
   
        SupplierInvoiceItemVehiclesAddtionalRepository entityRepository;
        public SupplierInvoiceItemVehiclesAddtionalUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            Mapping = new SupplierInvoiceItemVehiclesAddtionalDataMapping();
            Repository = new SupplierInvoiceItemVehiclesAddtionalRepository(context);
        }

       
        private ICustomContext currentContext;
        public SupplierInvoiceItemVehiclesAddtionalUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemVehiclesAddtionalUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(SupplierInvoiceItemVehiclesAddtionalPM entityPM)
        {
            SupplierInvoiceItemVehiclesAddtionalKeys entityKeys = new SupplierInvoiceItemVehiclesAddtionalKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey, InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber, LineNumber = entityPM.LineNumber };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(SupplierInvoiceItemVehiclesAddtionalPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(SupplierInvoiceItemVehiclesAddtionalPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 