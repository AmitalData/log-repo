 
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
   public partial class PaymentProtestTypeRepository:IRepository<PaymentProtestType>
   {
   
        private ICustomContext currentContext;
        public PaymentProtestTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentProtestTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentProtestType GetSingle(string code)
        {
            return (from a in context.PaymentProtestTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentProtestType> GetAll()
        {
            return from a in context.PaymentProtestTypes  
                   select a;
        }
				 
        public PaymentProtestType GetSingle(EntityKeyFields entityKeys)
        {
            PaymentProtestTypeKeys keys = entityKeys as PaymentProtestTypeKeys;
            return (from a in context.PaymentProtestTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentProtestType entity)
        {
            onAdd();
            context.PaymentProtestTypes.Add(entity);
        }

        public void Remove(PaymentProtestType entity)
        {
            context.PaymentProtestTypes.Attach(entity);
            context.PaymentProtestTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentProtestType entity)
        {
            onUpdate();
            context.PaymentProtestTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentProtestType> All()
        {
            return context.PaymentProtestTypes.ToList();
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
	 