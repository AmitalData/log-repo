 
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
   public partial class SupplierInvoiceItemsLevyRepository:IRepository<SupplierInvoiceItemsLevy>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsLevyRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsLevyRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsLevy GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsLevies
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsLevy> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsLevies  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsLevy GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsLevyKeys keys = entityKeys as SupplierInvoiceItemsLevyKeys;
            return (from a in context.SupplierInvoiceItemsLevies
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsLevy entity)
        {
            onAdd();
            context.SupplierInvoiceItemsLevies.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsLevy entity)
        {
            context.SupplierInvoiceItemsLevies.Attach(entity);
            context.SupplierInvoiceItemsLevies.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsLevy entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsLevies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsLevy> All()
        {
            return context.SupplierInvoiceItemsLevies.ToList();
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
	 