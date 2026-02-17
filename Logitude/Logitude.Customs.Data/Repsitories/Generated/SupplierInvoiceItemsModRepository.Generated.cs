 
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
   public partial class SupplierInvoiceItemsModRepository:IRepository<SupplierInvoiceItemsMod>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemsModRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemsModRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemsMod GetSingle(string declarationid, int invoicecounterkey, int linenumber, int modificationcounterkey, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsMods
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.LineNumber == linenumber && a.ModificationCounterKey == modificationcounterkey && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemsMod> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemsMods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemsMod GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemsModKeys keys = entityKeys as SupplierInvoiceItemsModKeys;
            return (from a in context.SupplierInvoiceItemsMods
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.LineNumber == keys.LineNumber && a.ModificationCounterKey == keys.ModificationCounterKey
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemsMod entity)
        {
            onAdd();
            context.SupplierInvoiceItemsMods.Add(entity);
        }

        public void Remove(SupplierInvoiceItemsMod entity)
        {
            context.SupplierInvoiceItemsMods.Attach(entity);
            context.SupplierInvoiceItemsMods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemsMod entity)
        {
            onUpdate();
            context.SupplierInvoiceItemsMods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemsMod> All()
        {
            return context.SupplierInvoiceItemsMods.ToList();
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
	 