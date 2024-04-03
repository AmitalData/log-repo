 
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
   public partial class PaymentMethodTypeRepository:IRepository<PaymentMethodType>
   {
   
        private ICustomContext currentContext;
        public PaymentMethodTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentMethodTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentMethodType GetSingle(string code)
        {
            return (from a in context.PaymentMethodTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentMethodType> GetAll()
        {
            return from a in context.PaymentMethodTypes  
                   select a;
        }
				 
        public PaymentMethodType GetSingle(EntityKeyFields entityKeys)
        {
            PaymentMethodTypeKeys keys = entityKeys as PaymentMethodTypeKeys;
            return (from a in context.PaymentMethodTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentMethodType entity)
        {
            onAdd();
            context.PaymentMethodTypes.Add(entity);
        }

        public void Remove(PaymentMethodType entity)
        {
            context.PaymentMethodTypes.Attach(entity);
            context.PaymentMethodTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentMethodType entity)
        {
            onUpdate();
            context.PaymentMethodTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentMethodType> All()
        {
            return context.PaymentMethodTypes.ToList();
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
	 