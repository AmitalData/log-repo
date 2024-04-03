 
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
   public partial class SupplierInvoiceModificationRepository:IRepository<SupplierInvoiceModification>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceModificationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceModificationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceModification GetSingle(string declarationid, int invoicecounterkey, int modificationcounterkey, int tenant)
        {
            return (from a in context.SupplierInvoiceModifications
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.ModificationCounterKey == modificationcounterkey && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceModification> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceModifications  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceModification GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceModificationKeys keys = entityKeys as SupplierInvoiceModificationKeys;
            return (from a in context.SupplierInvoiceModifications
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.ModificationCounterKey == keys.ModificationCounterKey
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceModification entity)
        {
            onAdd();
            context.SupplierInvoiceModifications.Add(entity);
        }

        public void Remove(SupplierInvoiceModification entity)
        {
            context.SupplierInvoiceModifications.Attach(entity);
            context.SupplierInvoiceModifications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceModification entity)
        {
            onUpdate();
            context.SupplierInvoiceModifications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceModification> All()
        {
            return context.SupplierInvoiceModifications.ToList();
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
	 