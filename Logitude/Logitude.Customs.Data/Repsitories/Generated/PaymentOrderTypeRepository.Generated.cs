 
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
   public partial class PaymentOrderTypeRepository:IRepository<PaymentOrderType>
   {
   
        private ICustomContext currentContext;
        public PaymentOrderTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentOrderTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentOrderType GetSingle(string code)
        {
            return (from a in context.PaymentOrderTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentOrderType> GetAll()
        {
            return from a in context.PaymentOrderTypes  
                   select a;
        }
				 
        public PaymentOrderType GetSingle(EntityKeyFields entityKeys)
        {
            PaymentOrderTypeKeys keys = entityKeys as PaymentOrderTypeKeys;
            return (from a in context.PaymentOrderTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentOrderType entity)
        {
            onAdd();
            context.PaymentOrderTypes.Add(entity);
        }

        public void Remove(PaymentOrderType entity)
        {
            context.PaymentOrderTypes.Attach(entity);
            context.PaymentOrderTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentOrderType entity)
        {
            onUpdate();
            context.PaymentOrderTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentOrderType> All()
        {
            return context.PaymentOrderTypes.ToList();
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
	 