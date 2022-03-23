 
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
   public partial class SupplierInvoiceFreightAmountRepository:IRepository<SupplierInvoiceFreightAmount>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceFreightAmountRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceFreightAmountRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceFreightAmount GetSingle(string id, int tenant)
        {
            return (from a in context.SupplierInvoiceFreightAmounts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceFreightAmount> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceFreightAmounts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceFreightAmount GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceFreightAmountKeys keys = entityKeys as SupplierInvoiceFreightAmountKeys;
            return (from a in context.SupplierInvoiceFreightAmounts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceFreightAmount entity)
        {
            onAdd();
            context.SupplierInvoiceFreightAmounts.Add(entity);
        }

        public void Remove(SupplierInvoiceFreightAmount entity)
        {
            context.SupplierInvoiceFreightAmounts.Attach(entity);
            context.SupplierInvoiceFreightAmounts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceFreightAmount entity)
        {
            onUpdate();
            context.SupplierInvoiceFreightAmounts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceFreightAmount> All()
        {
            return context.SupplierInvoiceFreightAmounts.ToList();
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
	 