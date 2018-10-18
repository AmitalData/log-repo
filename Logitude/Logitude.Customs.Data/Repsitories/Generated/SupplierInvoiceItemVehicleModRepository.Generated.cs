 
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
   public partial class SupplierInvoiceItemVehicleModRepository:IRepository<SupplierInvoiceItemVehicleMod>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemVehicleModRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemVehicleModRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemVehicleMod GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int vehiclelinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicleMods
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.VehicleLineNumber == vehiclelinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemVehicleMod> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemVehicleMods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemVehicleMod GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemVehicleModKeys keys = entityKeys as SupplierInvoiceItemVehicleModKeys;
            return (from a in context.SupplierInvoiceItemVehicleMods
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.VehicleLineNumber == keys.VehicleLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemVehicleMod entity)
        {
            onAdd();
            context.SupplierInvoiceItemVehicleMods.Add(entity);
        }

        public void Remove(SupplierInvoiceItemVehicleMod entity)
        {
            context.SupplierInvoiceItemVehicleMods.Attach(entity);
            context.SupplierInvoiceItemVehicleMods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemVehicleMod entity)
        {
            onUpdate();
            context.SupplierInvoiceItemVehicleMods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemVehicleMod> All()
        {
            return context.SupplierInvoiceItemVehicleMods.ToList();
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
	 