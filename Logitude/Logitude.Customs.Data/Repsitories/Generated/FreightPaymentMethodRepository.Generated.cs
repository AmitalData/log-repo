 
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
   public partial class FreightPaymentMethodRepository:IRepository<FreightPaymentMethod>
   {
   
        private ICustomContext currentContext;
        public FreightPaymentMethodRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public FreightPaymentMethodRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  FreightPaymentMethod GetSingle(string code)
        {
            return (from a in context.FreightPaymentMethods
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FreightPaymentMethod> GetAll()
        {
            return from a in context.FreightPaymentMethods  
                   select a;
        }
				 
        public FreightPaymentMethod GetSingle(EntityKeyFields entityKeys)
        {
            FreightPaymentMethodKeys keys = entityKeys as FreightPaymentMethodKeys;
            return (from a in context.FreightPaymentMethods
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FreightPaymentMethod entity)
        {
            onAdd();
            context.FreightPaymentMethods.Add(entity);
        }

        public void Remove(FreightPaymentMethod entity)
        {
            context.FreightPaymentMethods.Attach(entity);
            context.FreightPaymentMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FreightPaymentMethod entity)
        {
            onUpdate();
            context.FreightPaymentMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FreightPaymentMethod> All()
        {
            return context.FreightPaymentMethods.ToList();
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
	 