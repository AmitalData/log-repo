 
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
   public partial class PaymentChequeStatusRepository:IRepository<PaymentChequeStatus>
   {
   
        private IAccountingContext currentContext;
        public PaymentChequeStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public PaymentChequeStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentChequeStatus GetSingle(string code)
        {
            return (from a in context.PaymentChequeStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentChequeStatus> GetAll()
        {
            return from a in context.PaymentChequeStatuses  
                   select a;
        }
				 
        public PaymentChequeStatus GetSingle(EntityKeyFields entityKeys)
        {
            PaymentChequeStatusKeys keys = entityKeys as PaymentChequeStatusKeys;
            return (from a in context.PaymentChequeStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentChequeStatus entity)
        {
            onAdd();
            context.PaymentChequeStatuses.Add(entity);
        }

        public void Remove(PaymentChequeStatus entity)
        {
            context.PaymentChequeStatuses.Attach(entity);
            context.PaymentChequeStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentChequeStatus entity)
        {
            onUpdate();
            context.PaymentChequeStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentChequeStatus> All()
        {
            return context.PaymentChequeStatuses.ToList();
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
	 