 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class InvoiceApiStepRepository:IRepository<InvoiceApiStep>
   {
   
        private IAccountingContext currentContext;
        public InvoiceApiStepRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InvoiceApiStepRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InvoiceApiStep GetSingle(string code)
        {
            return (from a in context.InvoiceApiSteps
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InvoiceApiStep> GetAll()
        {
            return from a in context.InvoiceApiSteps  
                   select a;
        }
				 
        public InvoiceApiStep GetSingle(EntityKeyFields entityKeys)
        {
            InvoiceApiStepKeys keys = entityKeys as InvoiceApiStepKeys;
            return (from a in context.InvoiceApiSteps
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InvoiceApiStep entity)
        {
            onAdd();
            context.InvoiceApiSteps.Add(entity);
        }

        public void Remove(InvoiceApiStep entity)
        {
            context.InvoiceApiSteps.Attach(entity);
            context.InvoiceApiSteps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InvoiceApiStep entity)
        {
            onUpdate();
            context.InvoiceApiSteps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InvoiceApiStep> All()
        {
            return context.InvoiceApiSteps.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 