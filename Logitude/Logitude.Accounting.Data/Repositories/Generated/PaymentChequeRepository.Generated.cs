 
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
   public partial class PaymentChequeRepository:IRepository<PaymentCheque>
   {
   
        private IAccountingContext currentContext;
        public PaymentChequeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public PaymentChequeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentCheque GetSingle(string id, int tenant)
        {
            return (from a in context.PaymentCheques
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentCheque> GetAll(int tenant)
        {
            return from a in context.PaymentCheques  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PaymentCheque GetSingle(EntityKeyFields entityKeys)
        {
            PaymentChequeKeys keys = entityKeys as PaymentChequeKeys;
            return (from a in context.PaymentCheques
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentCheque entity)
        {
            onAdd();
            context.PaymentCheques.Add(entity);
        }

        public void Remove(PaymentCheque entity)
        {
            context.PaymentCheques.Attach(entity);
            context.PaymentCheques.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentCheque entity)
        {
            onUpdate();
            context.PaymentCheques.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentCheque> All()
        {
            return context.PaymentCheques.ToList();
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
	 