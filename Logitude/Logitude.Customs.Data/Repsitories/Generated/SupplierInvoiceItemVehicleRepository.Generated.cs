 
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
   public partial class SupplierInvoiceItemVehicleRepository:IRepository<SupplierInvoiceItemVehicle>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemVehicleRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemVehicleRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemVehicle GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicles
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemVehicle> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemVehicles  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemVehicle GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemVehicleKeys keys = entityKeys as SupplierInvoiceItemVehicleKeys;
            return (from a in context.SupplierInvoiceItemVehicles
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemVehicle entity)
        {
            onAdd();
            context.SupplierInvoiceItemVehicles.Add(entity);
        }

        public void Remove(SupplierInvoiceItemVehicle entity)
        {
            context.SupplierInvoiceItemVehicles.Attach(entity);
            context.SupplierInvoiceItemVehicles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemVehicle entity)
        {
            onUpdate();
            context.SupplierInvoiceItemVehicles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemVehicle> All()
        {
            return context.SupplierInvoiceItemVehicles.ToList();
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
	 