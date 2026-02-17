 
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
   public partial class SupplierInvoiceRepository:IRepository<SupplierInvoice>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoice GetSingle(string declarationid, int invoicecounterkey, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoice> GetAll(int tenant)
        {
            return from a in context.SupplierInvoices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoice GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceKeys keys = entityKeys as SupplierInvoiceKeys;
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoice entity)
        {
            onAdd();
            context.SupplierInvoices.Add(entity);
        }

        public void Remove(SupplierInvoice entity)
        {
            context.SupplierInvoices.Attach(entity);
            context.SupplierInvoices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoice entity)
        {
            onUpdate();
            context.SupplierInvoices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoice> All()
        {
            return context.SupplierInvoices.ToList();
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
	 