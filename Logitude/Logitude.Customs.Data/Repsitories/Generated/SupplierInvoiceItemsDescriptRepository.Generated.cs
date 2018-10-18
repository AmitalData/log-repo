 
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
   public partial class SupplierInvoiceItemsDescriptRepository:IRepository<SupplierInvoiceItemsDescript>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsDescriptRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsDescriptRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsDescript GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsDescripts
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsDescript> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsDescripts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsDescript GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsDescriptKeys keys = entityKeys as SupplierInvoiceItemsDescriptKeys;
            return (from a in context.SupplierInvoiceItemsDescripts
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsDescript entity)
        {
            onAdd();
            context.SupplierInvoiceItemsDescripts.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsDescript entity)
        {
            context.SupplierInvoiceItemsDescripts.Attach(entity);
            context.SupplierInvoiceItemsDescripts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsDescript entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsDescripts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsDescript> All()
        {
            return context.SupplierInvoiceItemsDescripts.ToList();
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
	 