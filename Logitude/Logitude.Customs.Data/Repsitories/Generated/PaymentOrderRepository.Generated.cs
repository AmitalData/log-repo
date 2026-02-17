 
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
   public partial class PaymentOrderRepository:IRepository<PaymentOrder>
   {
   
        private ICustomContext currentContext;
        public PaymentOrderRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentOrderRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentOrder GetSingle(string id, int tenant)
        {
            return (from a in context.PaymentOrders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentOrder> GetAll(int tenant)
        {
            return from a in context.PaymentOrders  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PaymentOrder GetSingle(EntityKeyFields entityKeys)
        {
            PaymentOrderKeys keys = entityKeys as PaymentOrderKeys;
            return (from a in context.PaymentOrders
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentOrder entity)
        {
            onAdd();
            context.PaymentOrders.Add(entity);
        }

        public void Remove(PaymentOrder entity)
        {
            context.PaymentOrders.Attach(entity);
            context.PaymentOrders.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentOrder entity)
        {
            onUpdate();
            context.PaymentOrders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentOrder> All()
        {
            return context.PaymentOrders.ToList();
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
	 