 
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
   public partial class SupplierInvoiceItemModVehicleRepository:IRepository<SupplierInvoiceItemModVehicle>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemModVehicleRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemModVehicleRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemModVehicle GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, string adjustmenttypecode, int tenant)
        {
            return (from a in context.SupplierInvoiceItemModVehicles
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.AdjustmentTypeCode == adjustmenttypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemModVehicle> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemModVehicles  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemModVehicle GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemModVehicleKeys keys = entityKeys as SupplierInvoiceItemModVehicleKeys;
            return (from a in context.SupplierInvoiceItemModVehicles
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.AdjustmentTypeCode == keys.AdjustmentTypeCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemModVehicle entity)
        {
            onAdd();
            context.SupplierInvoiceItemModVehicles.Add(entity);
        }

        public void Remove(SupplierInvoiceItemModVehicle entity)
        {
            context.SupplierInvoiceItemModVehicles.Attach(entity);
            context.SupplierInvoiceItemModVehicles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemModVehicle entity)
        {
            onUpdate();
            context.SupplierInvoiceItemModVehicles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemModVehicle> All()
        {
            return context.SupplierInvoiceItemModVehicles.ToList();
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
	 