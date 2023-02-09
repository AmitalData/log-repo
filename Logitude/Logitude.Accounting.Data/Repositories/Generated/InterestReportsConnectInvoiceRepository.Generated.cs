 
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
   public partial class InterestReportsConnectInvoiceRepository:IRepository<InterestReportsConnectInvoice>
   {
   
        private IAccountingContext currentContext;
        public InterestReportsConnectInvoiceRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestReportsConnectInvoiceRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestReportsConnectInvoice GetSingle(string id, int tenant)
        {
            return (from a in context.InterestReportsConnectInvoices
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestReportsConnectInvoice> GetAll(int tenant)
        {
            return from a in context.InterestReportsConnectInvoices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestReportsConnectInvoice GetSingle(EntityKeyFields entityKeys)
        {
            InterestReportsConnectInvoiceKeys keys = entityKeys as InterestReportsConnectInvoiceKeys;
            return (from a in context.InterestReportsConnectInvoices
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestReportsConnectInvoice entity)
        {
            onAdd();
            context.InterestReportsConnectInvoices.Add(entity);
        }

        public void Remove(InterestReportsConnectInvoice entity)
        {
            context.InterestReportsConnectInvoices.Attach(entity);
            context.InterestReportsConnectInvoices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestReportsConnectInvoice entity)
        {
            onUpdate();
            context.InterestReportsConnectInvoices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestReportsConnectInvoice> All()
        {
            return context.InterestReportsConnectInvoices.ToList();
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
	 