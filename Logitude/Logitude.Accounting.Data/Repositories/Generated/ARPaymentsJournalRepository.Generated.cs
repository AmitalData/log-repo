 
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
   public partial class ARPaymentsJournalRepository:IRepository<ARPaymentsJournal>
   {
   
        private IAccountingContext currentContext;
        public ARPaymentsJournalRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ARPaymentsJournalRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ARPaymentsJournal GetSingle(int tenant, string paymentid, bool isvoided, int tenant)
        {
            return (from a in context.ARPaymentsJournals
                    where a.Tenant == tenant && a.PaymentId == paymentid && a.IsVoided == isvoided && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ARPaymentsJournal> GetAll(int tenant)
        {
            return from a in context.ARPaymentsJournals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ARPaymentsJournal GetSingle(EntityKeyFields entityKeys)
        {
            ARPaymentsJournalKeys keys = entityKeys as ARPaymentsJournalKeys;
            return (from a in context.ARPaymentsJournals
                    where a.Tenant == keys.Tenant && a.PaymentId == keys.PaymentId && a.IsVoided == keys.IsVoided
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ARPaymentsJournal entity)
        {
            onAdd();
            context.ARPaymentsJournals.Add(entity);
        }

        public void Remove(ARPaymentsJournal entity)
        {
            context.ARPaymentsJournals.Attach(entity);
            context.ARPaymentsJournals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ARPaymentsJournal entity)
        {
            onUpdate();
            context.ARPaymentsJournals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARPaymentsJournal> All()
        {
            return context.ARPaymentsJournals.ToList();
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
	 