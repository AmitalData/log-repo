 
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
   public partial class PaymentOrderProtestReasonRepository:IRepository<PaymentOrderProtestReason>
   {
   
        private ICustomContext currentContext;
        public PaymentOrderProtestReasonRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentOrderProtestReasonRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentOrderProtestReason GetSingle(string paymentorderid, int line, int tenant)
        {
            return (from a in context.PaymentOrderProtestReasons
                    where a.PaymentOrderId == paymentorderid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentOrderProtestReason> GetAll(int tenant)
        {
            return from a in context.PaymentOrderProtestReasons  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PaymentOrderProtestReason GetSingle(EntityKeyFields entityKeys)
        {
            PaymentOrderProtestReasonKeys keys = entityKeys as PaymentOrderProtestReasonKeys;
            return (from a in context.PaymentOrderProtestReasons
                    where a.PaymentOrderId == keys.PaymentOrderId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentOrderProtestReason entity)
        {
            onAdd();
            context.PaymentOrderProtestReasons.Add(entity);
        }

        public void Remove(PaymentOrderProtestReason entity)
        {
            context.PaymentOrderProtestReasons.Attach(entity);
            context.PaymentOrderProtestReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentOrderProtestReason entity)
        {
            onUpdate();
            context.PaymentOrderProtestReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentOrderProtestReason> All()
        {
            return context.PaymentOrderProtestReasons.ToList();
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
	 