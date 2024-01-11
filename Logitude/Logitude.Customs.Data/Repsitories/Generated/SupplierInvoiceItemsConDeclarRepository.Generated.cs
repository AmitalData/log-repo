 
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
   public partial class SupplierInvoiceItemsConDeclarRepository:IRepository<SupplierInvoiceItemsConDeclar>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsConDeclarRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsConDeclarRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsConDeclar GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsConDeclars
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsConDeclar> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsConDeclars  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsConDeclar GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsConDeclarKeys keys = entityKeys as SupplierInvoiceItemsConDeclarKeys;
            return (from a in context.SupplierInvoiceItemsConDeclars
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsConDeclar entity)
        {
            onAdd();
            context.SupplierInvoiceItemsConDeclars.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsConDeclar entity)
        {
            context.SupplierInvoiceItemsConDeclars.Attach(entity);
            context.SupplierInvoiceItemsConDeclars.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsConDeclar entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsConDeclars.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsConDeclar> All()
        {
            return context.SupplierInvoiceItemsConDeclars.ToList();
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
	 