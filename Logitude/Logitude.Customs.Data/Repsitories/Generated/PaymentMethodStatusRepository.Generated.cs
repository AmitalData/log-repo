 
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
   public partial class PaymentMethodStatusRepository:IRepository<PaymentMethodStatus>
   {
   
        private ICustomContext currentContext;
        public PaymentMethodStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentMethodStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentMethodStatus GetSingle(string code)
        {
            return (from a in context.PaymentMethodStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentMethodStatus> GetAll()
        {
            return from a in context.PaymentMethodStatus  
                   select a;
        }
				 
        public PaymentMethodStatus GetSingle(EntityKeyFields entityKeys)
        {
            PaymentMethodStatusKeys keys = entityKeys as PaymentMethodStatusKeys;
            return (from a in context.PaymentMethodStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentMethodStatus entity)
        {
            onAdd();
            context.PaymentMethodStatus.Add(entity);
        }

        public void Remove(PaymentMethodStatus entity)
        {
            context.PaymentMethodStatus.Attach(entity);
            context.PaymentMethodStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentMethodStatus entity)
        {
            onUpdate();
            context.PaymentMethodStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentMethodStatus> All()
        {
            return context.PaymentMethodStatus.ToList();
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
	 