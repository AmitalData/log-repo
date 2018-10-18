 
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
   public partial class PaymentChequeLineRepository:IRepository<PaymentChequeLine>
   {
   
        private IAccountingContext currentContext;
        public PaymentChequeLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public PaymentChequeLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentChequeLine GetSingle(string paymentchequeid, int line, int tenant)
        {
            return (from a in context.PaymentChequeLines
                    where a.PaymentChequeId == paymentchequeid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentChequeLine> GetAll(int tenant)
        {
            return from a in context.PaymentChequeLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PaymentChequeLine GetSingle(EntityKeyFields entityKeys)
        {
            PaymentChequeLineKeys keys = entityKeys as PaymentChequeLineKeys;
            return (from a in context.PaymentChequeLines
                    where a.PaymentChequeId == keys.PaymentChequeId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentChequeLine entity)
        {
            onAdd();
            context.PaymentChequeLines.Add(entity);
        }

        public void Remove(PaymentChequeLine entity)
        {
            context.PaymentChequeLines.Attach(entity);
            context.PaymentChequeLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentChequeLine entity)
        {
            onUpdate();
            context.PaymentChequeLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentChequeLine> All()
        {
            return context.PaymentChequeLines.ToList();
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
	 