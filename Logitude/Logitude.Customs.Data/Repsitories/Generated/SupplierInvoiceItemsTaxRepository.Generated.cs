 
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
   public partial class SupplierInvoiceItemsTaxRepository:IRepository<SupplierInvoiceItemsTax>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsTaxRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsTaxRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsTax GetSingle(string declarationid, int invoicecounterkey, int linenumber, string taxtypecode, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsTaxes
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.LineNumber == linenumber && a.TaxTypeCode == taxtypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsTax> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsTaxes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsTax GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsTaxKeys keys = entityKeys as SupplierInvoiceItemsTaxKeys;
            return (from a in context.SupplierInvoiceItemsTaxes
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.LineNumber == keys.LineNumber && a.TaxTypeCode == keys.TaxTypeCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsTax entity)
        {
            onAdd();
            context.SupplierInvoiceItemsTaxes.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsTax entity)
        {
            context.SupplierInvoiceItemsTaxes.Attach(entity);
            context.SupplierInvoiceItemsTaxes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsTax entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsTaxes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsTax> All()
        {
            return context.SupplierInvoiceItemsTaxes.ToList();
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
	 