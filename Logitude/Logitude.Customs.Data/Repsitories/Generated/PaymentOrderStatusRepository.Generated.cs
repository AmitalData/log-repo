 
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
   public partial class PaymentOrderStatusRepository:IRepository<PaymentOrderStatus>
   {
   
        private ICustomContext currentContext;
        public PaymentOrderStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentOrderStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentOrderStatus GetSingle(string code)
        {
            return (from a in context.PaymentOrderStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentOrderStatus> GetAll()
        {
            return from a in context.PaymentOrderStatus  
                   select a;
        }
				 
        public PaymentOrderStatus GetSingle(EntityKeyFields entityKeys)
        {
            PaymentOrderStatusKeys keys = entityKeys as PaymentOrderStatusKeys;
            return (from a in context.PaymentOrderStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentOrderStatus entity)
        {
            onAdd();
            context.PaymentOrderStatus.Add(entity);
        }

        public void Remove(PaymentOrderStatus entity)
        {
            context.PaymentOrderStatus.Attach(entity);
            context.PaymentOrderStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentOrderStatus entity)
        {
            onUpdate();
            context.PaymentOrderStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentOrderStatus> All()
        {
            return context.PaymentOrderStatus.ToList();
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
	 