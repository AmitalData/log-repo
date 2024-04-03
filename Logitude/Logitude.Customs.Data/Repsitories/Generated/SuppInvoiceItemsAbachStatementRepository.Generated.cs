 
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
   public partial class SuppInvoiceItemsAbachStatementRepository:IRepository<SuppInvoiceItemsAbachStatement>
   {
   
        private ICustomContext currentContext;
        public SuppInvoiceItemsAbachStatementRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SuppInvoiceItemsAbachStatementRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SuppInvoiceItemsAbachStatement GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int? sequencenumeric, int tenant)
        {
            return (from a in context.SuppInvoiceItemsAbachStatement
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.SequenceNumeric == sequencenumeric && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SuppInvoiceItemsAbachStatement> GetAll(int tenant)
        {
            return from a in context.SuppInvoiceItemsAbachStatement  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SuppInvoiceItemsAbachStatement GetSingle(EntityKeyFields entityKeys)
        {
            SuppInvoiceItemsAbachStatementKeys keys = entityKeys as SuppInvoiceItemsAbachStatementKeys;
            return (from a in context.SuppInvoiceItemsAbachStatement
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.InvoiceItemLineNumber == keys.InvoiceItemLineNumber && a.SequenceNumeric == keys.SequenceNumeric
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SuppInvoiceItemsAbachStatement entity)
        {
            onAdd();
            context.SuppInvoiceItemsAbachStatement.Add(entity);
        }

        public void Remove(SuppInvoiceItemsAbachStatement entity)
        {
            context.SuppInvoiceItemsAbachStatement.Attach(entity);
            context.SuppInvoiceItemsAbachStatement.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SuppInvoiceItemsAbachStatement entity)
        {
            onUpdate();
            context.SuppInvoiceItemsAbachStatement.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SuppInvoiceItemsAbachStatement> All()
        {
            return context.SuppInvoiceItemsAbachStatement.ToList();
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
	 