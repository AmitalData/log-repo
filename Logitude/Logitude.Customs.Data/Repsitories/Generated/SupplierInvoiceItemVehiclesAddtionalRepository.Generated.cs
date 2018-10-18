 
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
   public partial class SupplierInvoiceItemVehiclesAddtionalRepository:IRepository<SupplierInvoiceItemVehiclesAddtional>
   {
   
        private ICustomContext currentContext;
        public SupplierInvoiceItemVehiclesAddtionalRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvoiceItemVehiclesAddtionalRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvoiceItemVehiclesAddtional GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber)
        {
            return (from a in context.SupplierInvoiceItemVehiclesAddtionals
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber 
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvoiceItemVehiclesAddtional> GetAll()
        {
            return from a in context.SupplierInvoiceItemVehiclesAddtionals  
                   select a;
        }
				 
        public SupplierInvoiceItemVehiclesAddtional GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvoiceItemVehiclesAddtionalKeys keys = entityKeys as SupplierInvoiceItemVehiclesAddtionalKeys;
            return (from a in context.SupplierInvoiceItemVehiclesAddtionals
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 
        public void Add(SupplierInvoiceItemVehiclesAddtional entity)
        {
            context.SupplierInvoiceItemVehiclesAddtionals.Add(entity);
        }

        public void Remove(SupplierInvoiceItemVehiclesAddtional entity)
        {
            context.SupplierInvoiceItemVehiclesAddtionals.Attach(entity);
            context.SupplierInvoiceItemVehiclesAddtionals.Remove(entity);
        }

        public void Update(SupplierInvoiceItemVehiclesAddtional entity)
        {
            context.SupplierInvoiceItemVehiclesAddtionals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvoiceItemVehiclesAddtional> All()
        {
            return context.SupplierInvoiceItemVehiclesAddtionals.ToList();
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
	 