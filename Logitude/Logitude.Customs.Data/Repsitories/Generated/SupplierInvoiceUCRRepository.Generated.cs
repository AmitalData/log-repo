 
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
   public partial class SupplierInvoiceUCRRepository:IRepository<SupplierInvoiceUCR>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceUCRRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceUCRRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceUCR GetSingle(string declarationid, int invoicecounterkey, int sequencenumeric, int tenant)
        {
            return (from a in context.SupplierInvoiceUCRs
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.SequenceNumeric == sequencenumeric && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceUCR> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceUCRs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceUCR GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceUCRKeys keys = entityKeys as SupplierInvoiceUCRKeys;
            return (from a in context.SupplierInvoiceUCRs
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.SequenceNumeric == keys.SequenceNumeric
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceUCR entity)
        {
            onAdd();
            context.SupplierInvoiceUCRs.Add(entity);
        }

        public void Remove(SupplierInvoiceUCR entity)
        {
            context.SupplierInvoiceUCRs.Attach(entity);
            context.SupplierInvoiceUCRs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceUCR entity)
        {
            onUpdate();
            context.SupplierInvoiceUCRs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceUCR> All()
        {
            return context.SupplierInvoiceUCRs.ToList();
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
	 