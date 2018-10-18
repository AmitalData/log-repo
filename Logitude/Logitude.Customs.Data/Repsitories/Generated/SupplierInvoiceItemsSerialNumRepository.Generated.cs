 
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
   public partial class SupplierInvoiceItemsSerialNumRepository:IRepository<SupplierInvoiceItemsSerialNum>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsSerialNumRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsSerialNumRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsSerialNum GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsSerialNums
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsSerialNum> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsSerialNums  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsSerialNum GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsSerialNumKeys keys = entityKeys as SupplierInvoiceItemsSerialNumKeys;
            return (from a in context.SupplierInvoiceItemsSerialNums
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsSerialNum entity)
        {
            onAdd();
            context.SupplierInvoiceItemsSerialNums.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsSerialNum entity)
        {
            context.SupplierInvoiceItemsSerialNums.Attach(entity);
            context.SupplierInvoiceItemsSerialNums.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsSerialNum entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsSerialNums.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsSerialNum> All()
        {
            return context.SupplierInvoiceItemsSerialNums.ToList();
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
	 