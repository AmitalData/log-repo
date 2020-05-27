 
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
   public partial class InterestTransactionRepository:IRepository<InterestTransaction>
   {
   
        private IAccountingContext currentContext;
        public InterestTransactionRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestTransactionRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestTransaction GetSingle(string id, int tenant)
        {
            return (from a in context.InterestTransactions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestTransaction> GetAll(int tenant)
        {
            return from a in context.InterestTransactions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestTransaction GetSingle(EntityKeyFields entityKeys)
        {
            InterestTransactionKeys keys = entityKeys as InterestTransactionKeys;
            return (from a in context.InterestTransactions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestTransaction entity)
        {
            onAdd();
            context.InterestTransactions.Add(entity);
        }

        public void Remove(InterestTransaction entity)
        {
            context.InterestTransactions.Attach(entity);
            context.InterestTransactions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestTransaction entity)
        {
            onUpdate();
            context.InterestTransactions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestTransaction> All()
        {
            return context.InterestTransactions.ToList();
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
	 