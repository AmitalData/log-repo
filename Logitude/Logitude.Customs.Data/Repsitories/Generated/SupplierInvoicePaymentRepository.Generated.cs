 
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
   public partial class SupplierInvoicePaymentRepository:IRepository<SupplierInvoicePayment>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoicePaymentRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoicePaymentRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoicePayment GetSingle(string declarationid, int invoicecounterkey, int sequencenumeric, int tenant)
        {
            return (from a in context.SupplierInvoicePayments
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.SequenceNumeric == sequencenumeric && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoicePayment> GetAll(int tenant)
        {
            return from a in context.SupplierInvoicePayments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoicePayment GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoicePaymentKeys keys = entityKeys as SupplierInvoicePaymentKeys;
            return (from a in context.SupplierInvoicePayments
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.SequenceNumeric == keys.SequenceNumeric
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoicePayment entity)
        {
            onAdd();
            context.SupplierInvoicePayments.Add(entity);
        }

        public void Remove(SupplierInvoicePayment entity)
        {
            context.SupplierInvoicePayments.Attach(entity);
            context.SupplierInvoicePayments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoicePayment entity)
        {
            onUpdate();
            context.SupplierInvoicePayments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoicePayment> All()
        {
            return context.SupplierInvoicePayments.ToList();
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
	 