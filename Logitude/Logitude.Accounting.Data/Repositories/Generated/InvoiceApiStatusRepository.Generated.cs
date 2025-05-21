 
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
   public partial class InvoiceApiStatusRepository:IRepository<InvoiceApiStatus>
   {
   
        private IAccountingContext currentContext;
        public InvoiceApiStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InvoiceApiStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InvoiceApiStatus GetSingle(string statuscode)
        {
            return (from a in context.InvoiceApiStatuses
                    where a.StatusCode == statuscode 
                    select a).FirstOrDefault();
        }

        public IQueryable<InvoiceApiStatus> GetAll()
        {
            return from a in context.InvoiceApiStatuses  
                   select a;
        }
				 
        public InvoiceApiStatus GetSingle(EntityKeyFields entityKeys)
        {
            InvoiceApiStatusKeys keys = entityKeys as InvoiceApiStatusKeys;
            return (from a in context.InvoiceApiStatuses
                    where a.StatusCode == keys.StatusCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InvoiceApiStatus entity)
        {
            onAdd();
            context.InvoiceApiStatuses.Add(entity);
        }

        public void Remove(InvoiceApiStatus entity)
        {
            context.InvoiceApiStatuses.Attach(entity);
            context.InvoiceApiStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InvoiceApiStatus entity)
        {
            onUpdate();
            context.InvoiceApiStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InvoiceApiStatus> All()
        {
            return context.InvoiceApiStatuses.ToList();
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
	 