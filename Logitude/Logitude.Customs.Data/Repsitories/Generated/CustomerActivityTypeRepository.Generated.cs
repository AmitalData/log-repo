 
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
   public partial class CustomerActivityTypeRepository:IRepository<CustomerActivityType>
   {
   
        private ICustomContext currentContext;
        public CustomerActivityTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomerActivityTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerActivityType GetSingle(string code)
        {
            return (from a in context.CustomerActivityTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerActivityType> GetAll()
        {
            return from a in context.CustomerActivityTypes  
                   select a;
        }
				 
        public CustomerActivityType GetSingle(EntityKeyFields entityKeys)
        {
            CustomerActivityTypeKeys keys = entityKeys as CustomerActivityTypeKeys;
            return (from a in context.CustomerActivityTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerActivityType entity)
        {
            onAdd();
            context.CustomerActivityTypes.Add(entity);
        }

        public void Remove(CustomerActivityType entity)
        {
            context.CustomerActivityTypes.Attach(entity);
            context.CustomerActivityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerActivityType entity)
        {
            onUpdate();
            context.CustomerActivityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerActivityType> All()
        {
            return context.CustomerActivityTypes.ToList();
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
	 