 
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
   public partial class SupplierInvoiceItemsReqListRepository:IRepository<SupplierInvoiceItemsReqList>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsReqListRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsReqListRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsReqList GetSingle(string declarationid, int linenumber, int invoicecounterkey, int invoiceitemlinenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsReqLists
                    where a.DeclarationId == declarationid && a.LineNumber == linenumber && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsReqList> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsReqLists  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsReqList GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsReqListKeys keys = entityKeys as SupplierInvoiceItemsReqListKeys;
            return (from a in context.SupplierInvoiceItemsReqLists
                    where a.DeclarationId == keys.DeclarationId && a.LineNumber == keys.LineNumber && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsReqList entity)
        {
            onAdd();
            context.SupplierInvoiceItemsReqLists.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsReqList entity)
        {
            context.SupplierInvoiceItemsReqLists.Attach(entity);
            context.SupplierInvoiceItemsReqLists.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsReqList entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsReqLists.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsReqList> All()
        {
            return context.SupplierInvoiceItemsReqLists.ToList();
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
	 