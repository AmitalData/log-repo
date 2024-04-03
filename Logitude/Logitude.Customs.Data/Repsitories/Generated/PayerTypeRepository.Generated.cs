 
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
   public partial class PayerTypeRepository:IRepository<PayerType>
   {
   
        private ICustomContext currentContext;
        public PayerTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PayerTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PayerType GetSingle(string code)
        {
            return (from a in context.PayerTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PayerType> GetAll()
        {
            return from a in context.PayerTypes  
                   select a;
        }
				 
        public PayerType GetSingle(EntityKeyFields entityKeys)
        {
            PayerTypeKeys keys = entityKeys as PayerTypeKeys;
            return (from a in context.PayerTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PayerType entity)
        {
            onAdd();
            context.PayerTypes.Add(entity);
        }

        public void Remove(PayerType entity)
        {
            context.PayerTypes.Attach(entity);
            context.PayerTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PayerType entity)
        {
            onUpdate();
            context.PayerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PayerType> All()
        {
            return context.PayerTypes.ToList();
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
	 