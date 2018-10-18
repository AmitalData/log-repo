 
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
   public partial class SupplierInvoiceItemsProdIdentRepository:IRepository<SupplierInvoiceItemsProdIdent>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsProdIdentRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsProdIdentRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsProdIdent GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsProdIdents
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsProdIdent> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsProdIdents  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsProdIdent GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsProdIdentKeys keys = entityKeys as SupplierInvoiceItemsProdIdentKeys;
            return (from a in context.SupplierInvoiceItemsProdIdents
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsProdIdent entity)
        {
            onAdd();
            context.SupplierInvoiceItemsProdIdents.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsProdIdent entity)
        {
            context.SupplierInvoiceItemsProdIdents.Attach(entity);
            context.SupplierInvoiceItemsProdIdents.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsProdIdent entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsProdIdents.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsProdIdent> All()
        {
            return context.SupplierInvoiceItemsProdIdents.ToList();
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
	 