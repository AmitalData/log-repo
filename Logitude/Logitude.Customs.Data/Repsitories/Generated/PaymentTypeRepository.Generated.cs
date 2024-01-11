 
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
   public partial class PaymentTypeRepository:IRepository<PaymentType>
   {
   
        private ICustomContext currentContext;
        public PaymentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentType GetSingle(string code)
        {
            return (from a in context.PaymentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentType> GetAll()
        {
            return from a in context.PaymentTypes  
                   select a;
        }
				 
        public PaymentType GetSingle(EntityKeyFields entityKeys)
        {
            PaymentTypeKeys keys = entityKeys as PaymentTypeKeys;
            return (from a in context.PaymentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentType entity)
        {
            onAdd();
            context.PaymentTypes.Add(entity);
        }

        public void Remove(PaymentType entity)
        {
            context.PaymentTypes.Attach(entity);
            context.PaymentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentType entity)
        {
            onUpdate();
            context.PaymentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentType> All()
        {
            return context.PaymentTypes.ToList();
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
	 