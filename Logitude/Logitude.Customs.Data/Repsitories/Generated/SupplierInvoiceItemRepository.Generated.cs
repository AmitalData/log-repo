 
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
   public partial class SupplierInvoiceItemRepository:IRepository<SupplierInvoiceItem>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItem GetSingle(string declarationid, int counterkey, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationid && a.CounterKey == counterkey && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItem> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItems  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItem GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemKeys keys = entityKeys as SupplierInvoiceItemKeys;
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == keys.DeclarationId && a.CounterKey == keys.CounterKey && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItem entity)
        {
            onAdd();
            context.SupplierInvoiceItems.Add(entity);
        }

        public void Remove(SupplierInvoiceItem entity)
        {
            context.SupplierInvoiceItems.Attach(entity);
            context.SupplierInvoiceItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItem entity)
        {
            onUpdate();
            context.SupplierInvoiceItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItem> All()
        {
            return context.SupplierInvoiceItems.ToList();
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
	 