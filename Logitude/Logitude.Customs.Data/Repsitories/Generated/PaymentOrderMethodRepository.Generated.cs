 
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
   public partial class PaymentOrderMethodRepository:IRepository<PaymentOrderMethod>
   {
   
        private ICustomContext currentContext;
        public PaymentOrderMethodRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentOrderMethodRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentOrderMethod GetSingle(string paymentorderid, int line, int tenant)
        {
            return (from a in context.PaymentOrderMethods
                    where a.PaymentOrderId == paymentorderid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentOrderMethod> GetAll(int tenant)
        {
            return from a in context.PaymentOrderMethods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PaymentOrderMethod GetSingle(EntityKeyFields entityKeys)
        {
            PaymentOrderMethodKeys keys = entityKeys as PaymentOrderMethodKeys;
            return (from a in context.PaymentOrderMethods
                    where a.PaymentOrderId == keys.PaymentOrderId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentOrderMethod entity)
        {
            onAdd();
            context.PaymentOrderMethods.Add(entity);
        }

        public void Remove(PaymentOrderMethod entity)
        {
            context.PaymentOrderMethods.Attach(entity);
            context.PaymentOrderMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentOrderMethod entity)
        {
            onUpdate();
            context.PaymentOrderMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentOrderMethod> All()
        {
            return context.PaymentOrderMethods.ToList();
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
	 