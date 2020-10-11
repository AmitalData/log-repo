 
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
   public partial class SupplierInvoiceItemsPriceRepository:IRepository<SupplierInvoiceItemsPrice>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsPriceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsPriceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsPrice GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsPrices
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsPrice> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsPrices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsPrice GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsPriceKeys keys = entityKeys as SupplierInvoiceItemsPriceKeys;
            return (from a in context.SupplierInvoiceItemsPrices
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsPrice entity)
        {
            onAdd();
            context.SupplierInvoiceItemsPrices.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsPrice entity)
        {
            context.SupplierInvoiceItemsPrices.Attach(entity);
            context.SupplierInvoiceItemsPrices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsPrice entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsPrices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsPrice> All()
        {
            return context.SupplierInvoiceItemsPrices.ToList();
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
	 