 
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
   public partial class SupplierInvoiceItemProcesTypeRepository:IRepository<SupplierInvoiceItemProcesType>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemProcesTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemProcesTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemProcesType GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItemProcesTypes
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemProcesType> GetAll(int tenant)
        {
            return from a in context.SupplierInvoiceItemProcesTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvoiceItemProcesType GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemProcesTypeKeys keys = entityKeys as SupplierInvoiceItemProcesTypeKeys;
            return (from a in context.SupplierInvoiceItemProcesTypes
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvoiceItemProcesType entity)
        {
            onAdd();
            context.SupplierInvoiceItemProcesTypes.Add(entity);
        }

        public void Remove(SupplierInvoiceItemProcesType entity)
        {
            context.SupplierInvoiceItemProcesTypes.Attach(entity);
            context.SupplierInvoiceItemProcesTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvoiceItemProcesType entity)
        {
            onUpdate();
            context.SupplierInvoiceItemProcesTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemProcesType> All()
        {
            return context.SupplierInvoiceItemProcesTypes.ToList();
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
	 