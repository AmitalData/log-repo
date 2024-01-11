 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvoiceItemVehicleAddRepository:IRepository<SupplierInvoiceItemVehicleAdd>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemVehicleAddRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemVehicleAddRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemVehicleAdd GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicleAdds
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemVehicleAdd> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemVehicleAdds  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemVehicleAdd GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemVehicleAddKeys keys = entityKeys as SupplierInvoiceItemVehicleAddKeys;
            return (from a in context.SupplierInvoiceItemVehicleAdds
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemVehicleAdd entity)
        {
            onAdd();
            context.SupplierInvoiceItemVehicleAdds.Add(entity);
        }

        public void Remove(SupplierInvoiceItemVehicleAdd entity)
        {
            context.SupplierInvoiceItemVehicleAdds.Attach(entity);
            context.SupplierInvoiceItemVehicleAdds.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemVehicleAdd entity)
        {
            onUpdate();
            context.SupplierInvoiceItemVehicleAdds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemVehicleAdd> All()
        {
            return context.SupplierInvoiceItemVehicleAdds.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 