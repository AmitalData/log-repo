 
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
   public partial class PayerActivityTypeRepository:IRepository<PayerActivityType>
   {
   
        private ICustomContext currentContext;
        public PayerActivityTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PayerActivityTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PayerActivityType GetSingle(string code)
        {
            return (from a in context.PayerActivityTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PayerActivityType> GetAll()
        {
            return from a in context.PayerActivityTypes  
                   select a;
        }
				 
        public PayerActivityType GetSingle(EntityKeyFields entityKeys)
        {
            PayerActivityTypeKeys keys = entityKeys as PayerActivityTypeKeys;
            return (from a in context.PayerActivityTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PayerActivityType entity)
        {
            onAdd();
            context.PayerActivityTypes.Add(entity);
        }

        public void Remove(PayerActivityType entity)
        {
            context.PayerActivityTypes.Attach(entity);
            context.PayerActivityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PayerActivityType entity)
        {
            onUpdate();
            context.PayerActivityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PayerActivityType> All()
        {
            return context.PayerActivityTypes.ToList();
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
	 